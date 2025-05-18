using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    GameObject mo;
    public Transform HeartContainer;
    public Transform BombContainer;
    private void Start()
    {
        if (PlayerSelectionManager.selectedPlayerPrefab != null)
        {
            mo = Instantiate(PlayerSelectionManager.selectedPlayerPrefab, new Vector3(-5f, 0f, 0f), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("���õ� �÷��̾ �����ϴ�!");
            return;
        }

        PlayerManager playerManager = mo.GetComponent<PlayerManager>();
        playerManager.heartContainer = HeartContainer;
        playerManager.bombContainer = BombContainer;
        
    }
}