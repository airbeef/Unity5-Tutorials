using UnityEngine;
using System.Collections;

// When the button is pressed this contacts the fist controller and tells it to punch
// Should this have a cool down? Can you only press this based on the canpunch variable?

public class PunchButtonController : MonoBehaviour 
{
	private FistController _fistController;
	private Animator _anim;

	// Use this for initialization
	void Start () 
	{
		GameObject goTemp = (GameObject)GameObject.FindGameObjectWithTag ("Glove");
		_fistController = goTemp.GetComponent<FistController>();

		_anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnMouseDown()
	{
		// change the button's look, this is an animation
		_anim.SetTrigger ("Press");
		// tell the fist controller to punch
		_fistController.Punch ();

	}

}
