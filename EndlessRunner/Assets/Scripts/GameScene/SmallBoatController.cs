using UnityEngine;

public class SmallBoatSpawner : MonoBehaviour
{
    public GameObject smallBoatPrefab;   // ������ ����
    public Transform player;             // ���� �÷��̾�
    public Vector3 offset = new Vector3(0, -1.1f, -1f); // �÷��̾���� ��� ��ġ
    private GameObject spawnedBoat;      // ������ ��Ʈ ����
    public PlayerController playerController;
    private bool isOnWater = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (spawnedBoat == null)
            {
                // ��Ʈ ����
                spawnedBoat = Instantiate(smallBoatPrefab, player.position + offset, Quaternion.Euler(0,270f,0));
                playerController.animator.SetBool("IsBoat", true);
            }
            else
            {
                // ��Ʈ ����
                Destroy(spawnedBoat);
                playerController.SetSpeed(5);
                spawnedBoat = null;
                playerController.animator.SetBool("IsBoat", false);
            }
        }

        // ��Ʈ�� ��������� �÷��̾� ��ġ ����
        if (spawnedBoat != null)
        {
            Rigidbody rb = spawnedBoat.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 targetPos = player.position; //+ offset;
                playerController.SetSpeed(3);
                rb.MovePosition(Vector3.Lerp(rb.position, targetPos, 4f * Time.deltaTime));
            }
        }

       
    }
}