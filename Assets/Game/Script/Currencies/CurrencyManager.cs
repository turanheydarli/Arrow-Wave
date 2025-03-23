using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Script.Currencies
{
    /// <summary>
    /// Added this manager to have a central place for loading, saving, and modifying all in-game currencies.
    /// </summary>
    public class CurrencyManager : MonoBehaviour
    {
        // A list of currencies to set initial game data
        [SerializeField] private List<CurrencyModel> defaults;

        private static Dictionary<CurrencyType, CurrencyModel> _currencies = new();
        private static Dictionary<CurrencyType, CurrencyModel> _defaultCurrencies = new();

        public static  UnityEvent OnUpdateCurrency;
        public static  UnityEvent<CurrencyType, float> OnAddToCurrency;

        private const string SaveFile = "Currency.es3";
        private const string SaveKey = "Currency";

        private void Awake()
        {
            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            defaults.ForEach(c => _defaultCurrencies.Add(c.CurrencyType, c));

            foreach (var currency in defaults)
            {
                _currencies[currency.CurrencyType] = LoadCurrency(currency.CurrencyType);
            }
        }

        private static CurrencyModel LoadCurrency(CurrencyType type)
        {
            var loadedCurrency = ES3.Load<CurrencyModel>($"{SaveKey}_{type}", SaveFile, _defaultCurrencies[type]);
            return loadedCurrency;
        }

        public static CurrencyModel GetCurrency(CurrencyType type)
        {
            if (_currencies.TryGetValue(type, out var currency))
            {
                return currency;
            }

            Debug.LogWarning($"Currency of type {type} not found.");
            return _defaultCurrencies[type];
        }

        public static void AddToCurrency(CurrencyType type, float amountToAdd)
        {
            var currency = GetCurrency(type);
            currency.Add(amountToAdd);

            OnAddToCurrency?.Invoke(type, amountToAdd);

            SaveCurrency(currency);
        }

        public static bool SubtractFromCurrency(CurrencyType type, float amountToSpend)
        {
            var currency = GetCurrency(type);
            if (currency.Amount >= amountToSpend)
            {
                currency.Subtract(amountToSpend);
                SaveCurrency(currency);
                return true;
            }

            return false;
        }

        public static bool CheckCurrencyAvailability(CurrencyType type, float requiredAmount)
        {
            var currency = GetCurrency(type);
            return currency.Amount >= requiredAmount;
        }

        public static void SaveAllCurrencies()
        {
            foreach (var currency in _currencies.Values)
            {
                SaveCurrency(currency);
            }
        }

        private static void SaveCurrency(CurrencyModel currency)
        {
            ES3.Save($"{SaveKey}_{currency.CurrencyType}", currency, SaveFile);
            OnUpdateCurrency?.Invoke();
        }
    }
}
