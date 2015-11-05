using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //variables
    public GameObject player;
    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        //subtract distance between player and camera
        offset = transform.position - player.transform.position; 

	}
	
	// LateUpdate will still run after all processes are updatd
	void LateUpdate ()
    {
        //Every frame, move the camera's offset position to player's position
        transform.position = player.transform.position + offset;
	}
}
