using UnityEngine;
using System; //allows serialization
using System.Collections.Generic; //brings up lists
using Random = UnityEngine.Random; //allows level to be randomly generated; must be specificed since System also uses Random

public class BoardManager : MonoBehaviour {

    [Serializable] // this is it's own class
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //variables
    public int columns = 8; //initializez size of gameboard
    public int rows = 8;
    public Count wallCount = new Count(5, 9); //minimum of 5 to 9 walls per level
    public Count foodCount = new Count(1, 5); //minimum of 1 to 5 food items

    //array of game items
    public GameObject exit;
    public GameObject[] floorTiles; 
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    //private variables
    private Transform boardHolder; //keeps gameboject hierachy clean and collapsable
    private List<Vector3> gridPositions = new List <Vector3> (); //track items on gameboard and their positions

    //starts list of positions for grid
    void InitialiseList()
    {
        Debug.Log("BoardManager.InitialiseList");

        gridPositions.Clear(); //clears board before game spawn

        //sets object positions on the grid
        for (int x = 1; x < columns - 1; x++) // x spans from 1,1 - 6,1;
        {
            for (int y = 1; y < rows - 1; y++) //y  spans from 1,1 to 6,6
            {
                gridPositions.Add(new Vector3(x, y, 0f)); //creating list of possible positions
            }
        }

    }


    //Sets up board with outer walls and floor tiles
    void BoardSetup()
    {
        Debug.Log("BoardManager.BoardSetup");

        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++) // x spans from -1, -1; creates border
        {
            for (int y = -1; y < rows + 1; y++) //y spans from -1, -6; creates border
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; //creates random index floor tiles from array and creates border
  
                //checks if array is in outer walls between -1,-1 to 8,8; chooses random tile
                if(x == -1 || x == columns || y == -1 || y == rows)
                   toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //instantiate selected tile                     //place it along x & y axis | no rotation  |assigned to game object
                GameObject instance = 
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //set parent to collapsable hierachy for clean organization
                instance.transform.SetParent(boardHolder);
               
            }

        }

    }


    //Sets up board with inner obstacles, enemies, and power ups
    Vector3 RandomPosition() //Vector3
    {
        Debug.Log("BoardManager.RandomPosition");

        int randomIndex = Random.Range(0, gridPositions.Count); //declate random range in positions list
        Vector3 randomPosition = gridPositions[randomIndex]; //set position in radom position of list
        gridPositions.RemoveAt(randomIndex); //removes position so items won't be spawned on top of each other

        return randomPosition; //return function 

        
    }

    //Spawns actual tile and places it on the board
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) //array, min and max integers
    {
        Debug.Log("BoardManager.LayoutObjectRandom");

        int objectCount = Random.Range(minimum, maximum + 1); //controls how man objects will spawn

        for (int i = 0; i < objectCount; i++) //spawn numbers equal to object count
        {
            Vector3 randomPosition = RandomPosition(); //instantiate random position
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)]; //picks a random tile and spawns item/enemy/obstacle
            Instantiate(tileChoice, randomPosition, Quaternion.identity); //spawn tile at random position & do not rotate

        }

    }

    //Level Set up ; called by Game Manager
    public void SetupScene(int level)
    {
        Debug.Log("BoardManager.SetupScene");

        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //enemies and difficulty
        int enemyCount = (int)Mathf.Log(level, 2f); //creates difficulty to scale up with progression; turns float(Math.log) into integer
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount); //spawn enemies
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity); //spawn exit; will always be in same place, no rotation

    }

}
