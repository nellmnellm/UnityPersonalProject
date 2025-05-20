using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 100;           //시작 체력
    public int currentHealth = 100;         //현재 체력
    public Slider healthSlider;             //체력 UI와 연결
    public Image damageImage;               //데미지 입을때 이미지
    public AudioClip deathClip;             //플레이어 죽을시 소리

    public float flashSpeed = 5.0f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f); //변경될 색

    Animator animator;                          //애니메이터
    AudioSource playerAudio;                //오디오 소스
    PlayerMovement playerMovement;          //플레이어 움직임
    bool isDead;                            //죽음 확인용 변수
    bool damaged;                          //데미지 확인용
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        currentHealth = startHealth;
    }

    private void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }



    //플레이어가 데미지 받으면 호출할 함수
    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
        else
        {
            animator.SetTrigger("Damage");
        }

    }

    void Death()
    {
        StageController.Instance.FinishGame();
        isDead = true;

        animator.SetTrigger("Die");

        playerMovement.enabled = false;

    }

}
