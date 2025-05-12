using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Vector3 moveDirection; // �̵� ����
    public float moveSpeed;             // �̵� �ӵ�

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

        // ��� üũ �� ����
        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);

        transform.position = nextPosition;
    }
}