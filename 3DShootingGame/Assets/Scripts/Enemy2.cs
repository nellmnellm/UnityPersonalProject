using UnityEngine;

public class Enemy2 : Enemy
{
    private void Start()
    {
        speed = 5;
        dir = Vector3.down * 3;
        HP = 3;
    }
}