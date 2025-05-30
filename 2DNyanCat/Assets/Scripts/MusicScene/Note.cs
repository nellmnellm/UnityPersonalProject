using UnityEngine;

public class Note : MonoBehaviour
{
    private float noteSpeed = 5f;
    private float judgeX = -2f;
    private float hitTime;

    void Update()
    {
        float songTime = GameManager.Instance.SongTime;
        float distance = (hitTime - songTime) * noteSpeed;

        transform.position = new Vector3(judgeX + distance, transform.position.y, 0f);

        if (transform.position.x < -10f)
        {
            Destroy(gameObject); // 또는 Miss 처리
        }
    }

    public void Init(float targetTime)
    {
        hitTime = targetTime;
    }

    public void OnHit()
    {
        Destroy(gameObject); // 또는 ObjectPool 반환
    }


}