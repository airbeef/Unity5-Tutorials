using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour {

    //variables
    public float speed; //general speed setting of movement
    private Rigidbody rb; //holds rigidbody reference

    public Text countText; //container for score text
    private int count; //holds score after collecting cubes

    public Text winText; //container that holds win text

    //At start of game
    void Start ()
    {
        //grab rigidbody from ball
        rb = GetComponent<Rigidbody>();

        //Intiate Count & End Game
        count = 0;
        SetCountText();
        winText.text = "";
    }

    //checked before any physics updates
    void FixedUpdate () 
    {
        //movement input : WASD or arrow key
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Vector 3 (x,y,z) and Rigidbody movement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    //Collision Detector  & Score Logging
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        //Destroy(new.gameObject); 
    }

    //Score & End Game Funtion
    void SetCountText ()
    {
        countText.text = "Count: " + count.ToString();

        if(count == 10)
        {
            winText.text = "You Win";
        }
    }
}
