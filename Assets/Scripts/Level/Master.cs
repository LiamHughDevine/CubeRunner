using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    public GameObject VirtualCamera;
    protected List<Player> Players = new List<Player>();
    public ProceduralGeneration ProGen;
    protected int ChunkLength;
    public float Speed = 1f;
    public float JumpHeight = 1f;
    protected int CameraLine = 1000;
    public bool Arena = false;
    protected int ArenaCounter = 0;

    protected virtual void Start()
    {
        InitialiseCamera();
    }

    // Moves the camera to the correct position
    protected void InitialiseCamera()
    {
        float width = Camera.main.orthographicSize * Camera.main.aspect;
        float height = Camera.main.orthographicSize;
        VirtualCamera.transform.position += new Vector3(width + 1, height, 0f);
        ChunkLength = (int)Math.Ceiling(width * 2);
        CameraLine = ChunkLength - 10;
    }

    public virtual void InitialisePlayer(string name)
    {
        Players.Add(new Player(name, ChunkLength));        
    }

    public float GetSpeed()
    {
        return Speed * 4;
    }

    public float GetJumpHeight()
    {
        return JumpHeight * 7;
    }

    // Resolves the movement of a player
    public void MovePlayer(string name, float distance)
    {
        var currentPlayer = GetPlayer(name);

        currentPlayer.IncreaseDistance(distance);
        MoveGeneration(name);
        MoveCamera();
    }

    // Checks if either player is far away from the camera enough that it needs to move
    private void MoveCamera()
    {
        foreach (Player player in Players)
        {
            if (player.CameraDistance() > CameraLine)
            {
                float distanceChange = player.CameraDistance() - CameraLine;
                VirtualCamera.transform.position += new Vector3(distanceChange, 0f, 0f);                
                foreach (Player p in Players)
                {
                    p.MoveCamera(distanceChange);
                }
                return;
            }
        }
    }

    // Checks if any player has moved the minimum tiles that are needed for another generation to be needed
    private void MoveGeneration(string name)
    {
        if (GetPlayer(name).GenerationDistance() > ChunkLength)
        {
            if (ArenaCounter <= 0)
            {
                ProGen.CreateNewTiles();
            }
            else
            {
                ArenaCounter--;
            }
            foreach (Player player in Players)
            {
                player.MoveGeneration(ChunkLength);
            }
        }
    }

    // Returns a player based on their name
    protected Player GetPlayer(string name)
    {
        foreach (Player player in Players)
        {
            if (player.GetName() == name)
            {
                return player;
            }
        }
        throw new Exception();
    }

    // Checks if the player is too far to the left or too far down
    public bool CheckDeath(string name, float yPosition)
    {
        var currentPlayer = GetPlayer(name);

        if (currentPlayer.CameraDistance() < 0.5 || yPosition < -1)
        {
            ResolveDeath(name);
            return true;
        }
        return false;
    }

    protected abstract void ResolveDeath(string name);
}