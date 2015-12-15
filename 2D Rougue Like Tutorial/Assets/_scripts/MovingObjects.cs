using UnityEngine;
using System.Collections;

public abstract class MovingObjects : MonoBehaviour
{
    public float moveTime = 0.1f; //time it takes PC/NPCs to move
    public LayerMask blockingLayer; //checks collision of pc,npc and obstacle layer

    private BoxCollider2D boxCollider; //
    private Rigidbody2D rb2D; // stores component references of unity moving
    private float inverseMoveTime; //makes movement calculations more efficient


    // can be overwritten by inheritant classes in  
    // case of different implementation
    protected virtual void Start()
    {
        Debug.Log("MovingObjects.Start");

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime; //multiplies to save computation time

    }

    //returns more than one value for movement and object detection
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Debug.Log("MovingObjects.Move");

        Vector2 start = transform.position; //initial position, Vector2 discards z axis data
        Vector2 end = start + new Vector2(xDir, yDir); // end movement calculated by Dir parameters

        boxCollider.enabled = false; //turns of PC's own collider
        hit = Physics2D.Linecast(start, end, blockingLayer); //declares what can be hit in blockinglayer tag
        boxCollider.enabled = true; //turns PC collider back on if in contact with blockingLayer tagged items

        //checks to see if blockinglayer items actually come into conteact
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end)); //starts movement function
            return true; //PC was able to move on the board
        }

        return false; //move was unsuccessful

        
    }

    //Moves npcs/pcs. 'end' dictates where chracter should move to
    protected IEnumerator SmoothMovement(Vector3 end)
    {

        Debug.Log("MovingObjects.SmoothMovement");

        float sqrRemaingDistance = (transform.position - end).sqrMagnitude; //calculates movement from starting position to end paramenter. sqrMagnitude is cheaper than Magnitude

        //checks ssqrRemaingDistance is greater than float.epsilon; used to calculate small numbers
        while (sqrRemaingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime); //calculates initial position, end position, time it takes to get there
            rb2D.MovePosition(newPosition); //moves to new position
            sqrRemaingDistance = (transform.position - end).sqrMagnitude; //calculates remaining distance
            yield return null; //waits a fram before reevaluating loop
        }

    }


    //specifies what type of component is interacted with if blocked (enemies to player, PC to inner walls)
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component //generic variable to store movement
    {
        Debug.Log("MovingObjects.AttemptMove | Check onCantMove just in case");

        RaycastHit2D hit; //can PC or NPC hit anything?

        bool canMove = Move(xDir, yDir, out hit); //Can anyone move and check if they can hit anything?

        //In case if something was hit, get Component reference ex: Enemy vs Player / Player vs obstacles, etc.
        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        //If PC or NPC can't move but can hit something
        if (!canMove && hitComponent != null)

            onCantMove(hitComponent);
    }

    // returns void, takes parameter and component 'T' to indicates missing, or incomplete, implementation
    protected abstract void onCantMove<T>(T component)
         where T : Component;
 
}
