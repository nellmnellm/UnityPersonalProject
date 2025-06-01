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
        inputActions.GamePlay.Enable(); // Action Map �̸��� Gameplay�� ���
        inputActions.GamePlay.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.GamePlay.Attack.performed -= OnAttack;
        inputActions.GamePlay.Disable();
    }

    void Start()
    {
        // ó�� ���� ��ġ
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

        //context.time = �޴� ����
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
        // ���� ��Ҵ��� üũ
        isGrounded = Mathf.Abs(transform.position.y - minY) < 0.2f;
        // ���� ����: F Ű, �Ʒ��ʿ� ���� ����
        if (Input.GetKeyDown(KeyCode.F) && isGrounded && jumpCoroutine == null)
        {
            isJumping = true;
            jumpCoroutine = StartCoroutine(JumpUp());
        }

        // Space�� ������ �ٷ� �ٴ�����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(-2f, minY, 0f);
        }

        
        

        //����Ű (<, >) . �����Ҷ��� �ִϸ����� ����. 
        /*if (Input.GetKeyDown(KeyCode.D)) //���� ���濹�� // �����Ϳ����� S �Ұ�!
        {
            animator.SetTrigger("isAttack");
            for (int i = 0; i < judgeZones.Length; i++)
            {
                var note = judgeZones[i].GetFirstNote();
                if (note != null)
                {
                    // ����!
                    Judgement result = (Judgement)i; // 0=Perfect, 1=Great, 2=Good
                    ScoreManager.Instance.RegisterJudgement(result);

                    note.OnHit(); // ���� �Ǵ� ����Ʈ
                    return; // ���⼭ �ٷ� return: �� ���� ó���ϰ� ����
                }
            }

            // �ƹ� ��Ʈ�� ������ Miss
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