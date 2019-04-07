using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Enemy : _MovingObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target; // Objetivo del enemy
    private bool skipMove; // Saltar un turno

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }
    
    protected override void Start()
    {
        _GameManager.sharedInstance.AddEnemyToList(this); // instancia del objeto donde se esta usando

        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove(int xDir, int yDir)
    {
        if(skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0, yDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        { // Abs => valor a positivo
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove(xDir, yDir);
    }

    protected override void OnCantMove(GameObject go)
    {
        _Player hitPlayer = go.GetComponent<_Player>();
        if(hitPlayer != null)
        {
            hitPlayer.LoseFood(playerDamage);
            animator.SetTrigger("enemyAttack");
        }
    }
}
