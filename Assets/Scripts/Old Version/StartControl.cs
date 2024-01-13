using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Switch to the game scene
    public void StartGameHitter()
    {
        SceneManager.LoadSceneAsync("MainGameHitterRed");
    }

    public void StartGamePitcher()
    {
        SceneManager.LoadSceneAsync("MainGamePitcherBlue");
    }
}
