using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUPUIUpdater : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameManager.Instance.onLevelUp.AddListener(OnLevelUp);
        _text.gameObject.SetActive(false);
    }

    private void OnLevelUp()
    {
        _text.gameObject.SetActive(true);
        StartCoroutine(OnLevelUpCoroutine());
    }

    private IEnumerator OnLevelUpCoroutine()
    {
        yield return new WaitForSeconds(1f);
        _text.gameObject.SetActive(false);
        
    }
}
