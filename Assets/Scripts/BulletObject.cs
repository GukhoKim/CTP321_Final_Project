using UnityEngine;
using System.Collections;

public abstract class BulletObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask highBlockingLayer;
    public LayerMask enemy;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;


	// Use this for initialization
	protected virtual void Start ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}

    protected bool BulletMove(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end;
        int range = (int)new Vector2(xDir, yDir).magnitude;
        Vector2 unit = new Vector2(xDir/range, yDir/range);

        end = start + unit;  
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, enemy | highBlockingLayer);
        
        int i = 0;
        while (i < range - 1 && hit.transform == null)
        {
            end = end + unit;
            hit = Physics2D.Linecast(start, end, enemy | highBlockingLayer);
            i++;
        }
        boxCollider.enabled = true;
        
        Debug.Log(hit.transform);
        StartCoroutine(SmoothMovement(end));
        if (hit.transform == null)
            return true;
        return false;
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        Destroy(gameObject);
    }

    protected virtual void BulletFired<T>(int xDir, int yDir, int range)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = BulletMove(xDir*range, yDir*range, out hit);
        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            somethingHit(hitComponent);
    }

    protected abstract void somethingHit<T>(T compoent)
        where T : Component;
}
