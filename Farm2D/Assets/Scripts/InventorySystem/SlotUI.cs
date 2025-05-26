using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InventorySystem
{
    public class SlotUI : MonoBehaviour
    {
        public Image slot_icon;
        public TMP_Text slot_count_text;

        //½½·Ô Àû¿ë
        public void SetSlot(Slot slot)
        {
            if (slot != null)
            {
                slot_icon.sprite = slot.icon;
                slot_count_text.text = slot.count.ToString();
            }
        }
        //ºó ½½·Ô ¼³Á¤
        public void SetEmpty()
        {
            slot_icon.sprite = null;
            slot_count_text.text = "";
        }

    }
}