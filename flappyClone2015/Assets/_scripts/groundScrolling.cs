using UnityEngine;
using System.Collections;

public class groundScrolling : MonoBehaviour {

    public float groundSpeed;
    Vector3 groundPos;


	// FixedUpdate due to ground interacting with physics
	void FixedUpdate () {

        groundPos = transform.position; //moves the ground
        groundPos.x += -groundSpeed * Time.deltaTime; //moves ground left
        transform.position = groundPos; //rests ground
	}
}
