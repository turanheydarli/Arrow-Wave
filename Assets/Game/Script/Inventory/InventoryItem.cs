using System;
using UnityEngine;

namespace Game.Script.Inventory
{
    [Serializable]
    public class InventoryItemData
    {
        public bool isOwned;
    }
    public abstract class InventoryItem : ScriptableObject
    {
        public string id;
        public Sprite icon;
        public string itemName;
        public int opensAt;
        public int price;

        public InventoryItemData data;
        public InventoryItemData defaultData;

        public bool isAvailable => opensAt <= (PlayerPrefs.GetInt("Current_Level_Index", 0) + 1);

        public abstract void Buy();

        public abstract void Equip();

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString().Split("-")[0];
            }
        }
    }
}