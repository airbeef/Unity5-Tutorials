using UnityEngine;
using System.Collections;

public class flappyPlayer : MonoBehaviour {

    //How Does Our Bird Work? 
    public Vector3 velocity = Vector3.zero; //Movement
    public Vector3 gravity;  //gravity; can be modified in the editor; can be removed if using Rigidbody 2D
    public Vector3 flapVelocity; //bird flapping velocity after player input
    public float maxSpeed = 5f; //can be removed 
    public float angle = 0;
    public float forwardspeed = 1f;

    //used for key input
    bool didFlap = false;


	// Use this for initialization
	void Start () {
	
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
	void FixedUpdate ()
    {
        velocity.x = forwardspeed; //birds forward momentum
        velocity += gravity * Time.deltaTime; //how much gravity will drop bird
        
        if(didFlap == true)
        {
            didFlap = false; //reset bird after input

            if(velocity.y < 0) //cancels movement so bird won't rotate straight down
            {
                velocity.y = 0; 
            }

          velocity += flapVelocity; //bird physics movement after input
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed); //confirms that player movement can't exceed max speed and velocity (clamps movement)

        transform.position += velocity * Time.deltaTime; //will move forward bird every frame

        //Player Rotation
        if (velocity.y < 0)
        {
            //Lerp rotates player angle as they move 
            //(0 = level, 90 = 90 degrees, v/m dictates the player rotation)
            angle = Mathf.Lerp(0, -90, -velocity.y / maxSpeed);
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);

        //Test Code - Different Rotation technique//
        //Quaternion birdrotate = Quaternion.Euler(0, 0, angle); //the bird only rotates on z axis
        //transform.rotation = Quaternion.Lerp(transform.rotation, birdrotate, Time.deltaTime * 6); //normalize rotation by time * 6
    }
}
