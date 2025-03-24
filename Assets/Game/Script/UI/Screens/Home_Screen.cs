using System;
using System.Collections;
using System.Collections.Generic;
using Game.Script.Currencies;
using Game.Script.Inventory;
using TMPro;
using UnityEngine;

public class Home_Screen : MonoBehaviour, Screen_1_Callback
{
    [SerializeField]
    private TMP_Text coinsText;


    public Action Play_Callback;

    protected  void OnEnable()
    {
        RenderCurrencies();
        CurrencyManager.OnUpdateCurrency.AddListener(RenderCurrencies);
    }

    private void RenderCurrencies()
    {
        coinsText.text = CurrencyManager.GetCurrency(CurrencyType.Coin).Amount.ToString("N0");
    }

    public void Show(Action play_callback){
        Play_Callback = play_callback;
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    // UI Actions ---------------------
    public void Play_Button_Clicked()
    {
        Play_Callback?.Invoke();
    }

    protected  void OnDisable()
    {
        CurrencyManager.OnUpdateCurrency.RemoveListener(RenderCurrencies);
    }
}
