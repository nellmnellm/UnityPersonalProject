using UnityEngine;

public class RidingController : MonoBehaviour
{

    public Transform player; // �÷��̾� transform
    public Transform rideAttachPoint; // ���̵��� ���� ��ġ (ex: ĳ���� �߹�)
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
                    spawnedRide.transform.SetParent(rideAttachPoint); // ������ ����
                    playerController.animator.SetBool("IsRide", true);
                    Animator rideAnimator = spawnedRide.GetComponent<Animator>();
                    if (rideAnimator != null)
                    {
                        rideAnimator.SetBool("IsWalk", true);  // �ȴ� �ִϸ��̼�
                    }
                }
                else
                {
                    Debug.LogWarning("�ش� ���̵� �������� �����ϴ�: " + selectedRideName);
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