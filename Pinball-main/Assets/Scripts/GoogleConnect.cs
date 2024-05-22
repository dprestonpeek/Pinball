using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleConnect : MonoBehaviour
{
    string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdqEP2h6aFEaj4vfD5VHs4l6M7SrULrDNHM3RCljVAdJFoJmw/formResponse";
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
        StartCoroutine(Post(name, score));
    }

    IEnumerator Post(string name, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1701684002", name);
        form.AddField("entry.1156348581", score.ToString());
        UnityWebRequest www = UnityWebRequest.Post(formUrl, form);

        yield return www.SendWebRequest();
    }
}
