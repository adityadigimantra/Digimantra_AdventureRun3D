using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersionControl : MonoBehaviour
{
    public GameObject UpdatePanel;
    public GameObject LoadingText;
    public string LatestVersion, CurrentVersion;

    void Start()
    {
        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {

        WWW BallGame = new WWW("https://raw.githubusercontent.com/adityadigimantra/AdventureRun3D_VersionControl/main/Version%20Control");
        yield return new WaitUntil(() => BallGame.text != "");
        string lm=BallGame.text;
        LatestVersion = lm.Substring(0, lm.Length - 1);

        if(LatestVersion!=CurrentVersion)
        {
            UpdatePanel.SetActive(true);
            LoadingText.SetActive(false);
        }
        else
        {
            LoadingText.SetActive(true);
            StartCoroutine(loadAfterFewSeconds(2f));
        }
        yield break;
       
    }

    IEnumerator loadAfterFewSeconds(float waitBySeconds)
    {
        yield return new WaitForSeconds(waitBySeconds);
        SceneManager.LoadScene(2);
    }
    public void ContinueButtonFunction()
    {
        UpdatePanel.SetActive(false);
        LoadingText.SetActive(true);
        SceneManager.LoadScene(2);
    }
    public void UpdateGameFunction()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.digimantralabs.adventurerun3d");
    }

}
