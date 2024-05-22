using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField]
    Username setUsername;

    public bool soundOn;
    [SerializeField]
    TMP_Text soundToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (soundOn && soundToggle.text != "Sound OFF")
        {
            soundToggle.text = "Sound OFF";
        }
        else if (!soundOn && soundToggle.text != "Sound ON")
        {
            soundToggle.text = "Sound ON";
        }
    }

    public void SoundToggle()
    {
        soundOn = !soundOn;
    }

    public void ScreenScale()
    {
        GameManager.instance.ResetGameScaleStart();
    }

    public void SetUsername()
    {
        setUsername.ChangeUsername();
    }
}
