using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy4 : Enemy
{
    public float rushSpeed = 30f;
    private void Start()
    {
        speed = 5f;
        dir = Vector3.down;
        HP = 100;
        enemyScore = 2000;
        StartCoroutine(RushAfterDelay(9f));

    }

    IEnumerator RushAfterDelay(float delaySeconds)
    {
        
        yield return new WaitForSeconds(1);
        speed = 0;

        yield return new WaitForSeconds(delaySeconds);

        var target = GameObject.FindWithTag("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        speed = rushSpeed;
    }

    
}