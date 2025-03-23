using System;
using Game.Script.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private Image iconImage;
    [SerializeField] private Image bgImage;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Sprite defaultBgSprite;
    [SerializeField] private Sprite equippedBgSprite;
    [SerializeField] private Button itemButton;

    public event Action OnItemSelected;

    private InventoryItem _inventoryItem;
    private bool _isEquipped;

    public void SetupItem<T>(T item) where T : InventoryItem
    {
        Item = item;
        _inventoryItem = item;
        _isEquipped = InventoryManager.Instance.IsEquipped(item);

        itemName.text = item.itemName;
        bgImage.sprite = _isEquipped ? equippedBgSprite : defaultBgSprite;

        iconImage.sprite = item.icon;
        iconImage.color = item.isAvailable ? Color.white : Color.black;

        progressText.text = !item.isAvailable
            ? $"{item.opensAt - (Game_Control.CurrentLevel + 1)} levels."
            : $"";
        
        if (itemButton != null)
        {
            itemButton.interactable = item.isAvailable;
        }
    }

    public void OnItemClicked()
    {
        OnItemSelected?.Invoke();
    }

    public InventoryItem Item { get; private set; }

    public void UpdateUI()
    {
        SetupItem(Item);
    }
}