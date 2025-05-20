using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 100;           //���� ü��
    public int currentHealth = 100;         //���� ü��
    public Slider healthSlider;             //ü�� UI�� ����
    public Image damageImage;               //������ ������ �̹���
    public AudioClip deathClip;             //�÷��̾� ������ �Ҹ�

    public float flashSpeed = 5.0f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f); //����� ��

    Animator animator;                          //�ִϸ�����
    AudioSource playerAudio;                //����� �ҽ�
    PlayerMovement playerMovement;          //�÷��̾� ������
    bool isDead;                            //���� Ȯ�ο� ����
    bool damaged;                          //������ Ȯ�ο�
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



    //�÷��̾ ������ ������ ȣ���� �Լ�
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
