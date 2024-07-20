using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TwoTeamSceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonPressed()
    {
        SceneManager.LoadSceneAsync("TwoTeams_PlayerSelect");
    }

    public void BackButtonPressed()
    {
        SceneManager.LoadSceneAsync("TwoTeamsOpening");
    }

    public void TryAgainButtonPressed()
    {
        SceneManager.LoadSceneAsync("TwoTeams");
    }

}
