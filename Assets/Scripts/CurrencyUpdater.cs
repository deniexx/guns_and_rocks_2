using System;
using TMPro;
using UnityEngine;

public class CurrencyUpdater : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameManager.Instance.onCurrencyChanged.AddListener(UpdateText);
        _text.SetText(GameManager.Instance.Currency.ToString());
    }

    private void UpdateText(int currency)
    {
        _text.SetText(currency.ToString());
    }
}
