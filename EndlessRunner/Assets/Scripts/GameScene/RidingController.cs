using UnityEngine;

public class RidingController : MonoBehaviour
{

    public Transform player; // 플레이어 transform
    public Transform rideAttachPoint; // 라이딩을 붙일 위치 (ex: 캐릭터 발밑)
    private GameObject spawnedRide;
    public PlayerController playerController;
    
    private string selectedRideName;

    void Start()
    {
        selectedRideName = PlayerPrefs.GetString("SelectedRide", "Horse");
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (spawnedRide == null)
            {
                GameObject ridePrefab = Resources.Load<GameObject>("Rides/" + selectedRideName);
                if (ridePrefab != null)
                {
                    spawnedRide = Instantiate(ridePrefab, rideAttachPoint.position, rideAttachPoint.rotation);
                    spawnedRide.transform.SetParent(rideAttachPoint); // 하위로 부착
                    playerController.animator.SetBool("IsRide", true);
                    Animator rideAnimator = spawnedRide.GetComponent<Animator>();
                    if (rideAnimator != null)
                    {
                        rideAnimator.SetBool("IsWalk", true);  // 걷는 애니메이션
                    }
                }
                else
                {
                    Debug.LogWarning("해당 라이딩 프리팹이 없습니다: " + selectedRideName);
                }
            }
            else
            {
                Destroy(spawnedRide);
                spawnedRide = null;
                playerController.animator.SetBool("IsRide", false);
            }
        }
    }
}