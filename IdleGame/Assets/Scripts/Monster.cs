using System.Collections;
using System.Threading;
using Unity.VisualScripting;
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
    bool isDead = false;
    

    public WeaponEnhancer enhancer;
    

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
        HP = 10.0f;
        //GetDamage(5.0f);
        if (enhancer == null)
        {
            enhancer = FindObjectOfType<WeaponEnhancer>();
        }
    }
    
    public GameObject effect;


    public void GetDamage(double dmg)
    {
        dmg += (double)enhancer.weaponLevel;
        //죽었다면 호출되지 않게
        if (isDead)
        {
            
            return;
        }


        
        //HitText 처리
        Manager.Pool.pooling("Hit").get((value) =>
        {
            value.GetComponent<HitText>().Init(transform.position, dmg);

        });
        HP -= dmg;

        if (HP <= 0)
        {
            var effect = Manager.Pool.pooling("Effect01").get(
                (value) => value.transform.position = new Vector3(transform.position.x,
                                                                  transform.position.y,
                                                                  transform.position.z));

            //gameObject.SetActive(false);
            /* var eff = Resources.Load<GameObject>(effect.name);
             Instantiate(eff, transform.position, Quaternion.identity);*/
            Manager.Pool.pooling("CoinMove").get(value =>
            {
                value.GetComponent<CoinMove>().Init(transform.position);
            });

            for (int i=0; i<4; i++)
            {
                Manager.Pool.pooling("ItemObject").get((value) =>
                {
                    value.GetComponent<Item_Object>().Init(transform.position);
                });


            }
            Manager.Pool.pool_dict["Monster"].Release(gameObject);
            //gameObject.SetActive(false); 
        }
            
          
        
    }
    public void MonsterInit() => StartCoroutine(Onspawn());

    
    // 유니티 라이프 싸이클 함수 (생명 주기)
    private void Update()
    {
        transform.LookAt(Vector3.zero); //특정 방향을 바라보게 설정하는 기능.
        if (isSpawn == false)
            return;
  
            //var => 파라미터로는 적을 수 없음. 
        var distance = Vector3.Distance(transform.position, Vector3.zero);

        //기준보다 측정 거리가 작을 경우
        if (distance < 0.6f)
        {
            SetAnimator("IsIDLE"); //대기 모드로 변경
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * speed);
            SetAnimator("IsMOVE"); //이동 모드로 변경
        }


       /* if (Input.GetKeyDown(KeyCode.A))
        {
            GetDamage(1);
        }*/
    }

    
   
    
    #region
    //굿

    #endregion
}
