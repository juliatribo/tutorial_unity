using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAtack1;
    public AudioClip enemyAtack2;

 
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
            //comprovar el bool per si m'he de saltar el torn o no 
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        float xdif = Mathf.Abs(target.position.x - transform.position.x);
        float ydif = Mathf.Abs(target.position.y - transform.position.y);

        if (ydif > float.Epsilon && ydif > xdif)
            //el transform indica la posició de l'objecte (es jeràrquic, te un parent, ell es child)
            yDir = target.position.y > transform.position.y ? 1 : -1;

        else if (xdif > float.Epsilon)
            xDir = target.position.x > transform.position.x ? 1 : -1;

        //comprova si el jugador i ell estan a la mateixa col, si ho estan mira si esta per sobre o per sota i es mou cap a ell. Si no estan a la mateixa col es mou cap a una col mes aprop d'ell.



        AttemptMove<Player>(xDir, yDir);

    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        hitPlayer.LoseFood(playerDamage);

        animator.SetTrigger("enemyAttack");

        SoundManager.instance.RandomizeSfx(enemyAtack1, enemyAtack2);
    }

}
