using UnityEngine;

public class SmallBoatSpawner : MonoBehaviour
{
    public GameObject smallBoatPrefab;   // 프리팹 참조
    public Transform player;             // 따라갈 플레이어
    public Vector3 offset = new Vector3(0, -1.1f, -1f); // 플레이어와의 상대 위치
    private GameObject spawnedBoat;      // 생성된 보트 참조
    public PlayerController playerController;
    private bool isOnWater = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (spawnedBoat == null)
            {
                // 보트 생성
                spawnedBoat = Instantiate(smallBoatPrefab, player.position + offset, Quaternion.Euler(0,270f,0));
                playerController.animator.SetBool("IsBoat", true);
            }
            else
            {
                // 보트 제거
                Destroy(spawnedBoat);
                playerController.SetSpeed(5);
                spawnedBoat = null;
                playerController.animator.SetBool("IsBoat", false);
            }
        }

        // 보트가 살아있으면 플레이어 위치 따라감
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