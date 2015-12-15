using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite; //damaged sprite visuals
    public int hp = 4; //wall damage

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake ()
    {
        //get sprite rendering components
        spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("Wall.Awake");
    }
	
    //Wall damage function
    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = dmgSprite; //visual feedback that wall is damaged
        hp -= loss; //subtract hitpoints from wall

        //check to see if hit points are less than / equal to 0; disable object if it equals 0
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }

        Debug.Log("Wall.DamageWall");
    }
	
}
