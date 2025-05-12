using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy4 : Enemy
{

    public float rushSpeed = 30f;
    private void Start()
    {
        speed = 0f;
        HP = 100;

        StartCoroutine(RushAfterDelay(10f));


       
    }

    IEnumerator RushAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        var target = GameObject.Find("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        speed = rushSpeed;
    }
}