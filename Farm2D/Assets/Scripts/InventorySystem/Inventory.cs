using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items;

namespace Assets.Scripts.InventorySystem
{
    [Serializable]
    public class Inventory
    {

        public List<Slot> slots = new List<Slot>();


        //�κ��丮 ���� �� count��ŭ�� ������ �߰��մϴ�.
        public Inventory(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Slot slot = new Slot();

                slots.Add(slot);
            }
        }

        //�߰� ���(�߰� ���� ���¿� ���� Ȯ�� �� ���)
        public void Add(Item item)
        {
            //foreach ? or  for
            //1. �Ϲ������δ� for�� ��õ
            // --> ������ ����.
            // --> �迭 ��� �ÿ� ���� ���������� ����Ǵ� ���

            //2. foreach
            //: ������ ������ �ִ� ������ ���� �۾�(���ڿ�, �迭, Ư�� �÷���)
            //  �۾��� ���������� ����Ǳ� ������ �ܼ� ���� �۾� ���� �� ȿ�����Դϴ�.
            //  �ε��� ������ �� �� ���, ���迡 �������� ������ �� �ֽ��ϴ�.
            //  �� �ݺ� �������� �� ���� ���� �������� ���� ����� ����� ����.

            //1) ������ �߰� ���� ���� üũ�� ���� ���
            foreach (var slot in slots)
            {
                if (slot.item_name == item.data.itemName && slot.Addable())
                {
                    slot.Add(item);
                    return;
                }
            }
            //Ÿ�� None�� ���� ó��
            foreach (var slot in slots)
            {
                if (slot.item_name == "")
                {
                    slot.Add(item);
                    return;
                }
            }
        }
        public void Remove(int idx)
        {
            slots[idx].Remove();
        }

    }
}