using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform HeartContainer;
    public Transform BombContainer;
    public Vector3 spawnPosition = new Vector3(0f, -5f, 0f);

    public GameObject basePrefab; //라이프 많은 프리펩. 연습모드용.
    private IEnumerator Start()
    {
        yield return null;
        GameObject mo;

        if (PlayerManager.Instance == null)
        {

            // 1스테이지: 아직 인스턴스가 없으니 생성
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
            // 2스테이지: 기존 인스턴스 재사용
            mo = PlayerManager.Instance.gameObject;
            mo.transform.position = spawnPosition;
            for (int i=0; i<100; i++)
            {
                PlayerManager.Instance.AddBulletToPool();
            }
            
        }
        // UI 컨테이너 연결

        // UI 재생성

        PlayerManager.Instance.UpdateHearts();
        PlayerManager.Instance.UpdateBombs();
    }
}