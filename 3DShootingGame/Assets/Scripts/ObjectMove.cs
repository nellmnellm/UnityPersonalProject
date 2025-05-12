using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Vector3 moveDirection; // 이동 방향
    public float moveSpeed;             // 이동 속도

    [Header("Boundary Limits (World Coordinates)")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -15f;
    public float maxY = 15f;

    public bool isMoving = true;

    
    protected void Update()
    {
        if (!isMoving) return;

        Vector3 nextPosition = transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime;

        // 경계 체크 후 제한
        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);

        transform.position = nextPosition;
    }
}