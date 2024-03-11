using System;
using System.IO;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    [SerializeField]
    private Matrix[] Layers;
    [SerializeField]
    private Matrix Bias;
    [SerializeField]
    private int NumberOfLayers = 4;
    [SerializeField]
    private int NodesPerLayer = 12;
    [SerializeField]
    private int Inputs = 8;
    [SerializeField]
    private int Outputs = 3;
    [SerializeField]
    private int Generation;

    public NeuralNetwork(int seed)
    {
        Generation = 0;
        CreateLayers(seed);
        CreateBias(seed);
    }

    public NeuralNetwork(string path)
    {
        NeuralNetwork net = Load(path);
        Layers = net.Layers;
        Bias = net.Bias;
        NumberOfLayers = net.NumberOfLayers;
        NodesPerLayer = net.NodesPerLayer;
        Inputs = net.Inputs;
        Outputs = net.Outputs;
        Generation = net.Generation;
    }

    // The key values that influence the changes made are the generation and the placement of the parent network
    // The higher the generation, the less drastic changes will be made and the better the place,
    // the less drastic changes will be made
    public void Modify(int seed, int place)
    {
        Generation++;
        foreach(Matrix layer in Layers)
        {
            layer.Modify(seed + Stats.Modifications, place, Generation);
            Stats.Modifications++;
        }
        Bias.Modify(seed + Stats.Modifications, place, Generation);
        Stats.Modifications++;
    }

    public void IncreaseGeneration()
    {
        Generation++;
    }

    private void CreateLayers(int seed)
    {
        Layers = new Matrix[NumberOfLayers + 1];
        // Creates each of the layers. The first and last layers must be different sizes in order for the input and
        // output to work correctly
        Layers[0] = new Matrix(NodesPerLayer, Inputs);
        Layers[0].Randomise(seed);
        Layers[NumberOfLayers] = new Matrix(Outputs, NodesPerLayer);
        Layers[NumberOfLayers].Randomise(seed);
        for (int counter = 1; counter < NumberOfLayers; counter++)
        {
            Layers[counter] = new Matrix(NodesPerLayer, NodesPerLayer);
            Layers[counter].Randomise(seed);
        }
    }

    private void CreateBias(int seed)
    {
        Bias = new Matrix(NodesPerLayer, NumberOfLayers);
        Bias.Randomise(seed);
    }

    public int[] Decision(double[] input)
    {
        try
        {
            // Processes the inputs to get the final matrix
            Matrix End = RecurseNodes(new Matrix(input, Matrix.VectorType.Column), NumberOfLayers + 1);

            // Turns the matrix into 1's and 0's
            End.Standardise();

            // Converts the final output into an array of doubles
            double[] dOutput = End.GetColumn(0);
            int[] output = new int[dOutput.Length];
            for (int counter = 0; counter < dOutput.Length; counter++)
            {
                output[counter] = Convert.ToInt32(dOutput[counter]);
            }

            return output;
        }
        catch (InvalidSize e)
        {
            // Resizes the neural network
            Debug.Log(e);
            Inputs = input.Length;
            CreateLayers(Stats.BotsMade);
            CreateBias(Stats.BotsMade);
            Stats.BotsMade++;
            return Decision(input);
        }
    }

    private Matrix RecurseNodes(Matrix currentNodes, int recursionsLeft)
    {
        // Recurses until 0 left
        if (recursionsLeft == 0)
        {
            // Returns the original inputs (this is the start of processing)
            return currentNodes;
        }
        else if (recursionsLeft == NumberOfLayers + 1)
        {
            return CreateFinalNodes(RecurseNodes(currentNodes, recursionsLeft - 1), Layers[recursionsLeft - 1]);
        }
        else
        {
            // Creates the new nodes using the previous nodes
            return CreateNodes(RecurseNodes(currentNodes, recursionsLeft - 1), Layers[recursionsLeft - 1], Bias.GetColumn(recursionsLeft - 1));
        }
    }

    private Matrix CreateNodes(Matrix currentNodes, Matrix weights, double[] biases)
    {
        // Multiplies the previous nodes by the weights and adds any bias
        return Matrix.Add(Matrix.Multiply(weights, currentNodes), new Matrix(biases, Matrix.VectorType.Column));
    }

    private Matrix CreateFinalNodes(Matrix currentNodes, Matrix weights)
    {
        // Multiplies the previous nodes by the weights
        return Matrix.Multiply(weights, currentNodes);
    }

    public int GetGeneration()
    {
        return Generation;
    }

    public void Save(string path)
    {
        // Prepares each matrix for saving
        foreach (Matrix Layer in Layers)
        {
            Layer.PrepareJSON();
        }
        Bias.PrepareJSON();
        string jsonString = JsonUtility.ToJson(this);

        try
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + path);
            sw.Write(jsonString);
            sw.Close();
        }
        catch
        {
            Debug.Log("Saving failed");
        }
    }

    public static NeuralNetwork Load(string path)
    {
        try
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + path);
            string jsonString = sr.ReadLine();
            sr.Close();
            NeuralNetwork net = JsonUtility.FromJson<NeuralNetwork>(jsonString);
            // Converts each matrix back to normal after loading
            foreach (Matrix layer in net.Layers)
            {
                layer.ConvertFromJSON();
            }
            net.Bias.ConvertFromJSON();
            return net;
        }
        catch
        {
            Debug.Log("Loading Failed");
            try
            {
                // Tries another location
                var textFile = Resources.Load<TextAsset>(path);
                NeuralNetwork net = JsonUtility.FromJson<NeuralNetwork>(textFile.ToString());
                foreach (Matrix layer in net.Layers)
                {
                    layer.ConvertFromJSON();
                }
                net.Bias.ConvertFromJSON();
                return net;
            }
            catch
            {
                // Creates a random neural network
                Stats.BotsMade++;
                return new NeuralNetwork(Stats.BotsMade - 1);
            }
        }
    }
}