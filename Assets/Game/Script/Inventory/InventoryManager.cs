using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Script.Inventory
{
    /// <summary>
    /// Added this manager to have a central place for loading, saving, and managing all in-game bows and arrows. 
    /// </summary>
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField] private string defaultEquippedBow;
        [SerializeField] private string defaultEquippedArrow;

        public List<BowInventoryItem> Bows => allItems.OfType<BowInventoryItem>().ToList();
        public List<ArrowInventoryItem> Arrows => allItems.OfType<ArrowInventoryItem>().ToList();

        [SerializeField] public List<InventoryItem> allItems;

        private string equippedBowId;
        private string equippedArrowId;

        public BowInventoryItem EquippedBow { get; private set; }
        public ArrowInventoryItem EquippedArrow { get; private set; }


        private const string SaveFile = "Inventory.es3";
        private const string SaveKey = "Inventory";
        private const string EquippedBowKey = "EquippedBowId";
        private const string EquippedArrowKey = "EquippedArrowId";

        protected void Awake()
        {
            if (!ES3.KeyExists(SaveKey))
            {
                equippedBowId = defaultEquippedBow;
                equippedArrowId = defaultEquippedArrow;
            }


            LoadItems();
            LoadEquippedItems();
            RefreshEquippedReferences();
        }

        private void LoadItems()
        {
            foreach (var item in allItems)
            {
                item.data = LoadItem(item);
            }
        }

        private static InventoryItemData LoadItem(InventoryItem item)
        {
            return ES3.Load($"{SaveKey}_{item.id}", SaveFile, item.defaultData);
        }

        private void LoadEquippedItems()
        {
            equippedBowId = ES3.Load(EquippedBowKey, SaveFile, defaultEquippedBow);
            equippedArrowId = ES3.Load(EquippedArrowKey, SaveFile, defaultEquippedArrow);
        }

        private void RefreshEquippedReferences()
        {
            if (!string.IsNullOrEmpty(equippedBowId))
            {
                EquippedBow = Bows.FirstOrDefault(b => b.id == equippedBowId);
            }

            if (!string.IsNullOrEmpty(equippedArrowId))
            {
                EquippedArrow = Arrows.FirstOrDefault(a => a.id == equippedArrowId);
            }
        }

        public void EquipBow(BowInventoryItem bow)
        {
            if (bow == null) return;
            EquippedBow = bow;
            equippedBowId = bow.id;
        }

        public void EquipArrow(ArrowInventoryItem arrow)
        {
            if (arrow == null) return;
            EquippedArrow = arrow;
            equippedArrowId = arrow.id;
        }

        public bool IsEquipped(InventoryItem item)
        {
            switch (item)
            {
                case BowInventoryItem:
                    return item == EquippedBow;
                case ArrowInventoryItem:
                    return item == EquippedArrow;
            }

            return false;
        }

        private void OnApplicationQuit()
        {
            SaveAll();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveAll();
            }
        }

        public void SaveAll()
        {
            SaveAllItems();
            SaveEquippedItems();
        }

        public void SaveAllItems()
        {
            foreach (var item in allItems)
            {
                SaveItem(item);
            }
        }

        private static void SaveItem(InventoryItem item)
        {
            ES3.Save($"{SaveKey}_{item.id}", item.data, SaveFile);
        }

        private void SaveEquippedItems()
        {
            ES3.Save(EquippedBowKey, equippedBowId, SaveFile);
            ES3.Save(EquippedArrowKey, equippedArrowId, SaveFile);
        }
    }
}