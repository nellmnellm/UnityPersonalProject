using UnityEngine;

public class Enemy3 : Enemy
{
    private void Start()
    {
        speed = 5;
        HP = 3;
        var target = GameObject.Find("Player");
        dir = target.transform.position - transform.position;
        dir.Normalize();

       
    }
}