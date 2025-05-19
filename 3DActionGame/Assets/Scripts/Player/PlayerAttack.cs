using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("������ ��ġ")]
    [Tooltip("�Ϲݰ��� ������")] public int NormalDamage = 100;
    [Tooltip("�Ϲݰ��� ������")] public int SkillDamage = 200;
    [Tooltip("�Ϲݰ��� ������")] public int DashDamage = 300;

    [Header("Ÿ��")]
    public NormalTarget normalTarget;
    public SkillTarget skillTarget;

    public void NormalAttack()
    {
        var targetList = new List<Collider>(normalTarget.targetList);
        foreach (var target in targetList)
        {
            var enemy = target.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                StartCoroutine(enemy.StartDamage(NormalDamage, transform.position, 0.5f, 0.5f));
            }
        }
    }

    public void SkillAttack() 
    {
        var targetList = new List<Collider>(skillTarget.targetList);
        foreach (var target in targetList)
        {
            var enemy = target.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                StartCoroutine(enemy.StartDamage(SkillDamage, transform.position, 1f, 2f));
            }
        }
    }

    public void DashAttack()
    {
        var targetList = new List<Collider>(normalTarget.targetList);
        foreach (var target in targetList)
        {
            var enemy = target.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                StartCoroutine(enemy.StartDamage(DashDamage, transform.position, 1f, 2f));
            }
        }
    }


}
