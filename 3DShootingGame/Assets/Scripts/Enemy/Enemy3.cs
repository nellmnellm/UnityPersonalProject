using UnityEngine;

public class Enemy3 : Enemy
{
    private void Start()
    {
        speed = 15;
        HP = 5;
        var target = GameObject.Find("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();
        enemyScore = 400;

    }
}