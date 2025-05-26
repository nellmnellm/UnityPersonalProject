using System;
using UnityEngine;


[Serializable]
public class PlayerStat
{
    public float speed;
}
public class PlayerMovement : MonoBehaviour
{
    public PlayerStat stat;
    Animator animator;

    private Vector2 last = Vector2.down;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void SetAnimateMovenemt(Vector3 direction)
    {
        if (animator != null) {
            if (direction.magnitude > 0)
            {
                animator.SetBool("IsMove", true);

                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                last = direction.normalized;
            }
            else
            {
                animator.SetBool("IsMove", false);
                animator.SetFloat("Horizontal", last.x);
                animator.SetFloat("Vertical", last.y);
            }



        }

    }
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector2(h, v);
        SetAnimateMovenemt(dir);
        transform.position += dir * stat.speed * Time.deltaTime;
    }
}
