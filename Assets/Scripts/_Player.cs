using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _Player : _MovingObject
{
    public AudioClip moveSound1, moveSound2, eatSound1, eatSound2, drinkSound1, drinkSound2, gameOverSound;

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private Animator animator;
    private int food;

    Vector2 touchOrigin = -Vector2.one;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        food = _GameManager.sharedInstance.playerFoodPoints;
        foodText.text = "Food: " + food;
        base.Start();
    }

    private void OnDisable()
    {
        _GameManager.sharedInstance.playerFoodPoints = food;
    }

    // Revisar food
    void CheckIfGameOver()
    {
        if (food <= 0)
        {
            _SoundManager.instance.PlaySingle(gameOverSound);
            _SoundManager.instance.musicSource.Stop();
            _GameManager.sharedInstance.GameOver();
        }
    }

    // Intento movimiento
    protected override void AttemptMove(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;
        base.AttemptMove(xDir, yDir);
        CheckIfGameOver();
        _GameManager.sharedInstance.playersTurn = false;
    }

    protected override void OnCanMove()
    {
        base.OnCanMove();
        _SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
    }

    private void Update()
    {
        if (!_GameManager.sharedInstance.playersTurn || _GameManager.sharedInstance.doigSetup) return; // Si no es el turno del Player

        int horizontal = 0;
        int vertical = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0) vertical = 0; // Bloquear moverse en diagonal
#else
        if (Input.touchCount > 0) // .touchCount => cantidad de dedos
        {
            Touch myTouch = Input.touches[0]; // Touch => guardar dedo/ tocar pantallas

            if (myTouch.phase == TouchPhase.Began) // .phase => comprobar fase 
            {   // Began => a empezado a tocar
                touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin != Vector2.one)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;

                if (x != 0 || y != 0)
                {
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        horizontal = x > 0 ? 1 : -1;
                    }
                    else
                    {
                        vertical = y > 0 ? 1 : -1;
                    }
                }
            }
        }
#endif
        if (horizontal != 0 || vertical != 0) AttemptMove(horizontal, vertical);
    }
    // Si no se puede mover 
    protected override void OnCantMove(GameObject go)
    {
        _Wall hitWall = go.GetComponent<_Wall>();

        if (hitWall != null)
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
        foodText.text = "-" + loss + "Food: " + food;
        animator.SetTrigger("playerHit");
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.CompareTag("Food"))
        {
            food += pointsPerFood;
            _SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            foodText.text = "+" + pointsPerFood + "Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            _SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            foodText.text = "+" + pointsPerSoda + "Food: " + food;
            other.gameObject.SetActive(false);
        }
    }
}
