using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    public InputBindingsData bindingsData; // 인스펙터에서 연결
    private Animator animator;

    public JudgeZone[] judgeZones;

    private float minY = -4.2f;
    private float maxY = 4.2f;

    private bool isJumping = false;
    private bool isGrounded = false;

    private Coroutine jumpCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        bindingsData.attack1.action.Enable(); // Action Map 이름이 Gameplay일 경우
        bindingsData.attack2.action.Enable();
        bindingsData.jump.action.Enable();
        bindingsData.ground.action.Enable();
        bindingsData.move.action.Enable();

        bindingsData.attack1.action.performed += OnAttack;
        bindingsData.attack2.action.performed += OnAttack;
        bindingsData.jump.action.performed += OnJump;
        bindingsData.ground.action.performed += OnFall;
    }

    private void OnDisable()
    {
        bindingsData.attack1.action.performed -= OnAttack;
        bindingsData.attack2.action.performed -= OnAttack;
        bindingsData.jump.action.performed -= OnJump;
        bindingsData.ground.action.performed -= OnFall;

        bindingsData.attack1.action.Disable();
        bindingsData.attack2.action.Disable();
        bindingsData.jump.action.Disable();
        bindingsData.ground.action.Disable();
        bindingsData.move.action.Disable();
    }

    void Start()
    {
        // 처음 시작 위치
        transform.position = new Vector2(-2f, transform.position.y);
    }
    private void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        float yInput = bindingsData.move.action.ReadValue<float>(); // Axis (Up/Down)
        Vector3 newPos = transform.position + Vector3.up * yInput * Time.deltaTime * moveSpeed;

        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        transform.position = new Vector3(-2f, newPos.y, 0f);

        isGrounded = Mathf.Abs(transform.position.y - minY) < 0.2f;
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && jumpCoroutine == null)
        {
            isJumping = true;
            jumpCoroutine = StartCoroutine(JumpUp());
        }
    }

    private void OnFall(InputAction.CallbackContext context)
    {
        transform.position = new Vector3(-2f, minY, 0f);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("isAttack");
        double performedTime = context.time;

        for (int i = 0; i < judgeZones.Length; i++)
        {
            var note = judgeZones[i].GetFirstNote();
            if (note != null)
            {
                float offset = (float)(context.time - GameManager.Instance.RealtimeStartTime) - note.HitTime;
                GameManager.Instance.ShowJudgementOffset(offset);

                ScoreManager.Instance.RegisterJudgement((Judgement)i);
                note.OnHit();
                return;
            }
        }

        //ScoreManager.Instance.RegisterJudgement(Judgement.Miss); //넣을지 말지 고민중
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