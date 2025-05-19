using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform HeartContainer;
    public Transform BombContainer;
    public Vector3 spawnPosition = new Vector3(-5f, 0f, 0f);

    private void Start()
    {
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
                return;
            }
        }
        else
        {
            // 2��������: ���� �ν��Ͻ� ����
            mo = PlayerManager.Instance.gameObject;
            mo.transform.position = spawnPosition;
        }

        PlayerManager playerManager = mo.GetComponent<PlayerManager>();

        // UI �����̳� ����
        playerManager.heartContainer = HeartContainer;
        playerManager.bombContainer = BombContainer;

        // UI �����
        playerManager.UpdateHearts();
        playerManager.UpdateBombs();
    }
}