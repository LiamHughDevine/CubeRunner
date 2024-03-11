using System.Collections.Generic;
using UnityEngine;

public class TestMaster : Master
{
    private readonly Dictionary<Player, NeuralNetwork> Testers = new Dictionary<Player, NeuralNetwork>();
    public Rigidbody2D Bot;
    private float StartTime;
    private readonly List<NeuralNetwork> Victors = new List<NeuralNetwork>();
    private readonly int TotalTesters = 120;
    private float PreviousMax = 0;
    public LabelText TestersLabel;
    public LabelText GenerationLabel;
    public Color32[] Colours = new Color32[5];


    protected override void Start()
    {
        StartTime = Time.time;
        InitialiseCamera();

        // Finds the previous generation number
        NeuralNetwork generation = new NeuralNetwork("NeuralNetwork2");
        try
        {
            Stats.Generation = generation.GetGeneration();
            GenerationLabel.Start();
        }
        catch { }

        // Adds tester bots to the map
        for (int counter = 0; counter < TotalTesters; counter ++)
        {
            Vector3 pos = new Vector3(9, 19);
            string name = "Bot" + counter ;
            var bot = Instantiate(Bot, pos, transform.rotation);
            ChangeColour(counter, bot);
            bot.name = name;
            BotMovement move = bot.GetComponent<BotMovement>();
            move.Tester = true;
            move.M = this;
            move.Name = name;
            move.InputSpeed(Speed);
            move.InputJumpHeight(JumpHeight);
            Modify(counter, move);
            InitialisePlayerNetwork(name, move.GetController());
        }
    }

    // Changes the colour based on what neural network spawned the bot
    private void ChangeColour(int counter , Rigidbody2D bot)
    {
        if (counter > 60)
        {
            bot.GetComponent<SpriteRenderer>().color = Colours[1];
        }
        else if (counter == 60)
        {
            bot.GetComponent<SpriteRenderer>().color = Colours[0];
        }
        else if (counter > 20)
        {
            bot.GetComponent<SpriteRenderer>().color = Colours[3];
        }
        else if (counter == 20)
        {
            bot.GetComponent<SpriteRenderer>().color = Colours[2];
        }
        else if (counter == 0)
        {
            bot.GetComponent<SpriteRenderer>().color = Colours[4];
        }
        else
        {
            bot.GetComponent<SpriteRenderer>().color = Colours[5];
        }
    }

    // Modifies the neural network
    private void Modify(int counter, BotMovement move)
    {
        if (counter > 60)
        {
            move.InputNetwork("NeuralNetwork2");
            move.Modify(1);
        }
        else if (counter == 60)
        {
            move.InputNetwork("NeuralNetwork2");
            move.IncreaseGeneration();
        }
        else if (counter > 20)
        {
            move.InputNetwork("NeuralNetwork1");
            move.Modify(2);
        }
        else if (counter == 20)
        {
            move.InputNetwork("NeuralNetwork1");
            move.IncreaseGeneration();
        }
        else if (counter == 0)
        {
            move.InputNetwork("NeuralNetwork0");
            move.IncreaseGeneration();
        }
        else
        {
            move.InputNetwork("NeuralNetwork0");
            move.Modify(3);
        }
    }

    public void Update()
    {
        // Restarts the timer if the furthest distance any player has travelled is increased
        foreach (Player player in Players)
        {
            if (player.TotalDistance() > PreviousMax + 1)
            {
                StartTime = Time.time;
                PreviousMax = player.TotalDistance();
            }
        }

        // If the maximum distance has not increased in 20 seconds, it is likely that all testers have got stuck
        if (Time.time - StartTime >= 20)
        {
            ResolveTimer();
        }
    }

