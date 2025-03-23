using UnityEngine;

namespace Game.Script.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Arrows")]
    public class ArrowInventoryItem : InventoryItem
    {
        public GameObject arrowPrefab;
        public GameObject playerArrowPrefab;

        public override void Buy()
        {
            data.isOwned = true; 
        }

        public override void Equip()
        {
            InventoryManager.Instance.EquipArrow(this);
        }
    }
}