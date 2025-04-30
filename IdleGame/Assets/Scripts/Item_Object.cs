using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Item_Object : MonoBehaviour
{

    public Transform ItemText;
    public Text text;
    public float angle = 45.0f;
    public float gravity = 9.8f;
    public float range = 2.0f;

    private bool isCheck = false;
    //아이템 레어도
    void ItemRare()
    {

        isCheck = true;
        
        transform.rotation = Quaternion.identity;
        
        ItemText.gameObject.SetActive(true);
        ItemText.parent = B_Canvas.Instance.GetLayer(2);
        text.text = "아이템";



    }

   
    private void Update()
    {

        if (isCheck = false)
            return;
        
        ItemText.position = Camera.main.WorldToScreenPoint(transform.position); 
    }
    public void Init(Vector3 pos)
    {
        Vector3 item_pos = new Vector3(pos.x + (Random.insideUnitSphere.x) * range,
                                       0f,
                                       pos.z + (Random.insideUnitSphere.z * range));

        //전달받은 값 기준 주변에 위치할수 있도록
        StartCoroutine(Simulate(pos));
    }
    IEnumerator Simulate(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);
        float radian = angle * Mathf.Deg2Rad; //라디안 변환 값
        float velocity = Mathf.Sqrt(target_Distance * gravity / Mathf.Sin(2 * radian));

        float vx = velocity * Mathf.Cos(radian);
        float vy = velocity * Mathf.Sin(radian);

        float duration = target_Distance / vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);
        //LookAt 처럼 회전 방향 바라보게 만드는 코드

        float simulate_time = 0.0f;

        while (simulate_time < duration)
        {
            simulate_time += Time.deltaTime;

            //시간이 지날수록 위에서 점점 아래로, 밑변 방향으로 이동
            transform.Translate(0, (vy - (gravity * simulate_time)), vx * Time.deltaTime);
            yield return null;
        }
        //아이템 이동 시뮬레이션이 끝나면 레어도 체크 후 화면에 아이템 이름 띄우기
        ItemRare();
    }

}