    // If nobody moves forward for enough time, the game will end
    private void ResolveTimer()
    {
        if (Testers.Count > 3)
        {
            // Finds the position of the best 3 players
            float[] distances = new float[3] { -20, -20, -20 };
            Player[] bestPlayers = new Player[3];
            foreach (Player player in Players)
            {
                if (Testers.ContainsKey(player))
                {
                    // If the player is further ahead of any of the bestPlayers, they will become a new bestPlayer
                    float distance = player.TotalDistance();
                    if (distance > distances[0])
                    {
                        distances[2] = distances[1];
                        bestPlayers[2] = bestPlayers[1];
                        distances[1] = distances[0];
                        bestPlayers[1] = bestPlayers[0];
                        distances[0] = distance;
                        bestPlayers[0] = player;
                    }
                    else if (distance > distances[1])
                    {
                        distances[2] = distances[1];
                        bestPlayers[2] = bestPlayers[1];
                        distances[1] = distance;
                        bestPlayers[1] = player;
                    }
                    else if (distance > distances[2])
                    {
                        distances[2] = distance;
                        bestPlayers[2] = player;
                    }
                }
            }

            Victors.Add(Testers[bestPlayers[2]]);
            Victors.Add(Testers[bestPlayers[1]]);
            Victors.Add(Testers[bestPlayers[0]]);
        }
        else if (Testers.Count == 3)
        {
            // Organises the final 3 players
            List<Player> remaining = new List<Player>(Testers.Keys);
            if (remaining[0].TotalDistance() >= remaining[1].TotalDistance() && remaining[0].TotalDistance() >= remaining[2].TotalDistance())
            {
                if (remaining[1].TotalDistance() >= remaining[2].TotalDistance())
                {
                    Victors.Add(Testers[remaining[2]]);
                    Victors.Add(Testers[remaining[1]]);
                    Victors.Add(Testers[remaining[0]]);
                }
                else
                {
                    Victors.Add(Testers[remaining[1]]);
                    Victors.Add(Testers[remaining[2]]);
                    Victors.Add(Testers[remaining[0]]);
                }
            }
            else if (remaining[1].TotalDistance() >= remaining[0].TotalDistance() && remaining[1].TotalDistance() >= remaining[2].TotalDistance())
            {
                if (remaining[0].TotalDistance() >= remaining[2].TotalDistance())
                {
                    Victors.Add(Testers[remaining[2]]);
                    Victors.Add(Testers[remaining[0]]);
                    Victors.Add(Testers[remaining[1]]);
                }
                else
                {
                    Victors.Add(Testers[remaining[0]]);
                    Victors.Add(Testers[remaining[2]]);
                    Victors.Add(Testers[remaining[1]]);
                }
            }
            else
            {
                if (remaining[0].TotalDistance() >= remaining[1].TotalDistance())
                {
                    Victors.Add(Testers[remaining[1]]);
                    Victors.Add(Testers[remaining[0]]);
                    Victors.Add(Testers[remaining[2]]);
                }
                else
                {
                    Victors.Add(Testers[remaining[0]]);
                    Victors.Add(Testers[remaining[1]]);
                    Victors.Add(Testers[remaining[2]]);
                }
            }
        }
        else if (Testers.Count == 2)
        {
            // Organises the final 2 players
            List<Player> remaining = new List<Player>(Testers.Keys);
            if (remaining[0].TotalDistance() >= remaining[1].TotalDistance())
            {
                Victors.Add(Testers[remaining[1]]);
                Victors.Add(Testers[remaining[0]]);
            }
            else
            {
                Victors.Add(Testers[remaining[0]]);
                Victors.Add(Testers[remaining[1]]);
            }
        }
        else if (Testers.Count == 1)
        {
            List<Player> remaining = new List<Player>(Testers.Keys);
            Victors.Add(Testers[remaining[0]]);
        }

        Save();
        // Repeats the test
        if (Arena)
        {
            ChangeScene.ChangeToScene(4);
        }
        else
        {
            ChangeScene.ChangeToScene(3);
        }
    }

    protected override void ResolveDeath(string name)
    {
        try
        {
            // Updates the number of Testers
            TestersLabel.Change();
        }
        catch { }
        if (Testers.Count <= 3)
        {
            // Adds the player to Victors
            Victors.Add(Testers[GetPlayer(name)]);
        }
        // Removes the player from Testers
        Testers.Remove(GetPlayer(name));
        if (Testers.Count == 1)
        {
            foreach(Player player in Players)
            {
                if (Testers.ContainsKey(player))
                {
                    Victors.Add(Testers[player]);
                }
            }
            Save();
            // Repeats the test
            if (Arena)
            {
                ChangeScene.ChangeToScene(4);
            }
            else
            {
                ChangeScene.ChangeToScene(3);
            }            
        }
    }

    // Saves the best neural networks
    private void Save()
    {
        for (int counter = 0; counter < Victors.Count; counter ++)
        {
            Victors[counter].Save("NeuralNetwork" + counter );
        }
    }

    // Adds the testers
    private void InitialisePlayerNetwork(string name, NeuralNetwork net)
    {
        if (Players.Count == 0)
        {
            if (Arena)
            {
                ArenaCounter = 4;
            }
        }
        Player player = new Player(name, ChunkLength);
        Players.Add(player);
        Testers.Add(player, net);
    }
}