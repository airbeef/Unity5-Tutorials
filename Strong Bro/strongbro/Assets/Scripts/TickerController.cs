using UnityEngine;
using System.Collections;



public class TickerController : MonoBehaviour 
{

	public static TextMesh _texMesh;
	public static int iCurrScore;

	// Use this for initialization
	void Start () 
	{
		_texMesh = gameObject.GetComponentInChildren<TextMesh> ();

		iCurrScore = 0;
		_texMesh.text = "Pow: " + iCurrScore;
	}
	
	// Update is called once per frame
	void Update () 
	{
	    
	}

	public static void UpdateScore(int iScore)
	{
		iCurrScore += iScore;
		_texMesh.text = "Pow: " + iCurrScore;
		//Debug.Log ("Incrementing score");
	}
}
