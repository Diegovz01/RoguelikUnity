using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Player : _MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        food = _GameManager.sharedInstance.playerFoodPoints;
        base.Start();
    }

    private void OnDisable()
    {
        _GameManager.sharedInstance.playerFoodPoints = food;
    }

    // Intento movimiento
    protected override void AttemptMove(int xDir, int yDir)
    {
        food--;
        base.AttemptMove(xDir, yDir);
        CheckIfGameOver();
        _GameManager.sharedInstance.playersTurn = false;
    }

    // Revisar food
    void CheckIfGameOver()
    {
        if (food <= 0) _GameManager.sharedInstance.GameOver();
    }

    private void Update()
    {
        if (!_GameManager.sharedInstance.playersTurn) return; // Si no es el turno del Player

        int horizontal;
        int vertical;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0) vertical = 0; // Bloquear moverse en diagonal
        if (horizontal != 0 || vertical != 0) AttemptMove(horizontal, vertical);
    }

    protected override void OnCantMove(GameObject go)
    {
        _Wall hitWall = go.GetComponent<_Wall>();

        if(hitWall != null)
        {
            hitWall.DamageWall(wallDamage);
            animator.SetTrigger("playerChop");
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        animator.SetTrigger("playerHit");
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.CompareTag("Food"))
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
