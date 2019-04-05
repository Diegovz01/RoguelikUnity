using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    float movementSpeed;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rb2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = 1f / moveTime; // Toma la velocidad para moverse en el tiempo.
    }

    // Update is called once per frame

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    // protected => Privada para quien no herede de MovingObject.
    { 
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider2D.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider2D.enabled = true;

        if(hit.transform == null)
        {
            // Hacer el movimiento
            // ... 16:45 youtube cap 6
            return true;
        }
        return false;
    }
}
