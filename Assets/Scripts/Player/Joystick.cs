using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    public GameObject JoystickIM;
    public GameObject JoystickBG;
    public Vector2 JoystickVec;
    private Vector2 JoystickTouchPos;
    private Vector2 JoystickOriginalPos;
    private float JoystickRadius;

    public void Start()
    {
        JoystickOriginalPos = JoystickBG.transform.position;
        JoystickRadius = JoystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    // Sets the initial position of the joystick
    public void PointerDown()
    {
        int counter = 0;
        List<Touch> touches = new List<Touch>();
        bool repeat = true;
        do
        {
            try
            {
                touches.Add(Input.GetTouch(counter));
                counter++;
            }
            catch
            {
                repeat = false;
            }
        } while (repeat);
        // Resolves the case where the user presses on two places at once
        if (touches.Count > 1)
        {
            float smallestDistance = 10000;
            int closestTouch = 0;
            for (int counter2 = 0; counter2 < touches.Count; counter2++)
            {
                if (Vector2.Distance(touches[counter].position, JoystickOriginalPos) < smallestDistance)
                {
                    smallestDistance = Vector2.Distance(touches[counter].position, JoystickOriginalPos);
                    closestTouch = counter2;
                }
            }
            JoystickIM.transform.position = touches[closestTouch].position;
            JoystickBG.transform.position = touches[closestTouch].position;
            JoystickTouchPos = touches[closestTouch].position;
        }
        else
        {
            JoystickIM.transform.position = Input.mousePosition;
            JoystickBG.transform.position = Input.mousePosition;
            JoystickTouchPos = Input.mousePosition;
        }
    }

    // Moves the joystick image
    public void OnMouseDrag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        JoystickVec = (dragPos -  JoystickTouchPos).normalized;

        float JoystickDist = Vector2.Distance(dragPos, JoystickTouchPos);

        if (JoystickDist < JoystickRadius)
        {
            JoystickIM.transform.position = JoystickTouchPos + JoystickVec * JoystickDist;
        }
        else
        {
            JoystickIM.transform.position = JoystickTouchPos + JoystickVec * JoystickRadius;
        }
    }

    // Resets the joystick
    public void PointerUp()
    {
        JoystickVec = Vector2.zero;
        JoystickIM.transform.position = JoystickOriginalPos;
        JoystickBG.transform.position = JoystickOriginalPos;
    }
}
