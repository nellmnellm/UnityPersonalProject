using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Item : MonoBehaviour
    {
        public ItemData data;

        //public 기능을 쓰면서 에디터에서는 보이지 않게 처리
        [HideInInspector] public Rigidbody2D rbody;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        
    }
}