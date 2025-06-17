using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScaleScreen : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    TMP_Text scale;
    [SerializeField]
    GameObject scaleScreen;
    //public float gameScale = 1.25f;
    public Vector3 gameScale = Vector3.one * 1.25f;
    public bool screenScaleSet = false;
    private float lockedGameScale = 1.25f;
    private string gameScaleString = "1.25";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AutoScale()
    {
        // Game dimensions
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        Vector2 screenDimemsions = new Vector2(screenWidth, screenHeight);
        Debug.Log("Screen width: " + screenDimemsions.x + ", Screen height: " + screenDimemsions.y);

        // Display dimensions
        int currentResolutionWidth = Screen.currentResolution.width;
        int currentResolutionHeight = Screen.currentResolution.height;
        Vector2 currentResolution = new Vector2(currentResolutionWidth, currentResolutionHeight);
        Debug.Log("Current resolution width: " + currentResolution.x + ", Current resolution height: " + currentResolution.y);

        Vector2 fullHD = new Vector2(1920, 1080);
        Vector2 TitanSlim = new Vector2(768, 1280);
        Vector2 OnePlus7Pro = new Vector2(1440, 3120);

        int[] ratio_16x9 = { 16, 9 };
        int[] ratio_3x5 = { 3, 5 };
        int[] ratio_6x13 = { 6, 13 };

        if (currentResolution == fullHD)
        {
            //we are on a PC, likely in-editor. Turn on debug mode.
            GameManager.instance.debugMode = true;
        }

        Accept();
    }

    public void ShowScaleScreen()
    {
        scaleScreen.SetActive(true);
    }

    public void SetScreenScale(Vector3 newScale)
    {
        parent.transform.localScale = newScale;
        gameScale = newScale;
        scale.text = GetNewGameScaleValue(gameScale);
        scaleScreen.SetActive(false);
        screenScaleSet = true;
    }

    public void ScaleGameUp()
    {
        Vector3 newScale = parent.transform.localScale;
        newScale.x += .05f;
        newScale.y += .05f;
        newScale.z += .05f;
        parent.transform.localScale = newScale;
        gameScale = newScale;
        scale.text = GetNewGameScaleValue(gameScale);
    }

    public void ScaleGameDown()
    {
        //if (gameScale > .3f)
        {
            Vector3 newScale = parent.transform.localScale;
            newScale.x -= .05f;
            newScale.y -= .05f;
            newScale.z -= .05f;
            parent.transform.localScale = newScale;
            gameScale = newScale;
            scale.text = GetNewGameScaleValue(gameScale);
        }
    }

    public void Accept()
    {
        PlayerPrefs.SetFloat("gameScaleX", gameScale.x);
        PlayerPrefs.SetFloat("gameScaleY", gameScale.y);
        PlayerPrefs.SetFloat("gameScaleZ", gameScale.z);
        //scaleScreen.SetActive(false);
        GameManager.instance.ActivateStartGame(true);
        screenScaleSet = true;
        GameManager.instance.optionScreenScale = true;
    }

    private string GetNewGameScaleValue(Vector3 gameScale)
    {
        string theScale = gameScale.ToString();
        string[] scalePlaces = theScale.Split(".");
        string dec = "00";
        if (scalePlaces.Length > 1)
        {
            if (scalePlaces[1].Length > 2)
            {
                dec = scalePlaces[1][0] + "" + scalePlaces[1][1];
            }
            else if (scalePlaces[1].Length == 2)
            {
                dec = scalePlaces[1];
            }
            else if (scalePlaces[1].Length == 1)
            {
                dec = scalePlaces[1] + "0";
            }
        }
        return scalePlaces[0] + "." + dec;
    }


}
