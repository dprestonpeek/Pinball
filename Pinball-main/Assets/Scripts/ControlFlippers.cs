using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlFlippers : MonoBehaviour
{
    public bool left = false;
    public bool isPressed = false;
    public bool justPressed = false;
    public bool justLifted = false;
    int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frame++;
    }

    private void LateUpdate()
    {
        if (frame != 0)
        {
            justPressed = false;
            justLifted = false;
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
        justPressed = true;
        frame = 0;
    }
    private void OnMouseUp()
    {
        isPressed = false;
        justLifted = true;
        frame = 0;
    }
}
