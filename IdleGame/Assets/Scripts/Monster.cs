using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

// ������Ʈ (Component) -> ����Ƽ ������Ʈ�� ����� ���.
// �⺻ ���� ������Ʈ�� �ְ�, ��ũ��Ʈ�� ����ڰ� ������ִ� ���� .(MonoBehaviour ���)
// MonoBehaviour ���
// ����Ƽ ������Ʈ�� �ش� Ŭ������ ������Ʈ�ν� ����� �� ����.
public class Monster : Unit
{
    //range => ����Ƽ �ν����Ϳ� �ش� �ʵ� ���� ���� ���� ����.
    [Range(1,5)] public float speed;

 
    // TODO : ���� Ŭ�������� ��Ȳ�� �°� �ִϸ��̼� ����
    // 

    bool isSpawn = false;
    bool isDead = false;
    

    public WeaponEnhancer enhancer;
    

    IEnumerator Onspawn()
    {
        float current = 0f; //�ð� �����
        float percent = 0f; // �ݺ����� ���� ����.
        float start = 0f;
        float end = transform.localScale.x;
        //localscale�� ������� ũ��(������Ʈ�� ũ��)
        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 3.0f;

            var pos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(pos, pos, pos);
            yield return null; //Ż���ߴٰ� ���ƿ��Ե�
        }
        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }


    protected override void Start()
    {
        base.Start(); // Unit�� start ȣ��.
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
        //�׾��ٸ� ȣ����� �ʰ�
        if (isDead)
        {
            
            return;
        }


        
        //HitText ó��
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

    
    // ����Ƽ ������ ����Ŭ �Լ� (���� �ֱ�)
    private void Update()
    {
        transform.LookAt(Vector3.zero); //Ư�� ������ �ٶ󺸰� �����ϴ� ���.
        if (isSpawn == false)
            return;
  
            //var => �Ķ���ͷδ� ���� �� ����. 
        var distance = Vector3.Distance(transform.position, Vector3.zero);

        //���غ��� ���� �Ÿ��� ���� ���
        if (distance < 0.6f)
        {
            SetAnimator("IsIDLE"); //��� ���� ����
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * speed);
            SetAnimator("IsMOVE"); //�̵� ���� ����
        }


       /* if (Input.GetKeyDown(KeyCode.A))
        {
            GetDamage(1);
        }*/
    }

    
   
    
    #region
    //��

    #endregion
}
