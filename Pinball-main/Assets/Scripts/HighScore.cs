using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    [SerializeField]
    TMP_Text placement;
    [SerializeField]
    TMP_Text username;
    [SerializeField]
    TMP_Text scoreText;

    public HighScoreInfo hsInfo;
    bool init = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            if (hsInfo != null && hsInfo.placement != null)
            {
                placement.text = hsInfo.placement;
                username.text = hsInfo.name;
                scoreText.text = hsInfo.score.ToString();
                init = true;
            }
        }
    }

    public class HighScoreInfo
    {
        public string timestamp;
        public string name;
        public int score;
        public string placement;
    }
}