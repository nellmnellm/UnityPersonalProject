using System.Collections;
using System.Threading;
using UnityEngine;

// 컴포넌트 (Component) -> 유니티 오브젝트가 사용할 기능.
// 기본 제공 컴포넌트도 있고, 스크립트는 사용자가 만들어주는 정의 .(MonoBehaviour 상속)
// MonoBehaviour 상속
// 유니티 오브젝트에 해당 클래스를 컴포넌트로써 등록할 수 있음.
public class Monster : Unit
{
    //range => 유니티 인스펙터에 해당 필드 값에 대한 범위 설정.
    [Range(1,5)] public float speed;

 
    // TODO : 몬스터 클래스에서 상황에 맞게 애니메이션 실행
    // 

    bool isSpawn = false;

    Vector3 playerVector3 = new Vector3(5, 0, -5);

    IEnumerator Onspawn()
    {
        float current = 0f; //시간 저장용
        float percent = 0f; // 반복문의 종료 조건.
        float start = 0f;
        float end = transform.localScale.x;
        //localscale은 상대적인 크기(오브젝트의 크기)
        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 3.0f;

            var pos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(pos, pos, pos);
            yield return null; //탈출했다가 돌아오게됨
        }
        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }


    protected override void Start()
    {
        base.Start(); // Unit의 start 호출.
      /*  StartCoroutine(Onspawn());
        MonsterInit();*/
        HP = 5.0f;
        GetDamage(5.0f);
        
    }
    


    public void GetDamage(double dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            var effect = Manager.Pool.pooling("Effect01").get(
                (value) => value.transform.position = transform.position);


           /* var eff = Resources.Load<GameObject>(effect.name);
            Instantiate(eff, transform.position, Quaternion.identity);*/
        }
            
          
        
    }
    public void MonsterInit() => StartCoroutine(Onspawn());


    // 유니티 라이프 싸이클 함수 (생명 주기)
    private void Update()
    {
        transform.LookAt(playerVector3); //특정 방향을 바라보게 설정하는 기능.
        if (isSpawn == false)
            return;
  
            //var => 파라미터로는 적을 수 없음. 
        var distance = Vector3.Distance(transform.position, playerVector3);

        //기준보다 측정 거리가 작을 경우
        if (distance < 0.6f)
        {
            SetAnimator("IsIDLE"); //대기 모드로 변경
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerVector3, Time.deltaTime * speed);
            SetAnimator("IsMOVE"); //이동 모드로 변경
        }

    }

    

    
    #region
    //굿

    #endregion
}
