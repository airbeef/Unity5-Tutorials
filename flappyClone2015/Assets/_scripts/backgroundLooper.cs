using UnityEngine;
using System.Collections;

public class backgroundLooper : MonoBehaviour {

    int numBGPannels = 6; //number of background pannels (ground and sky)
    float widthBgObject;
    Vector3 camPosition;

    //Will not need Update or FixedUpdate since this is
    //a looping child of the camera
    void OnTriggerEnter2D(Collider2D bgCollider)
    {
        Debug.Log("Triggered" + bgCollider.name); //throws up console to show what's collided

        widthBgObject = ((BoxCollider2D)bgCollider).size.x; //whatever the size of bg's x axis is

        //position assignment / parallax scrolling
        //Note: looping child pulled back so camera will not see trigger collider
        camPosition = bgCollider.transform.position; //identify that camera position rests on collider's position (right)
        camPosition.x += widthBgObject * numBGPannels - widthBgObject/2; //position of camera = width of bg * 6. /2 kills gap
        bgCollider.transform.position = camPosition; //whatever bg is, that's where camera should be
	}
}
