using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    float movementSpeed;
    BoxCollider2D boxCollider2D;
    Rigidbody2D rb2D;

    protected virtual void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        movementSpeed = 1f / moveTime; // Toma la velocidad para moverse en el tiempo.
    }

    protected IEnumerator SmoothMovement(Vector2 end)
    {
        float remaingDistance = Vector2.Distance(rb2D.position, end); // cuanto es la distancia
        while (remaingDistance > float.Epsilon) // valor muy pequeño
        {
            Vector2 newPosition = Vector2.MoveTowards(rb2D.position, end, movementSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition); // Nos movemos
            remaingDistance = Vector2.Distance(rb2D.position, end); // comprobamos Distance
            yield return null; // una vez por fotograma
        }
    }

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
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }
    
    protected abstract void OnCantMove(GameObject go);

    protected virtual void AttemptMove(int xDir, int yDir) // uso de genericos
    { // virtual para sobrescribirlo
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if (canMove) return;

        OnCantMove(hit.transform.gameObject);
    }
}
