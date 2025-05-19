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
            // 1스테이지: 아직 인스턴스가 없으니 생성
            if (PlayerSelectionManager.selectedPlayerPrefab != null)
            {
                mo = Instantiate(PlayerSelectionManager.selectedPlayerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("선택된 플레이어가 없습니다!");
                return;
            }
        }
        else
        {
            // 2스테이지: 기존 인스턴스 재사용
            mo = PlayerManager.Instance.gameObject;
            mo.transform.position = spawnPosition;
        }

        PlayerManager playerManager = mo.GetComponent<PlayerManager>();

        // UI 컨테이너 연결
        playerManager.heartContainer = HeartContainer;
        playerManager.bombContainer = BombContainer;

        // UI 재생성
        playerManager.UpdateHearts();
        playerManager.UpdateBombs();
    }
}