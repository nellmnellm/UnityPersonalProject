using UnityEngine;

public class CastleHurtAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnMissChanged += PlayHurtAnimation;
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnMissChanged -= PlayHurtAnimation;
    }

    private void PlayHurtAnimation()
    {
        animator.SetTrigger("hurt");
    }
}