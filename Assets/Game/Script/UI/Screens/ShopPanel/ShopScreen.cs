using System.Globalization;
using Game.Script.Controllers.Elements.Player;
using Game.Script.Currencies;
using Game.Script.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopScreen : UIBehaviour, Screen_0_Callback
{
    [Header("Screen & Panels")] [SerializeField]
    private GameObject shopScreen;

    [Header("Catalog References")] [SerializeField]
    private TMP_Text coinsText;

    [SerializeField]  private Transform contentParent;

    [SerializeField] private GameObject shopItemPrefab;

    [Header("Menu Buttons")] [SerializeField]
    private Button bowsButton;

    [SerializeField] private Button arrowsButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private ShopControlButton controlButton;

    protected override void Awake()
    {
        bowsButton.onClick.AddListener(ShowBows);
        arrowsButton.onClick.AddListener(ShowArrows);
        closeButton.onClick.AddListener(Hide);
    }

    protected override void OnEnable()
    {
        ShowBows();
        RenderCurrencies();
        CurrencyManager.OnUpdateCurrency.AddListener(RenderCurrencies);
        controlButton.SetSelectedItem(InventoryManager.Instance.EquippedBow);
    }

    private void RenderCurrencies()
    {
        coinsText.text = CurrencyManager.GetCurrency(CurrencyType.Coin).Amount.ToString("N0");
    }

    private void ShowBows()
    {
        ClearCatalog();
        var bowItems = InventoryManager.Instance.Bows;
        foreach (var bow in bowItems)
        {
            CreateShopItem(bow);
        }
        
    }

    private void ShowArrows()
    {
        ClearCatalog();
        var arrowItems = InventoryManager.Instance.Arrows;
        foreach (var arrow in arrowItems)
        {
            CreateShopItem(arrow);
        }
    }

    private void CreateShopItem(InventoryItem item)
    {
        GameObject newShopItem = Instantiate(shopItemPrefab, contentParent);

        var shopItemUI = newShopItem.GetComponent<ShopItemUI>();
        if (shopItemUI != null)
        {
            shopItemUI.SetupItem(item);

            shopItemUI.OnItemSelected += () =>
            {
                controlButton.SetSelectedItem(shopItemUI.Item);
                FindFirstObjectByType<PlayerSkinCustomizer>().SetItemPreview(shopItemUI.Item);
            };
        }
    }

    private void ClearCatalog()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    public void Show()
    {
        shopScreen.SetActive(true);
    }

    public void Hide()
    {
        shopScreen.SetActive(false);
    }

    protected override void OnDisable()
    {
        CurrencyManager.OnUpdateCurrency.RemoveListener(RenderCurrencies);
    }
}