using System;
using UnityEngine;
using Assets.Scripts.Items;

namespace Assets.Scripts.InventorySystem
{
    [Serializable]
    public class Slot
    {


        public string item_name;
        public Sprite icon;
        public int count;
        public int max_count;

        public Slot()
        {
            item_name = "";
            count = 0;
            max_count = 10; //������ �⺻���� ������ ���ڷ� ���

            //���� �� �ý����̳� ������ ���� ����, max_count ���� �ø��ų�(����),
            //���� ���� ��ü�� �ø��� ���� ����غ� �� �ֽ��ϴ�.(�κ��丮)
        }


        //���� ������ ������ max_count���� ���� ��쿡 �߰��� ��û
        public bool Addable()
        {
            return count < max_count ? true : false;
        }

        //�߰� ���� ���ο� ���� �Լ�
        public void Add(Item item) //Ÿ�� + ������ ���
        {
            item_name = item.data.itemName;
            icon = item.data.icon;
            count++;
        }

        public void Remove()
        {
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    icon = null;
                    item_name = "";
                }
            }
        }
    }
}