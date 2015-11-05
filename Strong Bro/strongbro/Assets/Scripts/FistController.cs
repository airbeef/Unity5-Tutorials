using UnityEngine;
using System.Collections;

public class FistController : MonoBehaviour 
{
    public int damage = 5;
    public bool canPunch = true;

    public GameObject powEffect;

    Animator anim;
    GameObject[] powEffects;
    public int numberOfPows = 3;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        powEffects = new GameObject[numberOfPows];

        for (int i = 0; i < numberOfPows; i++)
        {
            powEffects[i] = (GameObject)Instantiate(powEffect);
            powEffects[i].SetActive(false);
        }
	}

    void Update() 
    {
        // this is temporary - remove once the button is hooked up!
        //if (PressedPunch())
        //{
        //    Punch();
        //}
	}

    // debug to let us punch by clicking. remove when button is hooked up.
    bool PressedPunch()
    {
        return Input.GetButtonDown("Punch");
    }

    // this method is a public hook to start punching. We should make sure that canPunch is true before calling Punch.
    public void Punch()
    {
        // the animation controls canPunch.
        if (canPunch)
            StartPunch();
    }

    // this method executes the punch.
    void StartPunch()
    {
        //Debug.Log(Input.GetButtonDown("Punch"));

        anim.SetTrigger("Punch");
    }

    // damage the wall if you punch it.
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c &&
            c.tag == "Wall")
        {
            //Debug.Log("In OnTriggerEnter2D for fistController");
            c.SendMessage("TakeDamage", damage);
            CreatePowEffect();
        }
    }

    private void CreatePowEffect()
    {
        //Debug.Log("Trying to create pow effect.");
        for (int i = 0; i < powEffects.Length - 1; i++)
        {
            if (!powEffects[i].activeInHierarchy)
            {
                //Debug.Log("Playing Pow Effect.");
                powEffects[i].transform.position = new Vector3(transform.position.x, transform.position.y, -9);
                powEffects[i].SetActive(true);
                powEffects[i].SendMessage("DisplayEffect");
                return;
            }
        }
    }
}
