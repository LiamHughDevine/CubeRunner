using UnityEngine;
using System;

public class HumanMovement : Movement
{
    public Joystick joystick;
    protected override int[] SelectedMove()
    {
        try
        {
            int[] output = new int[2];

            // Checks if the joystick is moving left or right
            if (joystick.JoystickVec.x > 0)
            {
                output[1] = 1;
            }
            else if (joystick.JoystickVec.x < 0)
            {
                output[1] = -1;
            }

            // Checks if the jump button has been pressed
            if (Stats.JumpPressed)
            {
                output[0] = 1;
                Stats.JumpPressed = false;
            }
            else
            {
                output[0] = 0;
            }
            return output;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            // If there is an error, the game will not make a move for the player
            int[] output = new int[] { 0, 0, 0 };
            return output;
        }
    }
}