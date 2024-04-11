using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _manager;

    public static GameManager Instance => _manager;

    [HideInInspector]
    public Camera mainCam;

    private void Awake()
    {
        if (!_manager)
        {
            _manager = this;
            InitManager();
            return;
        }
        
        Destroy(gameObject);
    }

    private void InitManager()
    {
        mainCam = Camera.main;
    }
}
