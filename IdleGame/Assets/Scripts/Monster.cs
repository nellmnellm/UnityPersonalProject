using System.Collections;
using System.Threading;
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

    Vector3 playerVector3 = new Vector3(5, 0, -5);

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


    // ����Ƽ ������ ����Ŭ �Լ� (���� �ֱ�)
    private void Update()
    {
        transform.LookAt(playerVector3); //Ư�� ������ �ٶ󺸰� �����ϴ� ���.
        if (isSpawn == false)
            return;
  
            //var => �Ķ���ͷδ� ���� �� ����. 
        var distance = Vector3.Distance(transform.position, playerVector3);

        //���غ��� ���� �Ÿ��� ���� ���
        if (distance < 0.6f)
        {
            SetAnimator("IsIDLE"); //��� ���� ����
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerVector3, Time.deltaTime * speed);
            SetAnimator("IsMOVE"); //�̵� ���� ����
        }

    }

    

    
    #region
    //��

    #endregion
}
