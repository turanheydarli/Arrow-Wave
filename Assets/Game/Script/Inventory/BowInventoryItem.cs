using UnityEngine;

namespace Game.Script.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Bows")]
    public class BowInventoryItem : InventoryItem
    {
        public GameObject bowPrefab;
        public int maxActiveArrows;

        public override void Buy()
        {
            data.isOwned = true;
        }

        public override void Equip()
        {
            InventoryManager.Instance.EquipBow(this);
        }
    }
}