using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HighScore;

public class HighScoreManager : MonoBehaviour
{
    List<GameObject> theHighScores;
    public List<HighScoreInfo> highScoresInfo;
    Dictionary<string, int> highScoresPerUsername;

    [SerializeField]
    GameObject highScorePrefab;
    [SerializeField]
    Transform HighScoresParent;

    bool waitToFetch = false;
    bool waitToWrite = false;

    // Start is called before the first frame update
    void Start()
    {
        theHighScores = new List<GameObject>();
        highScoresInfo = new List<HighScoreInfo>();
        highScoresPerUsername = new Dictionary<string, int>();
        WriteHighScoreTable();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitToFetch)
        {
            if (highScoresInfo != null && theHighScores != null)
            {
                waitToFetch = false;
                WriteHighScoreTable();
            }
        }
        if (waitToWrite)
        {
            if (!GameManager.instance.google.fetching)
            {
                waitToWrite = false;
                SortHighScores();
                PrintHighScores();
            }
        }
    }

    public void WriteHighScoreTable()
    {
        if (highScoresInfo != null && theHighScores != null)
        {
            ClearHighScores();
            GameManager.instance.google.GetHighScores();
            if (GameManager.instance.google.fetching)
            {
                waitToWrite = true;
            }
        }
        else
        {
            waitToFetch = true;
        }
    }

    public void AddToHighScoreList(HighScoreInfo hs)
    {
        highScoresInfo.Add(hs);
    }

    public void SortHighScores()
    {
        if (highScoresInfo != null && highScoresInfo.Count > 0)
        {
            for (int i = 0; i < highScoresInfo.Count; i++)
            {
                int lowest = i;

                for (int j = i + 1; j < highScoresInfo.Count; j++)
                {
                    if (highScoresInfo[j].score < highScoresInfo[lowest].score)
                    {
                        lowest = j;
                    }
                }

                HighScoreInfo temp = highScoresInfo[lowest];
                highScoresInfo[lowest] = highScoresInfo[i];
                highScoresInfo[i] = temp;
            }
        }
        //highScoresInfo.Sort();
    }

    public void PrintHighScores()
    {
        foreach (HighScoreInfo hs in highScoresInfo)
        {
            if (highScoresPerUsername.ContainsKey(hs.name))
            {
                if (highScoresPerUsername[hs.name] < hs.score)
                {
                    highScoresPerUsername[hs.name] = hs.score;
                }
            }
            else
            {
                highScoresPerUsername.Add(hs.name, hs.score);
            }
        }
        //Dictionary<int, HighScoreInfo> leaderboard = new Dictionary<int, HighScoreInfo>();
        int maxPlacements = highScoresInfo.Count + 1;
        if (highScoresInfo.Count > 10)
        {
            maxPlacements = 11;
        }
        //1 rep for each placement
        for (int i = 1; i < maxPlacements; i++)
        {
            int highest = 0;
            KeyValuePair<string, int> highestInfo = new KeyValuePair<string, int>();
            foreach (KeyValuePair<string, int> hsInfo in highScoresPerUsername)
            {
                if (highest == 0)
                {
                    highest = hsInfo.Value;
                    highestInfo = hsInfo;
                }
                if (hsInfo.Value > highest)
                {
                    highest = hsInfo.Value;
                    highestInfo = hsInfo;
                }
            }
            if (highScoresPerUsername.Count > 0)
            {
                highScoresPerUsername.Remove(highestInfo.Key);
                AddToLeaderboard(highestInfo.Key, highestInfo.Value, i);
            }
        }
    }

    private void AddToLeaderboard(string name, int score, int placement)
    {
        GameObject theHS = Instantiate(highScorePrefab, Vector3.down * (placement - 4) * .5f, Quaternion.identity, HighScoresParent);
        HighScore HSobj = theHS.GetComponent<HighScore>();
        HSobj.hsInfo = new HighScoreInfo();
        HSobj.hsInfo.name = name;
        HSobj.hsInfo.score = score;
        HSobj.hsInfo.placement = placement.ToString();
        theHighScores.Add(theHS);
    }

    public void ClearHighScores()
    {
        if (theHighScores != null)
        {
            int i = 0;
            while (theHighScores.Count > 0)
            {
                theHighScores.RemoveAt(0);
                i++;
                if (i > 50)
                {
                    break;
                }
            }
        }
    }
}
