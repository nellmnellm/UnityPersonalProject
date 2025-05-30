using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    public JudgeZone[] judgeZones;
    private Vector2 targetPosition;

    private float minY = -4.2f;
    private float maxY = 4.2f;

    private bool isJumping = false;
    private bool isGrounded = false;

    private Coroutine jumpCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();

        // ó�� ���� ��ġ
        transform.position = new Vector2(-2f, transform.position.y);
    }
    private void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        // ��� �̵� (�¿� ����)
        float y = Input.GetAxisRaw("Vertical");
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
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
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
        }
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