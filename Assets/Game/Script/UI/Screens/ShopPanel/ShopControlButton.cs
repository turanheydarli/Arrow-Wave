using Game.Script.Currencies;
using Game.Script.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopControlButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button controlButton;

    [SerializeField] private TMP_Text buttonText;

    private InventoryItem _selectedItem;

    private void Awake()
    {
        gameObject.SetActive(false);
        controlButton.onClick.AddListener(OnControlButtonClicked);
    }

    public void SetSelectedItem(InventoryItem item)
    {
        _selectedItem = item;
        gameObject.SetActive(true);
        UpdateButtonLabel();
    }

    private void UpdateButtonLabel()
    {
        if (_selectedItem == null)
        {
            buttonText.text = "Select an item";
            return;
        }

        if (_selectedItem.data.isOwned)
        {
            if (InventoryManager.Instance.IsEquipped(_selectedItem))
            {
                buttonText.text = "Equipped";
            }
            else
            {
                buttonText.text = "Equip";
            }
        }
        else
        {
            buttonText.text = $"Buy ({_selectedItem.price})";
        }
    }

    private void OnControlButtonClicked()
    {
        if (_selectedItem == null) return;

        if (!_selectedItem.data.isOwned)
        {
            if (CurrencyManager.CheckCurrencyAvailability(CurrencyType.Coin, _selectedItem.price))
            {
                CurrencyManager.SubtractFromCurrency(CurrencyType.Coin, _selectedItem.price);
                _selectedItem.Buy();
                _selectedItem.Equip();
            }
            else
            {
                
                return;
            }
        }
        else if (!InventoryManager.Instance.IsEquipped(_selectedItem))
        {
            _selectedItem.Equip();
        }

        UpdateButtonLabel();
    }
}