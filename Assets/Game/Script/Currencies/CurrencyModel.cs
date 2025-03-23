using System;
using UnityEngine;

namespace Game.Script.Currencies
{
    [Serializable]
    public class CurrencyModel
    {
        public float Amount => amount;
        public CurrencyType CurrencyType => currencyType;

        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private float amount;

        public void Subtract(float spendAmount)
        {
            amount -= spendAmount;
        }

        public void Add(float addAmount)
        {
            amount += addAmount;
        }

        public bool CheckAvailability(float requiredAmount)
        {
            return amount >= requiredAmount;
        }
    }
}