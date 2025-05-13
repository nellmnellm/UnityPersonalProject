using UnityEngine;

public class Enemy2 : Enemy
{
    private void Start()
    {
        speed = 15;
        dir = Vector3.down; 
        HP = 5;
        enemyScore = 200;
    }
}