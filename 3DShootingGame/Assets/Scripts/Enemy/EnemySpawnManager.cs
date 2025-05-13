using UnityEngine;

[System.Serializable]
public class SpawnerSlot
{
    public EnemySpawner spawner;
    public float startTime;
    public float endTime;
}
public class EnemySpawnManager : MonoBehaviour
{
    public SpawnerSlot[] spawnerSlots;

    private float gameTime = 0f;

    private void Start()
    {
        foreach (var slot in spawnerSlots)
        {
            if (slot.spawner != null)
                slot.spawner.isActive = false;
        }
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        foreach (var slot in spawnerSlots)
        {
            if (slot.spawner == null) continue;

            bool shouldBeActive = gameTime >= slot.startTime && gameTime < slot.endTime;
            slot.spawner.isActive = shouldBeActive;
        }
    }
}