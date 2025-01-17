using UnityEngine;

namespace GameJam.Modules.Core
{
    public interface IInventory
    {
        void AddItem(Item item);
        void RemoveItem(Item item);
    }
}