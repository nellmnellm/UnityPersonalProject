using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Item Data")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
    }
}