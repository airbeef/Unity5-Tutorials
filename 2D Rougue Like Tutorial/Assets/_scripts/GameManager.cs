using UnityEngine;
using System.Collections;
using System.Collections.Generic; //uses List<>
using UnityEngine.UI; //instantiate UI elements

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null; //makes sure game manager can be accessed but not duplicated on top of each other
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true; //variable will be public but it won't display in editor

    private Text levelText; //controls text
    private GameObject levelImage; // controls UI canvas
    private int level = 1;  //start of enemy spawning after tutorial play
    private List<Enemy> enemies; //keeps track of enemies
    private bool enemiesMoving;
    private bool doingSetup; //allow levels to load and prevents player from interacting with game

	// Get and store component reference to boardmanager & init game function
	void Awake ()
    {
        Debug.Log("GameManager.Awake");

        //starts game loop within Game Manager/ destroys duplicate instances
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); //keeps track of score between scenes
        enemies = new List<Enemy>(); //keeps track of enemies
        boardScript = GetComponent<BoardManager>(); //builds out gameboard
        InitGame(); //starts game
    }

    //called everytime a scene is loaded; adds level number and Init()
    private void OnLevelWasLoaded (int index)
    {
        level++; //increase level

        InitGame(); //start game
    }

    //starts game
    void InitGame()
    {

        Debug.Log("GameManager.InitGame");

        doingSetup = true; // player can't move while title card load is complete
        levelImage = GameObject.Find("LevelImage"); //connects to canvas in editor
        levelText = GameObject.Find("LevelText").GetComponent<Text>(); //connects to text in editor
        levelText.text = "Day " + level + " Mah Ninja."; //level text that increments per level
        levelImage.SetActive(true); //canvas is now active
        Invoke("HideLevelImage", levelStartDelay); //once title card is displayed, wait 2 seconds before turning image off

        enemies.Clear(); //clear out enemies on new game
        boardScript.SetupScene(level); //tells script what level so it can determine enemies

    }

    //Cleans up Level Image when game starts
    private void HideLevelImage()
    {
        levelImage.SetActive(false); //turns off canvas
        doingSetup = false; //gives player their functionality back after image is finished
    }



    //game's end
    public void GameOver()
    {
        Debug.Log("GameManager.GameOver");

        levelText.text = "After " + level + " days, you starved."; //lets player know how many levels they survived
        levelImage.SetActive(true); //actives canvas
        enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (playersTurn || enemiesMoving || doingSetup) //check to see if player is moving, enemy is moving or canvas is loading
        {
            return; //wait until movement stops
        }

        //if niether player or enemies are moving, start enemy coroutine calculations
        StartCoroutine (MoveEnemies());
	}

    //registers enemies to GameManager so it can assign movement orders
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }


    //moves enemies in sequence; coroutine
    IEnumerator MoveEnemies()
    {
        Debug.Log("GameManager.MoveEnemies");

        enemiesMoving = true; //enemies are moving
        yield return new WaitForSeconds(turnDelay); //wait to see who is moving

        //check to see if enemies spawned yet
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
       
        //loop through enemies and issue move command
        for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].MoveEnemy(); //mobe enemy
                yield return new WaitForSeconds(enemies[i].moveTime); //wait until the next enemy can be moved
            }

        playersTurn = true; //let player move 
        enemiesMoving = false; //don't move until after player's turn
    }


}
