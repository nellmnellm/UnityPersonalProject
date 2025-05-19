using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour
{
    public List<Collider> targetList;

    private void Awake()
    {
        targetList = new List<Collider>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Remove(other);   
    }

    private void LateUpdate()
    {
        targetList.RemoveAll(target => target == null);
        //리스트 문법 => Contains(value) 값이 포함되고 있는지
        //Remove(value)해당 값을 제거
        //Add(value)해당값을 추가(리스트 마지막에)
        //RemoveAll(Predicate<T>) => 제거 조건의 대리자를 넣고 리무버 실행.
    }
}
