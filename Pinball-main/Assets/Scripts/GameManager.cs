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

    public int multiplier;

    private int torqueMultiplier = 15;

    bool gameStarted, gameOver, starting, canPlay;

    public static GameManager instance;

    [SerializeField]
    private ScaleScreen scaleScreen;
    bool settingScreenScale = false;

    [SerializeField]
    AudioSource flipperLeftUp, flipperLeftDown, flipperRightUp, flipperRightDown;
    [SerializeField]
    AudioClip flipperUp, flipperDown;

    private void Awake()
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
        AudioSource[] leftFlipperSFX = left.GetComponents<AudioSource>();
        flipperLeftUp = leftFlipperSFX[0];
        flipperLeftDown = leftFlipperSFX[1];
        AudioSource[] rightFlipperSFX = right.GetComponents<AudioSource>();
        flipperRightUp = rightFlipperSFX[0];
        flipperRightDown = rightFlipperSFX[1];

        gameStarted = false;
        canPlay = false;

        CheckGameScale();
    }

    private void FixedUpdate()
    {
        if (settingScreenScale)
        {
            if (scaleScreen.screenScaleSet)
            {
                startGame.SetActive(true);
                settingScreenScale = false;
            }
        }
        if (!startSequence.DoneStartup && !gameOver)
        {
            if (Input.GetKey(KeyCode.Return) && gameStarted)
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
            if (Input.GetKey(KeyCode.Q))
            {
                left.AddTorque(25f * torqueMultiplier);
                left2.AddTorque(25f * torqueMultiplier);
            }
            else
            {
                left.AddTorque(-20f * torqueMultiplier);
                left2.AddTorque(-20f * torqueMultiplier);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                flipperLeftUp.PlayOneShot(flipperUp);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                flipperLeftDown.PlayOneShot(flipperDown);
            }
            if (Input.GetKey(KeyCode.P))
            {
                right.AddTorque(-25f * torqueMultiplier);
            }
            else
            {
                right.AddTorque(20f * torqueMultiplier);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                flipperRightUp.PlayOneShot(flipperUp);
            }
            if (Input.GetKeyUp(KeyCode.P))
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
        Rigidbody2D rb = activeBall.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.down * 10);
    }

    public void GameEnd()
    {
        endGame.SetActive(true);
        if(highScore > prevHighScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Best", highScore);

            UpdateScore();
        }
        gameOver = true;
        gameStarted = false;
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
        startGame.SetActive(false);
        activeBall = Instantiate(ball, startPos.position, Quaternion.identity, parent);
        activeBall.transform.localScale = Vector3.one * (scaleScreen.gameScale - .25f);
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

    private void CheckGameScale()
    {
        if (PlayerPrefs.HasKey("gameScale"))
        {
            float gameScale = PlayerPrefs.GetFloat("gameScale");
            scaleScreen.SetScreenScale(gameScale);
        }
        else
        {
            ResetGameScale();
        }
    }

    public void ResetGameScale()
    {
        scaleScreen.screenScaleSet = false;
        startGame.SetActive(false);
        settingScreenScale = true;
        scaleScreen.ShowScaleScreen();
    }
}
