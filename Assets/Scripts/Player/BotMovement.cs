using UnityEngine;
using System;

public class BotMovement : Movement
{
    // For the neural network input
    private const double yOffset = 0.61;
    private const double xOffset = 0.44;
    public bool Tester = false;
    private NeuralNetwork Controller;
    private float Timer;
    private float PreviousPosition;

    protected override void Start()
    {
        if (Tester)
        {
            // Initialises the player object
            Speed = M.GetSpeed();
            JumpHeight = M.GetJumpHeight();
        }
        else
        {
            M.InitialisePlayer(Name);
            Controller = new NeuralNetwork("NeuralNetwork2");
            // Initialises the player object

            Speed = M.GetSpeed();
            JumpHeight = M.GetJumpHeight();
        }
        Timer = Time.time;
        PreviousPosition = transform.position.x;
    }

    public void InputNetwork(string path)
    {
        Controller = new NeuralNetwork(path);
    }

    public void Modify(int place)
    {
        Controller.Modify(Stats.BotsMade, place);
        Stats.BotsMade++;
    }

    public void InputSpeed(float speed)
    {
        Speed = speed;
    }

    public void InputJumpHeight(float jumpheight)
    {
        JumpHeight = jumpheight;
    }

    public NeuralNetwork GetController()
    {
        return Controller;
    }

    public int GetGeneration()
    {
        return Controller.GetGeneration();
    }

    public void IncreaseGeneration()
    {
        Controller.IncreaseGeneration();
    }

    protected override int[] SelectedMove()
    {
        // Helps to prevent the bot getting stuck
        if (transform.position.x != PreviousPosition)
        {
            Timer = Time.time;
        }
        else
        {
            if (Timer > Time.time + 2)
            {
                int[] output = new int[] { 0, 1 };
                return output;
            }
        }

        // Finds the input values
        // 0-6 refer to tiles and 7 refers to how far along the bot is across a tile
        double[] input = new double[8];

        int xTile = Convert.ToInt32(Math.Floor(transform.position.x - xOffset));
        int yTile = Convert.ToInt32(Math.Floor(transform.position.y - yOffset)) - 1;
        for (int counter = 0; counter < 7; counter++)
        {
            int x = xTile;
            int y = yTile;
            if (counter > 4)
            {
                y += 3;
            }
            else if (counter > 3)
            {
                y += 2;
            }
            else if (counter > 2)
            {
                y += 1;
            }
            else if (counter == 0)
            {
                y -= 1;
            }

            if (counter == 0 || counter == 2 || counter == 3 || counter == 4 || counter == 6)
            {
                x += 1;
            }

            if (y < 0 || y >= Stats.TileColumns[x].Length)
            {
                input[counter] = -1;
            }
            else
            {
                if (Stats.TileColumns[x][y] == 1)
                {
                    input[counter] = 1;
                }
                else if (Stats.TileColumns[x][y] == 0)
                {
                    input[counter] = -1;
                }
            }
        }
        if (transform.position.x - xTile > 0.7)
        {
            input[7] = 1;
        }
        else
        {
            input[7] = -1;
        }

        try
        {
            int[] decision = Controller.Decision(input);

            int[] output = new int[2];
            // Output 0 determines whether or not the bot will jump
            output[0] = decision[0];
            // Decision 1 is whether or not the player moves left. Decision 2 is whether they move right
            // If both are 1, then the player doesn't move
            if ((decision[1] == 1 && decision[2] == 1) || decision[1] == 0 && decision[2] == 0)
            {
                output[1] = 0;
            }
            else if (decision[1] == 1)
            {
                output[1] = 1;
            }
            else if (decision[2] == 1)
            {
                output[1] = -1;
            }
            return output;
        }
        catch
        {
            // If there is an error, the bot will move to the right and jump as this is usually a good move to make
            int[] output = new int[] { 1, 1};
            return output;
        }
    }
}