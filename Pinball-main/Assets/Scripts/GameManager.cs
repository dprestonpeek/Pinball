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
    Rigidbody2D left, right;

    [SerializeField]
    Transform startPos;

    GameObject activeBall;

    [SerializeField]
    GameObject life1, life2, life3;

    public int multiplier;

    private int torqueMultiplier = 15;

    bool gameStarted, gameOver, starting, canPlay;

    public static GameManager instance;

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

        gameStarted = false;
        canPlay = false;
    }

    private void FixedUpdate()
    {
        if (!startSequence.DoneStartup)
        {
            if (Input.GetKey(KeyCode.Return) && gameStarted)
            {
                startSequence.DrawSpring();
                starting = true;
            }
            else if (startSequence.DrawingSpring)
            {
                startSequence.LaunchBall(activeBall.GetComponent<Rigidbody2D>());
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
            }
            else
            {
                left.AddTorque(-20f * torqueMultiplier);
            }
            if (Input.GetKey(KeyCode.P))
            {
                right.AddTorque(-25f * torqueMultiplier);
            }
            else
            {
                right.AddTorque(20f * torqueMultiplier);
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

    public void GameEnd()
    {
        endGame.SetActive(true);
        if(highScore > prevHighScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Best", highScore);

            foreach (TMP_Text text in bestScores)
            {
                text.text = highScore.ToString();
            }
        }
        gameOver = true;
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
        activeBall = Instantiate(ball, startPos.position, Quaternion.identity);
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
        life1.SetActive(true);
        life2.SetActive(true);
        life3.SetActive(true);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
