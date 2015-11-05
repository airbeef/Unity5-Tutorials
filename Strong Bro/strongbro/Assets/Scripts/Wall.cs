using UnityEngine;
using System.Collections;

// Wall script watches to be damaged, then applies the damage
// Check to see if the damage amount changes the wall state, if yes then apply change
// spawn new wall if damage state expects it
// When damage hits zero destroy wall


public class Wall : MonoBehaviour 
{
    // keep track of the next game wall so you can tell it when to begin rendering again.
    public GameObject nextWallObject;
    Wall nextWall;

    // keep track of your background and foreground
    public GameObject backgroundObject;
    public GameObject foregroundObject;

    public Sprite[] sprites;
    //private SpriteRenderer spriteRenderer;
    //private WallType wallType;
    public int startingHealth = 50;
	private int currHealth;

    private int currentLevel = 0;
	//private int iDebugDamage = 5;

    private int damageSpriteIndex = 0;
    public float veryLowThreshold = 0.25f;
    public float lowThreshold = 0.5f;
    public float moderateThreshold = 0.75f;

    Vector2 startingPosition;

    SpriteRenderer spriteRenderer;
    Animator anim;
    //Damage damage = Damage.Full;

    public bool startingWall = false;

	// Use this for initialization
	void Start () 
	{
        if (nextWallObject != null)
            nextWall = nextWallObject.GetComponent<Wall>();

        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = foregroundObject.GetComponent<SpriteRenderer>();

        startingPosition = transform.position;

        if (startingWall)
        {
            BecomeMainWall();
        }
        else
        {
            Hide();
        }
	}

    // call when it's time to reset. Levels up and resets health. 
	void TakeDamage(int iDamage)
	{
        //Debug.Log(string.Format("Took {0} damage.", iDamage));

		currHealth -= iDamage;
		TickerController.UpdateScore (iDamage);

        CheckForSpriteChange();

        if (currHealth <= 0)
        {
            BeginBreaking();
        }
	}

    // call when it's time for the wall to break.
    private void BeginBreaking()
    {
        collider2D.enabled = false;

        anim.SetTrigger("Break");
    }

    void ChangeWall()
    {
        if (nextWall != null)
        {
            SetLayer(3);
            nextWall.BecomeMainWall();
        }
    }

    public void BecomeMainWall()
    {
        Debug.Log(gameObject.name + " is becoming the next wall!");

        backgroundObject.renderer.enabled = true;
        foregroundObject.renderer.enabled = true;



        ResetStats();

        LevelUp();

        SetLayer(2);

        collider2D.enabled = true;

        if (nextWall != null)
            nextWall.SetLayer(3);
    }

    public void Hide()
    {
        backgroundObject.renderer.enabled = false;
        foregroundObject.renderer.enabled = false;

        transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);

        collider2D.enabled = false;

