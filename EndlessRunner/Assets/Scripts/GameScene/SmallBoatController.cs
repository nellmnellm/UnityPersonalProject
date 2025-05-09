using System.Collections;
using TMPro;
using UnityEngine;

public class SmallBoatSpawner : MonoBehaviour
{
    public GameObject smallBoatPrefab;   // ������ ����
    public Transform player;             // ���� �÷��̾�
    public Vector3 offset1 = new Vector3(0, -1.1f, -0.5f); // ���� ������
    public Vector3 offset2 = new Vector3(0f, -0.2f, -0.2f); // ��Ʈ�� ���� ������ ������
    private GameObject spawnedBoat;      // ������ ��Ʈ ����
    public PlayerController playerController;
    
    public TileManager tileManager;
    private Coroutine boatCoroutine;
    
    private void Update()
    {
        
        if (tileManager.BoatCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.X) && spawnedBoat == null)
            {
                // ��Ʈ ����
                spawnedBoat = Instantiate(smallBoatPrefab, player.position + offset1, Quaternion.Euler(0, 270f, 0));
                playerController.animator.SetBool("IsBoat", true);
                tileManager.BoatCount--;
                
                if (boatCoroutine != null)
                {
                    StopCoroutine(boatCoroutine);
                }
                boatCoroutine = StartCoroutine(BoatDurationRoutine(3f));

            }
        }
        
        /* else
         {
             // ��Ʈ ����
             Destroy(spawnedBoat);
             playerController.SetSpeed(5);
             spawnedBoat = null;
             playerController.animator.SetBool("IsBoat", false);
         }*/


        // ��Ʈ�� ��������� �÷��̾� ��ġ ����
        if (spawnedBoat != null)
        {
            Rigidbody rb = spawnedBoat.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 targetPos = player.position + new Vector3(0f, -0.2f, 0.5f);
                playerController.SetSpeed(3);
                rb.MovePosition(Vector3.Lerp(rb.position, targetPos, 15f * Time.deltaTime));
            }
        }

       
    }

    private IEnumerator BoatDurationRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (spawnedBoat != null)
        {
            Destroy(spawnedBoat);
            spawnedBoat = null;
            playerController.animator.SetBool("IsBoat", false);
            playerController.SetSpeed(5);
        }
    }

    
}