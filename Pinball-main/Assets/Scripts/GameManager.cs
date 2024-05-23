using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour   
{
    [SerializeField]
    GameObject ball,startButton,quitButton;

    [SerializeField]
    GameObject touchControls;

    [SerializeField]
    GameObject startGame, endGame;

    [SerializeField]
    StartSequence startSequence;

    [SerializeField]
    List<TMP_Text> bestScores;
    [SerializeField]
    List<TMP_Text> scores;

    int score, highScore, prevHighScore;

    [SerializeField]
    Rigidbody2D left, left2, right;

    [SerializeField]
    Transform startPos;

    GameObject activeBall;

    [SerializeField]
    GameObject life1, life2, life3;

    [SerializeField]
    Transform parent;

    public GoogleConnect google;

    public int multiplier;

    private int torqueMultiplier = 15;

    bool gameStarted, gameOver, canPlay;
    public bool starting;

    public static GameManager instance;
    [SerializeField]
    public HighScoreManager hsManager;

    [SerializeField]
    private ScaleScreen scaleScreen;
    bool settingScreenScale = false;

    [SerializeField]
    Username setUsername;
    public static bool usernameSet = false;
    public static bool usernameDirty = false;

    [SerializeField]
    AudioSource flipperLeftUp, flipperLeftDown, flipperRightUp, flipperRightDown;
    [SerializeField]
    AudioClip flipperUp, flipperDown;

    public bool soundOn = true;
    public bool optionScreenScale = false;
    public bool optionUsername = false;
    public bool optionSound = false;
    public bool startScreen = false;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        score = 0;
        multiplier = 1;
        highScore = PlayerPrefs.GetInt("Best");
        prevHighScore = highScore;
        foreach (TMP_Text text in bestScores)
        {
            text.text = highScore.ToString();
        }
        hsManager = GetComponent<HighScoreManager>();
        AudioSource[] leftFlipperSFX = left.GetComponents<AudioSource>();
        flipperLeftUp = leftFlipperSFX[0];
        flipperLeftDown = leftFlipperSFX[1];
        AudioSource[] rightFlipperSFX = right.GetComponents<AudioSource>();
        flipperRightUp = rightFlipperSFX[0];
        flipperRightDown = rightFlipperSFX[1];

        gameStarted = false;
        canPlay = false;

        google = new GoogleConnect();
    }

    private void FixedUpdate()
    {
        if (!optionScreenScale)
        {
            CheckGameScale();
        }
        else if (!optionUsername)
        {
            if (!settingScreenScale)
            {
                setUsername.Check();
            }
        }
        else if (!optionSound)
        {
            CheckSound();
        }
        if (settingScreenScale)
        {
            if (scaleScreen.screenScaleSet)
            {
                ActivateStartGame(true);
                settingScreenScale = false;
                if (!usernameSet)
                {
                    setUsername.Check();
                }
                else
                {
                    ActivateStartGame(true);
                }
            }
        }
        else if (setUsername.changing)
        {
            if (usernameSet)
            {
                ActivateStartGame(true);
            }
        }
        if (!startSequence.DoneStartup && !gameOver)
        {
            if (!startGame.activeSelf)
            {
                if (!touchControls.activeInHierarchy)
                {
                    touchControls.SetActive(true);
                }
            }
            if (InputBridge.Instance.InputReturn() && gameStarted)
            {
                startSequence.DrawSpring();
                starting = true;
            }
            else if (startSequence.DrawingSpring)
            {
                if (activeBall && activeBall.transform.position.y < -5.5f)
                {
                    startSequence.LaunchBall(activeBall.GetComponent<Rigidbody2D>());
                }
                else
                {
                    startSequence.ResetSpring();
                }
            }
        }
        else
        {
            starting = false;
            canPlay = true;
        }
        if (canPlay)
        {
            if (InputBridge.Instance.InputLeft())
            {
                left.AddTorque(25f * torqueMultiplier);
                left2.AddTorque(25f * torqueMultiplier);
            }
            else
            {
                left.AddTorque(-20f * torqueMultiplier);
                left2.AddTorque(-20f * torqueMultiplier);
            }
            if (InputBridge.Instance.InputLeftDown())
            {
                flipperLeftUp.PlayOneShot(flipperUp);
            }
            if (InputBridge.Instance.InputLeftUp())
            {
                flipperLeftDown.PlayOneShot(flipperDown);
            }
            if (InputBridge.Instance.InputRight())
            {
                right.AddTorque(-25f * torqueMultiplier);
            }
            else
            {
                right.AddTorque(20f * torqueMultiplier);
            }
            if (InputBridge.Instance.InputRightDown())
            {
                flipperRightUp.PlayOneShot(flipperUp);
            }
            if (InputBridge.Instance.InputRightUp())
            {
                flipperRightDown.PlayOneShot(flipperDown);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                TableBump();
            }
        }
        else
        {
            if (gameOver)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    GameRestart();
                    gameOver = false;
                }
                touchControls.SetActive(false);
            }
            else
            {
                if (Input.GetKey(KeyCode.Space) && !gameStarted)
                {
                    GameStart();
                }
            }
        }
    }

    public void UpdateScore(int point, int mullIncrease)
    {
        multiplier += mullIncrease;
        score += point * multiplier;
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (score > highScore)
        {
            highScore = score;
            foreach (TMP_Text text in bestScores)
            {
                text.text = highScore.ToString();
            }
        }
        foreach (TMP_Text text in scores)
        {
            text.text = score.ToString();
        }
    }

    private void TableBump()
    {
        if (activeBall)
        {
            Rigidbody2D rb = activeBall.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * 10);
        }
    }

    public void GameEnd()
    {
        endGame.SetActive(true);
        if(highScore > prevHighScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Best", highScore);
            UpdateScore();
            google.SendHighScore(setUsername.username, highScore);
        }
        gameOver = true;
        gameStarted = false;
        canPlay = false;
    }

    public void BallLoss()
    {
        canPlay = false;
        Destroy(activeBall);
        startSequence.SetupStartup();
        if (!life3.activeSelf)
        {
            GameEnd();
        }
        else
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        ActivateStartGame(false);
        activeBall = Instantiate(ball, startPos.position, Quaternion.identity, parent);
        activeBall.transform.localScale = Vector3.one * .75f;
        if (life1.activeSelf)
        {
            life1.SetActive(false);
        }
        else if (life2.activeSelf)
        {
            life2.SetActive(false);
        }
        else if (life3.activeSelf)
        {
            life3.SetActive(false);
        }
        gameStarted = true;
    }

    public void ActivateStartGame(bool activate)
    {
        //if (activate)
        //{
        //    hsManager.WriteHighScoreTable();
        //}
        startGame.SetActive(activate);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameRestart()
    {
        endGame.SetActive(false);
        score = 0;
        UpdateScore();
        gameOver = false;
        canPlay = false;
        gameStarted = false;
        GameStart();
        life2.SetActive(true);
        life3.SetActive(true);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void CheckOptions()
    {
        CheckGameScale();
        if (!settingScreenScale)
        {
            setUsername.Check();
        }
        if (PlayerPrefs.HasKey("soundon"))
        {
            int sound = PlayerPrefs.GetInt("soundon");
            if (sound == 0)
            {
                soundOn = false;
            }
            else
            {
                soundOn = true;
            }
            optionSound = true;
        }
        else
        {
            PlayerPrefs.SetInt("soundon", 1);
            optionSound = true;
        }
    }

    private void CheckSound()
    {
        if (PlayerPrefs.HasKey("soundon"))
        {
            int sound = PlayerPrefs.GetInt("soundon");
            if (sound == 0)
            {
                soundOn = false;
            }
            else
            {
                soundOn = true;
            }
            optionSound = true;
        }
        else
        {
            PlayerPrefs.SetInt("soundon", 1);
            optionSound = true;
        }
    }

    private void CheckGameScale()
    {
        if (PlayerPrefs.HasKey("gameScale"))
        {
            float gameScale = PlayerPrefs.GetFloat("gameScale");
            scaleScreen.SetScreenScale(gameScale);
            optionScreenScale = true;
        }
        else
        {
            ResetGameScaleStart();
        }
    }

    public void ResetGameScaleStart()
    {
        scaleScreen.screenScaleSet = false;
        ActivateStartGame(false);
        settingScreenScale = true;
        scaleScreen.ShowScaleScreen();
    }

    public void FlipperLeftUp()
    {
        flipperLeftUp.PlayOneShot(flipperUp);
    }

    public void FlipperLeftDown()
    {
        flipperLeftDown.PlayOneShot(flipperDown);
    }

    public void FlipperRightUp()
    {
        flipperRightUp.PlayOneShot(flipperUp);
    }

    public void FlipperRightDown()
    {
        flipperRightDown.PlayOneShot(flipperDown);
    }
}
