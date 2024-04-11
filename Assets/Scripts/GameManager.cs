using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _manager;

    public static GameManager Instance => _manager;

    private GameObject _player;

    public GameObject Player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }

            return _player;
        }
    }

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
