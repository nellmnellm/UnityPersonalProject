using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform HeartContainer;
    public Transform BombContainer;
    public Vector3 spawnPosition = new Vector3(0f, -5f, 0f);

    private IEnumerator Start()
    {
        yield return null;
        GameObject mo;

        if (PlayerManager.Instance == null)
        {
            // 1��������: ���� �ν��Ͻ��� ������ ����
            if (PlayerSelectionManager.selectedPlayerPrefab != null)
            {
                mo = Instantiate(PlayerSelectionManager.selectedPlayerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("���õ� �÷��̾ �����ϴ�!");
                yield break;
            }
        }
        else
        {
            // 2��������: ���� �ν��Ͻ� ����
            mo = PlayerManager.Instance.gameObject;
            mo.transform.position = spawnPosition;
            for (int i=0; i<100; i++)
            {
                PlayerManager.Instance.AddBulletToPool();
            }
           
        }


        // UI �����̳� ����
        PlayerManager.Instance.SetUIContainers(HeartContainer, BombContainer);
        // UI �����
        PlayerManager.Instance.UpdateHearts();
        PlayerManager.Instance.UpdateBombs();
        
    }
}