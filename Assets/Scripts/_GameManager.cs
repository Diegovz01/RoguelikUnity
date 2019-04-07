using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;

public class _GameManager : MonoBehaviour
{
    public static _GameManager sharedInstance;
    public float turnDelay = 0.1f;

    _BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]public bool playersTurn = true;

    //private List<_Enemy> enemies = new List<_Enemy>();
    private List<_Enemy> enemies = new List<_Enemy>();
    private bool enemiesMoving;

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
        //enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }

    private void InitGame()
    {
        enemies.Clear(); // Limpiar lista de enemies
        boardScript.SetUpScene(1);
    }

    public void GameOver()
    {
        enabled = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }

    private void Update() 
    {
        if(playersTurn || enemiesMoving) return;

        StartCoroutine(MoveEnemies());   
    }

    public void AddEnemyToList(_Enemy enem)
    {
        enemies.Add(enem);
    }
}
