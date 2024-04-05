
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class googlePlayService : MonoBehaviour
{
    int score;
    int distance;
    // Start is called before the first frame update
    void Start()
    {
        
        try
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => { });
            Debug.Log("Activated");
    
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            Debug.Log("Failed");
        }
    }
    public void AddScoreToLeaderBoard(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkIl9_DjqUWEAIQAQ", success => { }); 
        }
    }
    
    public void ShowLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }
}
