using UnityEngine;
using UnityEngine.SceneManagement; //needed to load scene
using System.Collections;
using System;
using UnityEngine.UI; //needed to update food



//Player class inherits movement from MovingObject.cs
public class Player : MovingObjects {

    public int wallDamage = 1; //applies to player inflicting damage to walls
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText; //food text variable

    private Animator animator; //stores reference to animator component
    private int food; //stores food and drink points

	// different implementation for start in Player.cs
	protected override void Start ()
    {
        Debug.Log("Player.Start");

        animator = GetComponent<Animator>(); //grabs reference from player animation in editor

        food = GameManager.instance.playerFoodPoints; //gets food values and stores as levels change

        foodText.text = "Food: " + food; //sets value for food score
        
        base.Start(); //calls start function of base class movinb object

    }

    //called when player object is disabled; Unity function. 
    private void OnDisable()
    {
        Debug.Log("Player.OnDisable");

        GameManager.instance.playerFoodPoints = food; //Stores values of food in game manager as levels change in GameManager.cs

    }

	// Update is called once per frame | Movement code
	void Update ()
    {
        Debug.Log("Player.Update");

        if (!GameManager.instance.playersTurn) return; //check GameManager boolean to see if it's the players turn

        //directional movement stored along vertical/horizontal axis
        int horizontal = 0;
        int vertical = 0;

        //get movement values, cast to float and store integer here
        horizontal = (int)Input.GetAxisRaw("Horizontal"); 
        vertical = (int)Input.GetAxisRaw("Vertical");

        //stops the player from moving diagonally
        if (horizontal != 0)
            vertical = 0;

        //check to see is there is a non 0 value | player may encounter a wall to interact with
        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
    }

    //checks to see what player is doing during movement
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        Debug.Log("Player.AttemptMove");

        food--; //player looses food points as they move
        foodText.text = "Food: " + food; //keeps track of food score even is player tries to move
        base.AttemptMove <T> (xDir, yDir); //calls AttemptMove function in MovingObjects.cs
        RaycastHit2D hit; //calls in Raycast 2D to see if player hit anything
        CheckIfGameOver(); //see if player is dead
        GameManager.instance.playersTurn = false; //player's turn has ended

        if (Move (xDir, yDir, out hit))
        {
            //pending
        }

    }

    //Allows player to interact with triggered tags of obstacles, food and exit
    private void OnTriggerEnter2D (Collider2D other)
    {
        Debug.Log("Player.OnTriggerEnter2D");

        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay); //one second pause
            enabled = false; //player won't be able to move just yet since level started over
        }

        else if (other.tag == "Food")
        {
            food += pointsPerFood; //adds food points
            foodText.text = "+" + pointsPerFood + "Food: " + food; //score when player picks up food
            other.gameObject.SetActive(false); //gets rid of the food from screen

        }


        else if (other.tag == "Soda")
        {
            food += pointsPerSoda; //adds food points
            foodText.text = "+" + pointsPerSoda + "Food: " + food; //when player picks up soda
            other.gameObject.SetActive(false); //gets rid of the food from screen

        }

    }


    //take action if there is a wall and blocked by wall
    protected override void onCantMove<T>(T component)
    {
        Debug.Log("Player.onCantMove");

        Wall hitwall = component as Wall; //Check to see if player is near a wall
        hitwall.DamageWall(wallDamage); //this logs how much damage player can do to wall
    
        animator.SetTrigger("playerChop"); //player will use chop animation when hitting wall
    }

    //Go to next level if player collides with exit tile
    private void Restart()
    {
        Debug.Log("Player.Restart");

        SceneManager.LoadScene("baseScene");

       //Application.LoadLevel(Application.loadedLevel); //load the last scene 'Main'| Note: to load another level, we would have to load another scene
    }

    //if enemy attacks player, they lose health points
    public void LoseFood (int loss)
    {
        Debug.Log("Player.LoseFood");

        animator.SetTrigger("playerHit"); //player gets hit
        food -= loss; //player loses 'health'
        foodText.text = "-" + loss + " Food:" + food; // keeps track of food lost via text when hit 
        CheckIfGameOver(); //check if player is dead
    }

    //check to see if game is over
    private void CheckIfGameOver()
    {
        Debug.Log("Player.CheckIfGameOver");

        if (food <= 0)
            GameManager.instance.GameOver(); //if the food is less than/equal to 0; game over.
    }
}
