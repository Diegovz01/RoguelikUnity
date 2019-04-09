using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class _GameManager : MonoBehaviour
{
    public static _GameManager sharedInstance;
    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;

    _BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private List<_Enemy> enemies = new List<_Enemy>();
    private bool enemiesMoving;

    private int level = 0;
    int recordLevel;
    private GameObject levelImage, rePlayButton;
    private Text levelText;
    Text recordLevelText;
    public bool doigSetup;

    private void Awake()
    {
        if (_GameManager.sharedInstance == null)
        {
            _GameManager.sharedInstance = this;
        }
        else if (_GameManager.sharedInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<_BoardManager>();

        recordLevelText = GameObject.Find("RecordLevelText").GetComponent<Text>();
    }

    public void InitGame()
    {
        recordLevel = PlayerPrefs.GetInt("RecordLevel",0);

        doigSetup = true;
        levelImage = GameObject.Find("LevelImage");
        rePlayButton = GameObject.Find("Replay_Button");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        recordLevelText = GameObject.Find("RecordLevelText").GetComponent<Text>();

        levelText.text = "Day " + level;
        recordLevelText.text = "Record Days: " + recordLevel;
        levelImage.SetActive(true);
        rePlayButton.SetActive(false);

        enemies.Clear(); // Limpiar lista de enemies
        boardScript.SetUpScene(level);

        Invoke("HideLevelImage", levelStartDelay); // Invoke => ejecutar metodo con delay
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doigSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After \"" + level + "\" days,\n you starved.";
        levelImage.SetActive(true);
        rePlayButton.SetActive(true);
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
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving || doigSetup) return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(_Enemy enem)
    {
        enemies.Add(enem);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        if(level > recordLevel)
        {
            PlayerPrefs.SetInt("RecordLevel", level);
            recordLevel = PlayerPrefs.GetInt("RecordLevel");
        }
        InitGame();
    }
    public void ResetGame()
    {
        level = 0;
        playerFoodPoints = 100;
    }
}
