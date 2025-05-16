using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 100;           //���� ü��
    public int currentHealth = 100;         //���� ü��
    public Slider healthSlider;             //ü�� UI�� ����
    public Image damageImage;               //������ ������ �̹���
    public AudioClip deathClip;             //�÷��̾� ������ �Ҹ�

    Animator animator;                          //�ִϸ�����
    AudioSource playerAudio;                //����� �ҽ�
    PlayerMovement playerMovement;          //�÷��̾� ������
    bool isDead = false;                            //���� Ȯ�ο� ����

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        currentHealth = startHealth;
    }
    //�÷��̾ ������ ������ ȣ���� �Լ�
    public void TakeDamage(int amount)
    {
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
        isDead = true;

        animator.SetTrigger("Die");

        playerMovement.enabled = false;

    }

}
