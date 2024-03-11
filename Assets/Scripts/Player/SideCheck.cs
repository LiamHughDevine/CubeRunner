using UnityEngine;

public class SideCheck : MonoBehaviour
{
    private Movement Move;
    public enum Side
    {
        Left,
        Right
    }
    public Side SideToCheck;

    public void Start()
    {
        Move = gameObject.transform.parent.GetComponent<Movement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevents the player moving in the specified direction
        if (SideToCheck == Side.Left)
        {
            Move.SideCheckInput(true, true);
        }
        else
        {
            Move.SideCheckInput(false, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Allows the user to move in the specified direction again
        if (SideToCheck == Side.Left)
        {
            Move.SideCheckInput(true, false);
        }
        else
        {
            Move.SideCheckInput(false, false);
        }

    }
}
