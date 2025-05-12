using UnityEngine;

public class Star : MonoBehaviour
{
    public GameObject prefab;
    public int count = 10;
    public float radius = 5.0f;
    
    private void Start()
    {
        float smallRadius = radius * 0.382f;
        float middleRadius = radius * 0.691f;
        for (int i = 0; i < count; i++)
        {

            float radian = i * Mathf.PI * 2 / count;
            float radian2 = i * Mathf.PI * 2 / count + Mathf.PI / count;
            float x;
            float y;
            if (i % 2 == 0)
            {
                x = Mathf.Cos(radian) * radius;
                y = Mathf.Sin(radian) * radius;
            }
            else
            {
                x = Mathf.Cos(radian) * smallRadius;
                y = Mathf.Sin(radian) * smallRadius;
            }

            float x2 = Mathf.Cos(radian2) * middleRadius;
            float y2 = Mathf.Sin(radian2) * middleRadius;

            Vector3 pos = transform.position + new Vector3(x, y, 0);
            Vector3 pos2 = transform.position + new Vector3(x2, y2, 0);

            //float degree = radian * Mathf.Rad2Deg;

            //Quaternion rotation = Quaternion.Euler(0, 0, degree);

            Instantiate(prefab, pos, Quaternion.identity);
            Instantiate(prefab, pos2, Quaternion.identity);

        }
    }
}



