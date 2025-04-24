using System.Collections;
using System.Threading;
using UnityEngine;

// ������Ʈ (Component) -> ����Ƽ ������Ʈ�� ����� ���.
// �⺻ ���� ������Ʈ�� �ְ�, ��ũ��Ʈ�� ����ڰ� ������ִ� ���� .(MonoBehaviour ���)
// MonoBehaviour ���
// ����Ƽ ������Ʈ�� �ش� Ŭ������ ������Ʈ�ν� ����� �� ����.
public class Monster : MonoBehaviour
{
    //range => ����Ƽ �ν����Ϳ� �ش� �ʵ� ���� ���� ���� ����.
    [Range(1,5)] public float speed;

    Animator animator;
    // TODO : ���� Ŭ�������� ��Ȳ�� �°� �ִϸ��̼� ����
    // 

    bool isSpawn = false;

    Vector3 playerVector3 = new Vector3(5, 0, -5);

    private void Start()
    {
        animator = GetComponent<Animator>();
        //�ڵ� ������ animator�� �ν��ϰ� �ʵ峪 �޼��� ��밡��.
        StartCoroutine(SpawnStart());
        
    }

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

    private void SetAnimator (string temp)
    {
        //�⺻ �Ķ���Ϳ� ���� reset
        animator.SetBool("IsIDLE", false);
        animator.SetBool("IsMOVE", false);
        //���ڷ� ���޹��� ���� true��
        animator.SetBool(temp, true);
    }

    IEnumerator SpawnStart()
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

    #region
    //��

    #endregion
}
