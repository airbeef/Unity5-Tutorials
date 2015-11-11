using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

    Transform player; //player tag
    GameObject player_go; //player game object
    Vector3 playerposition; //player's position on screen
    float offsetX; //offset between player and camera

	// Use this for initialization
	void Start () {

        //Debug script in case object is lost
        player_go = GameObject.FindGameObjectWithTag("player");
        if(player_go== null)
        {
            Debug.LogError("Can't find object with tag: 'player'!");
            return; 
        }

        player = player_go.transform; //transform tag to game object.

        offsetX = transform.position.x - player.position.x; //how the camera will offset player
	}
	
	// Update is called once per frame
	void Update () {

        //if the player isn't dead
        if(player != null)
        {

            playerposition = transform.position; //player's established position
            playerposition.x = player.position.x + offsetX; //keep the camera consistently where the player is.
            transform.position = playerposition; //keep the two entities in the same frame
        }
        
	}
}
