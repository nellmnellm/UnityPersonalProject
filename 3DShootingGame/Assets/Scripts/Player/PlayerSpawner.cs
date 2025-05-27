using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform HeartContainer;
    public Transform BombContainer;
    public Vector3 spawnPosition = new Vector3(0f, -5f, 0f);

    public GameObject basePrefab; //������ ���� ������. ��������.
    private IEnumerator Start()
    {
        yield return null;
        GameObject mo;

        if (PlayerManager.Instance == null)
        {

            // 1��������: ���� �ν��Ͻ��� ������ ����
            if (PlayerSelectionManager.selectedPlayerPrefab != null && PlayerPrefs.GetInt("IS_PRACTICE", 0) == 0)
            {
                mo = Instantiate(PlayerSelectionManager.selectedPlayerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                mo = Instantiate(basePrefab, spawnPosition, Quaternion.identity);
            }
            PlayerManager.Instance.SetUIContainers(HeartContainer, BombContainer);
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

        // UI �����

        PlayerManager.Instance.UpdateHearts();
        PlayerManager.Instance.UpdateBombs();
    }
}