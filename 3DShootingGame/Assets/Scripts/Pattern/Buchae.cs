using UnityEngine;

public class Buchae : MonoBehaviour
{
    public GameObject prefab;
    public int count = 5; //퍼지는 총알 갯수
    public int angle = 20; //각도 조절
    public int baseAngle = -40; //시작각도
    public int overlap = 3;
    public float radius = 5.0f;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            
            float radian = i * Mathf.PI * 2 / count;
            float x = Mathf.Cos(baseAngle) * radius;
            float y = Mathf.Sin(baseAngle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float degree = baseAngle * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, degree);

            for (int j = overlap; j > 0; j--)
                Instantiate(prefab, pos * overlap, rotation);

            baseAngle += angle;
        } // 수정예정
    }
}
