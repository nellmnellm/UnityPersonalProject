using UnityEngine;

public class Enemy1 : Enemy
{
    private void Start()
    {
        speed = 5;
        dir = Vector3.down;
        HP = 5;
        enemyScore = 100;
    }
}