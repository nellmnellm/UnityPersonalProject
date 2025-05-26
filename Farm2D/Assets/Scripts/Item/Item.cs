using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Item : MonoBehaviour
    {
        public ItemData data;

        //public ����� ���鼭 �����Ϳ����� ������ �ʰ� ó��
        [HideInInspector] public Rigidbody2D rbody;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        
    }
}