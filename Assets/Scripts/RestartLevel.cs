using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void RestartGame()
    {
        //SceneManager.LoadScene(0);
        _GameManager.sharedInstance.ResetGame();
        Invoke("RScene", 0.3f);
    }
    void RScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.Find("GameManager(Clone)").GetComponent<_GameManager>().enabled = true;
        _SoundManager.instance.musicSource.Play();
    }
}
