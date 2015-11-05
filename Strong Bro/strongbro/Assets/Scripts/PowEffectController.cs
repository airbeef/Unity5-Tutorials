using UnityEngine;
using System.Collections;

public class PowEffectController : MonoBehaviour 
{
	public float maximum = 0.5f;
	public float minimum = 0.05f;

    Animator anim;

	// Use this for initialization
	void Start () 
    {
        Debug.Log("In Start for PowEffectController");
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // debug to test Pow
        //if (Input.GetButtonDown("Punch"))
        //{
        //    DisplayEffect(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //}
	}

    public void DisplayEffect()
    {
        // move the transform randomly here.
		float radius = Random.Range (minimum, maximum);
		int anglePow = Random.Range(0, 359);

		float xPow = radius * Mathf.Cos (anglePow);
		float yPow = radius * Mathf.Sin (anglePow);

		transform.position = new Vector3 (xPow, yPow, transform.position.z);

        //gameObject.SetActive(true);
        if (anim == null)
            anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Pow");
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
