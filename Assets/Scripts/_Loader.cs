using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Loader : MonoBehaviour
{
    public GameObject gameManager;

    private void Awake() {
        if(_GameManager.sharedInstance == null)
        {
            Instantiate(gameManager);
        }
    }
}
