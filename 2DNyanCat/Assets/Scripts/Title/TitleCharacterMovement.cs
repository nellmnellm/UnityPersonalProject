using UnityEngine;

public class TitleCharacterMovement : MonoBehaviour
{
    bool moveRight = true;
    
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 newPos = transform.position + new Vector3(x, y, 0) * 0.2f;

        newPos.x = Mathf.Clamp(newPos.x, -7.8f, 7.8f);
        newPos.y = Mathf.Clamp(newPos.y, -4.2f, 4.2f);

        transform.position = newPos;

        //방향 움직이지 않아도 고정시키기
        if (x == -1)
            moveRight = false;
        else if (x == 1)
            moveRight = true;

        transform.localScale = moveRight ? Vector3.one : new Vector3(-1, 1, 1);

    }
}
