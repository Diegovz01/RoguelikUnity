using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    public static _GameManager sharedInstance;
    _BoardManager boardScript;

    private void Awake() {
        if(_GameManager.sharedInstance == null)
        {
            _GameManager.sharedInstance = this;
        }else if(_GameManager.sharedInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<_BoardManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        boardScript.SetUpScene(1);
    }
}
