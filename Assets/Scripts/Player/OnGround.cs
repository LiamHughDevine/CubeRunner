using UnityEngine;

public class OnGround : MonoBehaviour
{
    private Movement Move;

    public void Start()
    {
        Move = gameObject.transform.parent.GetComponent<Movement>();
    }

    // If the collider hits the map / other player, it will allow the player to jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Map")
        {
            Move.TouchGround();
        }
    }

    // If the collider is no longer touching the map / other player, the player can't jump
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Map")
        {
            Move.LeaveGround();
        }
    }
}
