using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller; //������Ʈ
    public Animator animator;
    public ScoreManager scoreManager;

    [SerializeField] private Vector3 moveVector;              // ���� ����
    [SerializeField] private float vertical_velocity = 0.0f;  // ������ ���� ���� �ӵ�
    [SerializeField] private float gravity = 12.0f;           // �߷� ��

    [SerializeField] private float speed = 5.0f;              // �÷��̾��� �̵� �ӵ�
    [SerializeField] private float jumpHeight = 7.0f;

    

    [SerializeField] private bool isDead = false;


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed() => speed;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            scoreManager.OnDead();
            return;
        }


        if (transform.position.y < -10f) // ����
        {
            isDead = true;
            return;
        }

        if (transform.position.y > 20f) // �ϴÿ� �ʹ���
        {
            isDead = true;
            return;
        }

        animator.SetBool("IsRun", false);
        //ī�޶� ��Ʈ�ѷ��� �̿��� �÷��̾� ������ ���� ī�޶� ������ �����غ��� �մϴ�.
        if (Time.timeSinceLevelLoad < CameraController.camera_animate_duration)
        {

            return;
        }
        animator.SetBool("IsRun", true);

        float moveX = Input.GetAxisRaw("Horizontal") * speed;

        // �߷�/���� ó��
        if (controller.isGrounded)
        {
            animator.SetBool("IsJump", false);
            vertical_velocity = -1f; // �ٴڿ� ��¦ ���� �ٰ�

            if (Input.GetKeyDown(KeyCode.Space))
            {
                vertical_velocity = jumpHeight;
                animator.SetBool("IsJump", true);
            }
        }
        else
        {
            vertical_velocity -= gravity * Time.deltaTime;
        }

        // ���� �̵� ���� ����
        moveVector = new Vector3(moveX, vertical_velocity, speed);

        // ���� �̵�
        controller.Move(moveVector * Time.deltaTime);
    }
        /*
        controller.Move(Vector3.forward * speed * Time.deltaTime);
        moveVector = Vector3.zero; //���� ���� �� ����
        //���� ������� ��� velocity ����
        if (controller.transform.position.y > -5)
        {
            vertical_velocity = 0.0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 move = new Vector3(0, jump, 0);
                controller.Move(move * Time.deltaTime);
            }
        }
        
            //�ƴ� ��� �߷�ġ��ŭ ����������
            vertical_velocity -= gravity * Time.deltaTime;
        
        //1. �¿� �̵�
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed;
        //2. ���� ����
        moveVector.y = vertical_velocity;
        //3. ������ �̵�
        moveVector.z = speed;
        //������ ������ �̵� ���� 
        controller.Move(moveVector * Time.deltaTime);*/


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            isDead = true;
            scoreManager.OnDead();
            //�浹�ϸ� �ٷ� �״� �̺�Ʈ�� ���� 
        }
    }
    
}

