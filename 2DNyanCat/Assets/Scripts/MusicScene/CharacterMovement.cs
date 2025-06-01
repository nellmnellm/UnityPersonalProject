using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 20f;

    private InputSystem_Actions inputActions;
    private Animator animator;

    public JudgeZone[] judgeZones;

    private float minY = -4.2f;
    private float maxY = 4.2f;

    private bool isJumping = false;
    private bool isGrounded = false;

    private Coroutine jumpCoroutine;

    private void Awake()
    {
        inputActions = new InputSystem_Actions(); 
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        inputActions.GamePlay.Enable(); // Action Map 이름이 Gameplay일 경우
        inputActions.GamePlay.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.GamePlay.Attack.performed -= OnAttack;
        inputActions.GamePlay.Disable();
    }

    void Start()
    {
        // 처음 시작 위치
        transform.position = new Vector2(-2f, transform.position.y);
    }
    private void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("isAttack");

        //context.time = 받는 순간
        double performedTime = context.time;

        for (int i = 0; i < judgeZones.Length; i++)
        {
            var note = judgeZones[i].GetFirstNote();
            if (note != null)
            {
                /*Debug.Log($"context.time = {context.time:F6}");
                Debug.Log($"context.startTime = {context.startTime:F6}");
                Debug.Log($"Frame time = {Time.unscaledTime:F6}");*/
                float offset = (float)(context.time - GameManager.Instance.RealtimeStartTime) - note.HitTime;
                GameManager.Instance.ShowJudgementOffset(offset);
                Judgement result = (Judgement)i;
                ScoreManager.Instance.RegisterJudgement(result);
                note.OnHit();
                return;
            }
        }

        ScoreManager.Instance.RegisterJudgement(Judgement.Miss);
    }

    private void HandleInput()
    {
        float y = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            y = 1f;
        else if (Input.GetKey(KeyCode.DownArrow))
            y = -1f;
        Vector3 newPos = transform.position + Vector3.up * y * Time.deltaTime * moveSpeed;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        transform.position = new Vector3(-2f, newPos.y, 0f);
        // 땅에 닿았는지 체크
        isGrounded = Mathf.Abs(transform.position.y - minY) < 0.2f;
        // 점프 조건: F 키, 아래쪽에 있을 때만
        if (Input.GetKeyDown(KeyCode.F) && isGrounded && jumpCoroutine == null)
        {
            isJumping = true;
            jumpCoroutine = StartCoroutine(JumpUp());
        }

        // Space를 누르면 바로 바닥으로
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(-2f, minY, 0f);
        }

        
        

        //공격키 (<, >) . 공격할때의 애니메이터 설정. 
        /*if (Input.GetKeyDown(KeyCode.D)) //추후 변경예정 // 에디터에서는 S 불가!
        {
            animator.SetTrigger("isAttack");
            for (int i = 0; i < judgeZones.Length; i++)
            {
                var note = judgeZones[i].GetFirstNote();
                if (note != null)
                {
                    // 판정!
                    Judgement result = (Judgement)i; // 0=Perfect, 1=Great, 2=Good
                    ScoreManager.Instance.RegisterJudgement(result);

                    note.OnHit(); // 제거 또는 이펙트
                    return; // 여기서 바로 return: 한 번만 처리하고 끝냄
                }
            }

            // 아무 노트도 없으면 Miss
            ScoreManager.Instance.RegisterJudgement(Judgement.Miss);
        }*/
    }


    private IEnumerator JumpUp()
    {
        
        float duration = 0.1f;
        float elapsed = 0f;

        Vector3 start = transform.position;
        Vector3 end = new Vector3(-2f, Mathf.Clamp(start.y + 5f, minY, maxY), 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        isJumping = false;
        transform.position = end;
        jumpCoroutine = null;
    }


    private void UpdateAnimator()
    {

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isJumping", isJumping);
    }

   
   
}