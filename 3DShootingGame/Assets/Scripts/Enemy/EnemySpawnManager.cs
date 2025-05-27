using System.Collections;
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

    public IEnumerator ForceActivateSpawner(EnemySpawner targetSpawner)
    {
        foreach (var slot in spawnerSlots)
        {
            if (slot.spawner == targetSpawner)
            {
                slot.spawner.isActive = true;
                yield return new WaitForSeconds(0.7f);
            }
        }
    }

    
   public void ForcedSpawner(EnemySpawner spawner)
    {
        // 한 프레임 안에 스폰되도록 step 직전으로 설정
        spawner.currentTime = spawner.step - 5f;
    }
}