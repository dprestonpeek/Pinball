using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBridge : MonoBehaviour
{
    public static InputBridge Instance;
    [SerializeField]
    ControlFlippers left, right;

    float timerL = 0;
    float timerR = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerL > 0)
        {
            timerL -= Time.deltaTime;
        }
        else
        {
            left.isPressed = false;
        }
        if (timerR > 0)
        {
            timerR -= Time.deltaTime;
        }
        else
        {
            right.isPressed = false;
        }
    }

    public void DoInputReturn()
    {
        TouchLeft();
    }

    public void DoInputLeft()
    {
        TouchLeft();
    }

    public void DoInputRight()
    {
        TouchRight();
    }

    public bool InputReturn()
    {
        return Input.GetKey(KeyCode.Return) || left.isPressed || right.isPressed;
    }

    public bool InputLeft()
    {
        return Input.GetKey(KeyCode.Q) || left.isPressed;
    }

    public bool InputLeftDown()
    {
        return Input.GetKeyDown(KeyCode.Q) || left.justPressed;
    }

    public bool InputLeftUp()
    {
        return Input.GetKeyUp(KeyCode.Q) || left.justLifted;
    }

    public bool InputRight()
    {
        return Input.GetKey(KeyCode.P) || right.isPressed;
    }

    public bool InputRightDown()
    {
        return Input.GetKeyDown(KeyCode.P) || right.justPressed;
    }

    public bool InputRightUp()
    {
        return Input.GetKeyUp(KeyCode.P) || right.justLifted;
    }

    public void TouchLeft()
    {
        if (GameManager.instance.starting)
        {
            timerL = 1;
        }
        else
        {
            timerL = .25f;
        }
        left.isPressed = true;
    }

    public void TouchRight()
    {
        if (GameManager.instance.starting)
        {
            timerR = 1;
        }
        else
        {
            timerR = .25f;
        }
        right.isPressed = true;
    }
}
