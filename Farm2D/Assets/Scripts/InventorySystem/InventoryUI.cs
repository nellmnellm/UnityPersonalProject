using System;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject inventory; //�κ��丮 â

        public Player player; //�÷��̾� ���

        public List<SlotUI> slots = new List<SlotUI>(); //���� UI ����

        private void Update()
        {
            //��ư ������ �κ��丮 Ű�� ���
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnOFF();
            }
        }

        public void OnOFF()
        {
            SlotRenewal();
            //���� ���ο� ���� true�� false�� �κ��丮�� Ű�ų� ���ϴ�.
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
                //���� ��� UI�� ���� ���� ����
            }
            else
            {
                inventory.SetActive(true);
            }
        }

        //���Կ� ���� ����
        private void SlotRenewal()
        {
            if (slots.Count == player.Inventory.slots.Count)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (player.Inventory.slots[i].item_name != "")
                    {
                        //���Կ� �̹����� ���� ���� �����Ѵ�.
                        slots[i].SetSlot(player.Inventory.slots[i]);
                    }
                    else
                    {

                        slots[i].SetEmpty();
                    }
                }
            }
        }

        public void Remove(int slot_idx)
        {
            //��Ȯ��
            Item drop = GameManager.instance.ItemManager.
                GetItem(player.Inventory.slots[slot_idx].item_name);

            if (drop != null)
            {
                player.Drop(drop);
                player.Inventory.Remove(slot_idx);
                SlotRenewal();
            }
        }
    }
}