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
    List<TMP_Text> bestScores;
    [SerializeField]
    List<TMP_Text> scores;

    int score, highScore;

    [SerializeField]
    Rigidbody2D left, right;

    [SerializeField]
    Vector3 startPos;

    GameObject activeBall;

    [SerializeField]
    GameObject life1, life2, life3;

    public int multiplier;

    private int torqueMultiplier = 6;

    bool canPlay, gameOver;

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
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        foreach (TMP_Text text in bestScores)
        {
            text.text = highScore.ToString();
        }

        canPlay = false;
    }

    private void Update()
    {
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
                if (Input.GetKey(KeyCode.Space))
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
        if(score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;

            foreach (TMP_Text text in bestScores)
            {
                text.text = score.ToString();
            }
        }
        gameOver = true;
    }

    public void BallLoss()
    {
        canPlay = false;
        Destroy(activeBall);
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
            GameEnd();
        }
    }

    public void GameStart()
    {
        startGame.SetActive(false);
        activeBall = Instantiate(ball, new Vector2(startPos.x + Random.Range(-0.5f, 0.5f), startPos.y), Quaternion.identity);
        activeBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode2D.Impulse);
        canPlay = true;
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
        GameStart();
        gameOver = false;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
