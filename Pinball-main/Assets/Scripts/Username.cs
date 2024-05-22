using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Username : MonoBehaviour
{
    [SerializeField]
    GameObject setUsername;
    [SerializeField]
    TMP_InputField newUsername;
    [SerializeField]
    TMP_Text currentUsername;
    public string username = "Guest";
    public bool changing = false;

    // Start is called before the first frame update
    void Start()
    {
        GetCurrentUsername();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            username = PlayerPrefs.GetString("username");
            currentUsername.text = username;
            GameManager.usernameSet = true;
            GameManager.instance.optionUsername = true;
        }
        else
        {
            changing = true;
            GameManager.instance.ActivateStartGame(false);
            setUsername.SetActive(true);
        }
    }

    public void ChangeUsername()
    {
        username = newUsername.text;
        currentUsername.text = username;
        PlayerPrefs.SetString("username", username);
        changing = false;
        setUsername.SetActive(false);
        GameManager.instance.ActivateStartGame(true);
        GameManager.instance.optionUsername = true;
    }

    private TMP_Text GetCurrentUsername()
    {
        TMP_Text[] textObjs = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text obj in textObjs)
        {
            if (obj.tag == "username")
            {
                return obj;
            }
        }
        return null;
    }
}
