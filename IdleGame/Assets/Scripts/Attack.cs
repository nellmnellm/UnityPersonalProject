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

    //���ݽ� ������ ������Ʈ
    Dictionary<string, GameObject> attacks = new Dictionary<string, GameObject>();
    //������ �����ϸ� ����� ����Ʈ
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
        //���޹��� Ű�� ���ݿ� ���� ������Ʈ Ȱ��ȭ
        attacks[attack_key].gameObject.SetActive(true);

    }

    private void Update()
    {
        //hit�� true�ΰ�� return�ؼ� �ٽ� �ҷ����� �ʰ�
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
                //������ ������ �Լ��� ó���ǵ��� ����.
                attacks[attack_key].gameObject.SetActive(false);
                //���� ���� ����Ʈ �÷���
                attacks_enter[attack_key].Play();
                StartCoroutine(ReleaseObject(attacks_enter[attack_key].main.duration));
                //particle duration���� ���� ����. Update�� Ư�� �����϶� ����Ǵ� ���ؿ����� ���
            }


        }
        
    }


    //�����ð� �� ������Ʈ �ݳ�
    IEnumerator ReleaseObject(float time)
    {
        yield return new WaitForSeconds(time);
        Manager.Pool.pool_dict["Attack"].Release(gameObject);
        //yield return null;
    }
}
