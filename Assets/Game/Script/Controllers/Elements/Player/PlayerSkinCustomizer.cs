using Game.Script.Inventory;
using UnityEngine;

namespace Game.Script.Controllers.Elements.Player
{
    public class PlayerSkinCustomizer : MonoBehaviour
    {
        [SerializeField] private Transform bowParent;
        [SerializeField] private GameObject currentBowInstance;

        private GameObject _currentArrowInstance;

        private void OnEnable()
        {
            var equippedBow = InventoryManager.Instance.EquippedBow;
            var equippedArrow = InventoryManager.Instance.EquippedArrow;

            RefreshEquipment(equippedBow, equippedArrow);
        }

        public void SetItemPreview(InventoryItem item)
        {
            switch (item)
            {
                case BowInventoryItem bow:
                    RefreshEquipment(bow, InventoryManager.Instance.EquippedArrow);
                    break;

                case ArrowInventoryItem arrow:
                    RefreshEquipment(InventoryManager.Instance.EquippedBow, arrow);
                    break;
            }
        }

        private void RefreshEquipment(BowInventoryItem equippedBow, ArrowInventoryItem equippedArrow)
        {
            if (currentBowInstance != null)
            {
                Destroy(currentBowInstance);
            }

            if (equippedBow != null && equippedBow.bowPrefab != null)
            {
                currentBowInstance = Instantiate(equippedBow.bowPrefab, bowParent);
            }

            if (_currentArrowInstance != null)
            {
                Destroy(_currentArrowInstance);
            }

            if (equippedArrow != null && equippedArrow.arrowPrefab != null)
            {
                _currentArrowInstance = Instantiate(equippedArrow.arrowPrefab, currentBowInstance.transform);
            }
        }
    }
}