using System;
using UnityEngine;


[Serializable]
public class PlayerStat
{
    public float speed;
    public int count_of_harvest;
}
public class PlayerMovement : MonoBehaviour
{
    public PlayerStat stat;
    Animator animator;

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

            }
            else
            {
                animator.SetBool("IsMove", false);
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
