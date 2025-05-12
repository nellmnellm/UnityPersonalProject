using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject prefab;
    public int count = 20;
    public float radius = 5.0f;

    private void Start()
    {
        for(int i=0; i<count;i++)
        {
            float radian = i * Mathf.PI * 2 / count ;
            float x = Mathf.Cos(radian) * radius;
            float y = Mathf.Sin(radian) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float degree = radian * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, degree);

            Instantiate(prefab, pos, rotation);

        }
    }
}
