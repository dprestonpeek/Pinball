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
    public float gameScale = 1.25f;
    public bool screenScaleSet = false;
    private string gameScaleString = "1.25";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScaleScreen()
    {
        scaleScreen.SetActive(true);
    }

    public void SetScreenScale(float newScale)
    {
        parent.transform.localScale = new Vector3(newScale, newScale, newScale);
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
        gameScale = newScale.x;
        scale.text = GetNewGameScaleValue(gameScale);
    }

    public void ScaleGameDown()
    {
        if (gameScale > .3f)
        {
            Vector3 newScale = parent.transform.localScale;
            newScale.x -= .05f;
            newScale.y -= .05f;
            newScale.z -= .05f;
            parent.transform.localScale = newScale;
            gameScale = newScale.x;
            scale.text = GetNewGameScaleValue(gameScale);
        }
    }

    public void Accept()
    {
        PlayerPrefs.SetFloat("gameScale", gameScale);
        scaleScreen.SetActive(false);
        screenScaleSet = true;
    }

    private string GetNewGameScaleValue(float gameScale)
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