        SetLayer(4);
    }

    private void ResetStats()
    {
        currHealth = startingHealth;

        //damage = Damage.None;

        damageSpriteIndex = 0;
        ChangeSprite();
    }

    private void LevelUp()
    {
        currentLevel += 1;
        startingHealth = Mathf.RoundToInt(startingHealth * 1.1f);
    }

    void CheckForSpriteChange()
    {
        int currentDamageSpriteIndex = 0;


        if (currHealth < startingHealth * 0.25f)
            currentDamageSpriteIndex = 3;
        else if (currHealth < startingHealth * 0.5f)
            currentDamageSpriteIndex = 2;
        else if (currHealth < startingHealth * 0.75f)
            currentDamageSpriteIndex = 1;

        if (currentDamageSpriteIndex != this.damageSpriteIndex)
        {
            damageSpriteIndex = currentDamageSpriteIndex;
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        //Debug.Log("Changing sprite!");
        spriteRenderer.sprite = sprites[damageSpriteIndex];
    }

    private void SetLayer(float z)
    {
        //Debug.Log("Setting layer for object: " + gameObject.name);

        backgroundObject.transform.position = new Vector3(transform.position.x, transform.position.y, z + 0.1f);
        foregroundObject.transform.position = new Vector3(transform.position.x, transform.position.y, z);

        //transform.position = new Vector3(transform.position.x, transform.position.y, z);
        //backgroundObject.transform.position = new Vector3(transform.position.x, transform.position.y, z + 0.1f);
    }

    //void WallShowDamage()
    //{
        ////if (iCurrHealth < 15)
        //if (VeryLowHealth)
        //{
        //    // show last damage overlay
        //    // should we create the new wall here?
        //    spriteRenderer.sprite = sprites[3];
        //    Debug.Log("Very low health");

        //}
        ////else if (iCurrHealth > 15 && iCurrHealth < 30)
        //else if (LowHealth)
        //{
        //    // show damage overlay
        //    spriteRenderer.sprite = sprites[2];
        //    Debug.Log("Low health");
        //}
        //else if (ModerateHealth)
        //{
        //    // no need to change over lay
        //    spriteRenderer.sprite = sprites[1];
        //    Debug.Log("Moderate health");
        //    return;
        //}
        //switch (damageIndex)
        //{
        //    case Damage.None:
        //        break;
        //    case Damage.Some:
        //        break;
        //    case Damage.Lots:
        //        break;
        //    case Damage.Full:
        //        break;
        //    default:
        //        Debug.Log("No case " + damageIndex + " for switch Damage in WallsShowDamage");
        //        break;
        //}
    //}

	

    //void BreakWall()
    //{
        //// HeartStrong should say something here

        //GameObject NextWall = (GameObject) Instantiate(WallPrefab, transform.position, transform.rotation);

        ////int iTemp = (int) wallType;
        ////iTemp++;

        //if (wallType == WallType.Metal) 
        //{
        //    // this is the point where the difficulty scales
        //    // for now we should just leave this, maybe call temp success stuff
        //    Debug.Log ("You finished three walls!");
        //    return;
        //}
        //else
        //{
        //    NextWall.SendMessage ("YourWallIs", wallType++);
        //}
    //}

    // this sets the type of the wall and initializes its parameters.
    //void YourWallIs(WallType wt)
    //{
        //if (spriteRenderer == null)
        //    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //switch (wt)
        //{
        //    case WallType.Wood:
				
        //        // initialize array and load sprites
        //        sprites = new Sprite[4];

        //        sprites[0] = Resources.Load<Sprite>("Wood/broWood_100");
        //        sprites[1] = Resources.Load<Sprite>("Wood/broWood_75");
        //        sprites[2] = Resources.Load<Sprite>("Wood/broWood_50");
        //        sprites[3] = Resources.Load<Sprite>("Wood/broWood_25");
            
        //        // show wood material

        //        Debug.Log("Sprite renderer is active: " + (spriteRenderer == null));
        //        spriteRenderer.sprite = sprites[(int)damageIndex];

        //        //// create a background game object
        //        //GameObject child = new GameObject("background");
        //        //child.transform.parent = this.transform;
        //        //child.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1);
        //        //SpriteRenderer childRenderer = child.AddComponent<SpriteRenderer>();
        //        //childRenderer.sprite = Resources.Load<Sprite>("Wood/broWood_Vase");

        //        // change health to proper amount
        //        iStartingHealth = 50;
        //        iCurrHealth = iStartingHealth;
        //        Debug.Log("Wall is WOOD");
        //        break;

        //    case WallType.Concrete:
        //        // show concrete material
        //        // change health to proper amount
        //        iStartingHealth = 100;
        //        iCurrHealth = iStartingHealth;
        //        Debug.Log("Wall is CONCRETE");
        //        break;

        //    case WallType.Metal:
        //        // show metal material
        //        // change health to proper amount
        //        iStartingHealth = 150;
        //        iCurrHealth = iStartingHealth;
        //        Debug.Log("Wall is METAL");
        //        break;

        //    default:
        //        // how did you get here?
        //        Debug.LogError("YourWallIs is in it's default");
        //        break;
        //}
    //}

    //enum Damage
    //{
    //    None = 0,
    //    Some = 1,
    //    Lots = 2,
    //    Full = 3,
    //}

    //public bool VeryLowHealth
    //{
    //    get
    //    {
    //        return (iCurrHealth < veryLowThreshold * iStartingHealth);
    //    }
    //}

    //public bool LowHealth
    //{
    //    get
    //    {
    //        return (iCurrHealth < lowThreshold * iStartingHealth);
    //    }
    //}

    //public bool ModerateHealth
    //{
    //    get
    //    {
    //        return (iCurrHealth < moderateThreshold * iStartingHealth);
    //    }
    //}
}
