using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        if (PlayerSelectionManager.selectedPlayerPrefab != null)
        {
            Instantiate(PlayerSelectionManager.selectedPlayerPrefab, new Vector3(-5f, 0f, 0f), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("���õ� �÷��̾ �����ϴ�!");
        }
    }
}