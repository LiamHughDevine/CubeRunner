public class Player
{
    // Stores distance values for a single player
    private readonly string Name;
    private float DistanceFromGeneration;
    private float DistanceFromCamera = 9;
    private float TotalDistanceTravelled;

    public Player(string name, float chunkLength)
    {
        Name = name;
        DistanceFromGeneration = chunkLength / 2;
    }

    public string GetName()
    {
        return Name;
    }

    // Increases all stored distances
    public void IncreaseDistance(float distance)
    {
        DistanceFromGeneration += distance;
        DistanceFromCamera += distance;
        TotalDistanceTravelled += distance;
    }

    public float CameraDistance()
    {
        return DistanceFromCamera;
    }

    public float GenerationDistance()
    {
        return DistanceFromGeneration;
    }

    public float TotalDistance()
    {
        return TotalDistanceTravelled;
    }

    public void MoveCamera(float distance)
    {
        DistanceFromCamera -= distance;
    }

    public void MoveGeneration(int distance)
    {
        DistanceFromGeneration -= distance;
    }
}