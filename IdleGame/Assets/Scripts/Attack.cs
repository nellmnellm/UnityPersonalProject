using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Attack : MonoBehaviour
{
    public Transform target;
    public float move_speed;
    public Vector3 target_pos;
    double damage;
    public string attack_key;
    public bool hit = false;

    //공격시 생성될 오브젝트
    Dictionary<string, GameObject> attacks = new Dictionary<string, GameObject>();
    //공격이 적중하면 적용될 이펙트
    Dictionary<string, ParticleSystem> attacks_enter = new Dictionary<string, ParticleSystem>();


    private void Awake()
    {
        var attacks_trans = transform.GetChild(0);
        var onattacks_trans = transform.GetChild(1);

        for (int i=0; i<attacks_trans.childCount; i++)
        {
            attacks.Add(attacks_trans.GetChild(i).name, attacks_trans.GetChild(i).gameObject);
        }

        for (int i=0; i<onattacks_trans.childCount; i++)
        {
            attacks_enter.Add(onattacks_trans.GetChild(i).name, onattacks_trans.GetChild(i).GetComponent<ParticleSystem>());
        }
    }
    public void Init(Transform t, double dmg, string key)
    {
        target = t;
        transform.LookAt(target);

        hit = false;

        damage = dmg;

        attack_key = key;
        //전달받은 키의 공격용 게임 오브젝트 활성화
        attacks[attack_key].gameObject.SetActive(true);

    }

    private void Update()
    {
        //hit가 true인경우 return해서 다시 불러오지 않게
        if (hit) return;

        //target_pos.y = 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, target_pos, move_speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target_pos) <= 0.5f)
        {
            if (target != null)
            {

                hit = true;

                target.GetComponent<Unit>().HP -= damage;

                target.GetComponent<Monster>().GetDamage(damage);
                //몬스터의 데미지 함수가 처리되도록 설정.
                attacks[attack_key].gameObject.SetActive(false);
                //공격 명중 이펙트 플레이
                attacks_enter[attack_key].Play();
                StartCoroutine(ReleaseObject(attacks_enter[attack_key].main.duration));
                //particle duration으로 접근 가능. Update가 특정 조건일때 실행되는 수준에서는 사용
            }


        }
        
    }


    //일정시간 뒤 오브젝트 반납
    IEnumerator ReleaseObject(float time)
    {
        yield return new WaitForSeconds(time);
        Manager.Pool.pool_dict["Attack"].Release(gameObject);
        //yield return null;
    }
}
