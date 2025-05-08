using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller; //컴포넌트
    public Animator animator;
    public ScoreManager scoreManager;

    [SerializeField] private Vector3 moveVector;              // 방향 벡터
    [SerializeField] private float vertical_velocity = 0.0f;  // 점프를 위한 수직 속도
    [SerializeField] private float gravity = 12.0f;           // 중력 값

    [SerializeField] private float speed = 5.0f;              // 플레이어의 이동 속도
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


        if (transform.position.y < -10f) // 낙사
        {
            isDead = true;
            return;
        }

        if (transform.position.y > 20f) // 하늘에 너무뜸
        {
            isDead = true;
            return;
        }

        animator.SetBool("IsRun", false);
        //카메라 컨트롤러를 이용해 플레이어 움직임 전에 카메라 연출을 진행해보려 합니다.
        if (Time.timeSinceLevelLoad < CameraController.camera_animate_duration)
        {

            return;
        }
        animator.SetBool("IsRun", true);

        float moveX = Input.GetAxisRaw("Horizontal") * speed;

        // 중력/점프 처리
        if (controller.isGrounded)
        {
            animator.SetBool("IsJump", false);
            vertical_velocity = -1f; // 바닥에 살짝 눌러 붙게

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

        // 최종 이동 방향 구성
        moveVector = new Vector3(moveX, vertical_velocity, speed);

        // 실제 이동
        controller.Move(moveVector * Time.deltaTime);
    }
        /*
        controller.Move(Vector3.forward * speed * Time.deltaTime);
        moveVector = Vector3.zero; //방향 벡터 값 리셋
        //땅에 닿아있을 경우 velocity 고정
        if (controller.transform.position.y > -5)
        {
            vertical_velocity = 0.0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 move = new Vector3(0, jump, 0);
                controller.Move(move * Time.deltaTime);
            }
        }
        
            //아닐 경우 중력치만큼 떨어지도록
            vertical_velocity -= gravity * Time.deltaTime;
        
        //1. 좌우 이동
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed;
        //2. 점프 관련
        moveVector.y = vertical_velocity;
        //3. 앞으로 이동
        moveVector.z = speed;
        //설정한 방향대로 이동 진행 
        controller.Move(moveVector * Time.deltaTime);*/


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            isDead = true;
            scoreManager.OnDead();
            //충돌하면 바로 죽는 이벤트로 진행 
        }
    }
    
}

