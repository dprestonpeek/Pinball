using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using static HighScore;

public class GoogleConnect : MonoBehaviour
{
    string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdqEP2h6aFEaj4vfD5VHs4l6M7SrULrDNHM3RCljVAdJFoJmw/formResponse";

    public bool fetching = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendHighScore(string name, int score)
    {
        GameManager.instance.StartCoroutine(PostScore(name, score));
    }

    IEnumerator PostScore(string name, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1701684002", name);
        form.AddField("entry.1156348581", score.ToString());
        UnityWebRequest www = UnityWebRequest.Post(formUrl, form);

        yield return www.SendWebRequest();
    }

    public void GetHighScores()
    {
        fetching = true;
        GameManager.instance.StartCoroutine(ObtainScores());
    }

    IEnumerator ObtainScores()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/e/2PACX-1vThPk370UmNR8Yl-WcCNFucgUh9QjF96DuwiAha79Nrn9GJSywzqKtpkZYohAH6kwHJJYH6oDmIXadQ/pubhtml?gid=1025033323&single=true");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            string scoresText = "";
            string html = www.downloadHandler.text;

            string pattern = "(?<=[<][t][d][ ][c][l][a][s][s][=][\"][s][\\d][\"][ ][d][i][r][=][\"][l][t][r][\"][>])(.*?)(?=[<][/][t][d][>])";
            Regex reg = new Regex(pattern);

            bool timestamp = false;
            bool name = false;
            bool score = false;
            bool init = false;
            HighScoreInfo hs = new HighScoreInfo();

            int i = 0;

            Match match = reg.Match(html);
            Match nextMatch = match;
            while (nextMatch != null)
            {
                if (nextMatch.Groups.Count > 0)
                {
                    string theMatch = nextMatch.Groups[0].Value;
                    if (theMatch == "" && i < 10)
                    {
                        i++;
                        continue;
                    }
                    else if (i >= 10)
                    {
                        break;
                    }
                    if (name)
                    {
                        hs.name = theMatch;
                        name = false;
                        score = true;
                    }
                    else if (timestamp)
                    {
                        hs.timestamp = theMatch;
                        timestamp = false;
                        name = true;
                    }
                    else if (score)
                    {
                        hs.score = int.Parse(theMatch);
                        score = false;
                        if (!GameManager.instance.hsManager.highScoresInfo.Contains(hs))
                        {
                            GameManager.instance.hsManager.AddToHighScoreList(hs);
                        }
                        hs = new HighScoreInfo();
                        timestamp = true;
                    }
                    if (!init && theMatch == "Score")
                    {
                        timestamp = true;
                        init = true;
                    }
                    nextMatch = nextMatch.NextMatch();
                }
                else
                {
                    break;
                }
            }
        }
        fetching = false;
    }
}
