using UnityEngine;

public class Note : MonoBehaviour
{
    private float judgeX = -2f;
    private float hitTime;

    public float HitTime => hitTime;
    void Update()
    {
        
        float songTime = GameManager.Instance.SongTime;
        float noteSpeed = SettingManager.Instance.playerSettings.noteSpeed * 2.5f;
        float distance = (hitTime - songTime) * noteSpeed;

        
        transform.position = new Vector3(judgeX + distance, transform.position.y, 0f);

        if (transform.position.x < -5f)
        {
            Destroy(gameObject); // �Ǵ� Miss ó��
            ScoreManager.Instance.RegisterJudgement(Judgement.Miss);
        }
    }

    public void Init(float targetTime)
    {
        hitTime = targetTime;
    }

    public void OnHit()
    {
        Destroy(gameObject);
    }


}