using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    // This controls the horizontal / vertical movement of all players
    protected float Speed;
    protected float JumpHeight;
    private bool OnGround = false;
    private bool LeftCheck = false;
    private bool RightCheck = false;
    public Master M;
    public string Name;

    protected virtual void Start()
    {
        // Initialises the player object
        M.InitialisePlayer(Name);

        Speed = M.GetSpeed();
        JumpHeight = M.GetJumpHeight();
    }

    public void Update()
    {
        // Gets an array containing the moves
        // [0] = whether or not the player is jumping
        // [1] = whether the player is moving left / right / not at all
        int[] move = SelectedMove();

        Jump(move[0]);

        HorizontalMove(move[1]); 

        // Checks if the player has died
        if (M.CheckDeath(Name, transform.position.y))
        {
            gameObject.SetActive(false);
        }
    }

    public void TouchGround()
    {
        OnGround = true;
    }

    public void LeaveGround()
    {
        OnGround = false;
    }

    public void SideCheckInput (bool left, bool value)
    {
        if (left)
        {
            LeftCheck = value;
        }
        else
        {
            RightCheck = value;
        }
    }

    private void HorizontalMove(int direction)
    {
        Vector3 movement;

        float horizontal = direction;

        // Checks to see if there is anything left or right in order to prevent jittering
        if (horizontal < 0 && LeftCheck == false)
        {
            horizontal = Time.deltaTime * Speed * -1;
            
            // If the player is in the air, they move slower
            if (OnGround)
            {
                horizontal *= 1.2f;
            }
            // Declares how far the player will move
            movement = new Vector2(horizontal, 0f);
            // Resolves the movement
            M.MovePlayer(Name, horizontal);
        }
        else if (horizontal > 0 && RightCheck == false)
        {
            horizontal = Time.deltaTime * Speed;
            if (OnGround)
            {
                horizontal *= 1.2f;
            }
            movement = new Vector2(horizontal, 0f);
            M.MovePlayer(Name, horizontal);
        }
        else
        {
            movement = new Vector2(0f, 0f);
        }

        // Moves the player by that amount
        transform.position += movement;
    }

    private void Jump(int jump)
    {
        var body = gameObject.GetComponent<Rigidbody2D>();
        var vel = body.velocity.y;

        // Checks that the player is on the ground and wants to jump and isnt already jumping (this is due to a weird bug)
        if (jump == 1 && OnGround && vel <= 0 && OnGround)
        {
            // Adds an upwards force to create a realistic jump
            body.AddForce(new Vector2(0f, JumpHeight), ForceMode2D.Impulse);
            LeaveGround();
        }
    }

    protected abstract int[] SelectedMove();
}