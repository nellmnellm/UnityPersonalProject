using JetBrains.Annotations;
using System.Collections.Generic;

namespace Asset.Scripts.InventorySystem
{
    public class Slot
    {
        public CollectType type;
        public int count;
        public int max_count;
    }
}