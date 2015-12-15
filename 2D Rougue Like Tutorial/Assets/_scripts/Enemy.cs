using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObjects {


    public int playerDamage; //health points subtracted from player

    private Animator animator; //enemy animation
    private Transform target; //tells enemies where player is and move them to player
    private bool skipMove; //moves enemies every othter turn


	// Modify MovingObjects Class Start
	protected override void Start ()
    {
        Debug.Log("Enemy.Start");

        GameManager.instance.AddEnemyToList(this); //adds enemy list to the game maanger
        animator = GetComponent<Animator>(); //grabs enemy animation
        target = GameObject.FindGameObjectWithTag("Player").transform; //find PC game object
        base.Start(); //start within MovingObjects base class
	}
	
    //Checks to see if enemy can move
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        Debug.Log("Enemy.AttemptMove");

        //checks to see if enemy can move
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir); //verifies that there are places the enemy can move

        skipMove = true; //affirms that enemy moved
    }

    //moves enemy tiles
    public void MoveEnemy()
    {
        Debug.Log("Enemy.MoveEnemy");

        int xDir = 0; //initial movement start point from spawned enemy both x and y
        int yDir = 0;

        //moves enemy: if target - transform is less than 0; are enemy and player in the same column?
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            yDir = target.position.y > transform.position.y ? 2 : -2; //if enemy is in the same column, move enemy up | or down | towards player
        
        else
            xDir = target.position.x > transform.position.x ? 2 : -2; //if enemy is in the same row, move enemy right | or left | towards player

        //passes in player's movement to verify where enemies should move to
        AttemptMove<Player>(xDir, yDir);
    }

    //
    protected override void onCantMove<T>(T component)
    {
        Debug.Log("Enemy.OnCanTMove");

        Player hitplayer = component as Player; //generic component cast to Player;
        animator.SetTrigger("enemyAttack"); //set trigger so enemies attack
        hitplayer.LoseFood(playerDamage); //if enemy can't move and able to hit player, hit the player

        
    }
}
