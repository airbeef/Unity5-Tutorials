using UnityEngine;
using System.Collections;

public class flappyPlayer_ver2 : MonoBehaviour
{

    //How Does Our Bird Work? 
    public Vector3 velocity = Vector3.zero; //Movement
    public Vector3 gravity;  //gravity; can be modified in the editor; can be removed if using Rigidbody 2D
    public float flapVelocity; //bird flapping velocity after player input
    public float maxSpeed = 5f; //can be removed 
    public float angle = 0;
    public float forwardspeed = 1f;
    
    bool didFlap = false; //used for key input
    bool dead = false; //used when bird dies

    Animator birdAnimator; //flap & idle trigger

    // Use this for initialization
    void Start()
    {
        //animates trigger between idle animation and flapping animation
        birdAnimator = GetComponentInChildren<Animator>(); 
    }

    // Update
    void Update()
    {
        PlayerUpdate();
    }

    //Player Update and graphic update at beginning of Frame
    void PlayerUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            didFlap = true;
        }
    }

    // FixedUpdate updates physics at the end of the frame
    void FixedUpdate()
    {
        if(dead)
        {
            return;
        }

        GetComponent<Rigidbody2D>().AddForce(Vector2.right * forwardspeed); //replaces rigidbody2D

        if(didFlap)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * flapVelocity); //change Vector 3 to float for Flap velocity
            birdAnimator.SetTrigger("doFlap"); //animates trigger

            didFlap = false;
        }

        //This is bird rotation that is too wide
        if((GetComponent<Rigidbody2D>().velocity.y > 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
                              
            angle = Mathf.Lerp(0, -90, (GetComponent<Rigidbody2D>().velocity.y / 2f));
            //transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    //Death on collision
    void onCollisionEnter2D(Collision2D collision)
    {
        dead = true;
        birdAnimator.SetTrigger("collideDie");
        return;
    }

}
