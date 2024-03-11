using System;
using UnityEngine;
using UnityEngine.UI;

public class LabelText : MonoBehaviour
{
    private Text Label;
    public Type LabelType;
    private int Data;
    public enum Type
    {
        Winner,
        Distance,
        Generation,
        Testers,
    }

    public void Start()
    {
        Label = GetComponent<Text>();
        if (LabelType == Type.Winner)
        {
            Label.text = Stats.Winner + " Has Won!";
        }
        else if (LabelType == Type.Distance)
        {
            Label.text = "The Total Distance Was: " + Math.Round(Stats.DistanceTravelled).ToString();
        }
        else if (LabelType == Type.Generation)
        {
            Label.text = "Generation: " + Stats.Generation;            
        }
        else if (LabelType == Type.Testers)
        {
            Data = 120;
            Label.text = "Testers Alive: " + Data;            
        }
        else
        {
            throw new System.Exception();
        }        
    }

    // Decreases the number of testers recorded as alive
    public void Change()
    {
        if (LabelType == Type.Testers)
        {
            Data--;
            Label.text = "Testers Alive: " + Data;
        }
    }
}
