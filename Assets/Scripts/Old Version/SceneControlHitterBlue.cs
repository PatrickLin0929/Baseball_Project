using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneControlHitterBlue : MonoBehaviour
{
    bool actionMade = false;
    GameObject baseBall;
    //GameObject scoreText;
    Vector3 originalBaseBallPosition;
    int Score = 0;
    bool throwBall = false;
    int firstBase = 0;
    int secondBase = 0;
    int thirdBase = 0;
    int strikeCount = 0;
    int hitterOut = 0;
    int badBallCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        baseBall = GameObject.Find("Baseball");
        //scoreText = transform.Find("ScoreText").GetComponent<Text>();
        originalBaseBallPosition = baseBall.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        SceneManager.LoadSceneAsync("StartScreen");
    }

    public void HitBallPressed()
    {
        baseBall.transform.position = originalBaseBallPosition;
        actionMade = true;
        baseBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bool BallHitRandom  = (Random.value > 0.5f);
        throwBall = (Random.value > 0.5f);
        if(throwBall)
        {
            GameObject.Find("ThrowBallStatus").GetComponent<TextMeshProUGUI>().text = "Last Pitcher Status: Throw Ball";
        }
        else
        {
            GameObject.Find("ThrowBallStatus").GetComponent<TextMeshProUGUI>().text = "Last Pitcher Status: Don't Throw";
        }
        if(BallHitRandom && throwBall)
        {
            GameObject.Find("StatusText").GetComponent<TextMeshProUGUI>().text = "Cool! You hit the ball.";
            UpdateBaseWinBall();
            strikeCount = 0;
            badBallCount = 0;
        }
        else
        {
            GameObject.Find("StatusText").GetComponent<TextMeshProUGUI>().text = "NOT hit, try again.";
            strikeCount = strikeCount + 1;
            if(strikeCount > 3)
            {
                //UpdateBaseWinBall();
                strikeCount = 0;
                badBallCount = 0;
                hitterOut = hitterOut + 1;
            }
        }
    }

    public void DontHitBallPressed()
    {
        baseBall.transform.position = originalBaseBallPosition;
        actionMade = false;
        baseBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        badBallCount = badBallCount + 1;
        throwBall = (Random.value > 0.5f);
        if(throwBall)
        {
            GameObject.Find("ThrowBallStatus").GetComponent<TextMeshProUGUI>().text = "Last Pitcher Status: Throw Ball";
        }
        else
        {
            GameObject.Find("ThrowBallStatus").GetComponent<TextMeshProUGUI>().text = "Last Pitcher Status: Don't Throw";
        }
        
        if(throwBall)
        {
            GameObject.Find("StatusText").GetComponent<TextMeshProUGUI>().text = "NOT hit, try again.";
            strikeCount = strikeCount + 1;
            if(strikeCount > 3)
            {
                //UpdateBaseLoseBall();
                strikeCount = 0;
                badBallCount = 0;
                hitterOut = hitterOut + 1;
            }
        }
        else
        {
            GameObject.Find("StatusText").GetComponent<TextMeshProUGUI>().text = "The bad ball has thrown.";
            badBallCount = badBallCount + 1;
            if(badBallCount > 4)
            {
                UpdateBaseWinBall();
                strikeCount = 0;
                badBallCount = 0;
            }
        }
    }

    public void UpdateBaseWinBall()
    {
        if(firstBase == 0 && secondBase == 0 && thirdBase == 0)
        {
            firstBase = 1;
            secondBase = 0;
            thirdBase = 0;

        }
        else if(firstBase == 1 && secondBase == 0 && thirdBase == 0)
        {
            firstBase = 1;
            secondBase = 1;
            thirdBase = 0;

        }
        else if(firstBase == 0 && secondBase == 1 && thirdBase == 0)
        {
            firstBase = 1;
            secondBase = 0;
            thirdBase = 1;

        }
        else if(firstBase == 1 && secondBase == 1 && thirdBase == 0)
        {
            firstBase = 1;
            secondBase = 1;
            thirdBase = 1;

        }
        else if(firstBase == 0 && secondBase == 0 && thirdBase == 1)
        {
            firstBase = 1;
            secondBase = 0;
            thirdBase = 0;
            Score = Score + 1;
        }
        else if(firstBase == 1 && secondBase == 0 && thirdBase == 1)
        {
            firstBase = 1;
            secondBase = 1;
            thirdBase = 0;
            Score = Score + 1;
        }
        else if(firstBase == 0 && secondBase == 1 && thirdBase == 1)
        {
            firstBase = 1;
            secondBase = 0;
            thirdBase = 1;
            Score = Score + 1;
        }
        else if(firstBase == 1 && secondBase == 1 && thirdBase == 1)
        {
            firstBase = 1;
            secondBase = 1;
            thirdBase = 1;
            Score = Score + 1;
        }
    }

    public void UpdateBaseLoseBall()
    {
        if(firstBase == 0 && secondBase == 0 && thirdBase == 0)
        {
            firstBase = 0;
            secondBase = 0;
            thirdBase = 0;

        }
        else if(firstBase == 1 && secondBase == 0 && thirdBase == 0)
        {
            firstBase = 0;
            secondBase = 1;
            thirdBase = 0;

        }
        else if(firstBase == 0 && secondBase == 1 && thirdBase == 0)
        {
            firstBase = 0;
            secondBase = 0;
            thirdBase = 1;

        }
        else if(firstBase == 1 && secondBase == 1 && thirdBase == 0)
        {
            firstBase = 0;
            secondBase = 1;
            thirdBase = 1;

        }
        else if(firstBase == 0 && secondBase == 0 && thirdBase == 1)
        {
            firstBase = 0;
            secondBase = 0;
            thirdBase = 0;
            Score = Score + 1;
        }
        else if(firstBase == 1 && secondBase == 0 && thirdBase == 1)
        {
            firstBase = 0;
            secondBase = 1;
            thirdBase = 0;
            Score = Score + 1;
        }
        else if(firstBase == 0 && secondBase == 1 && thirdBase == 1)
        {
            firstBase = 0;
            secondBase = 0;
            thirdBase = 1;
            Score = Score + 1;
        }
        else if(firstBase == 1 && secondBase == 1 && thirdBase == 1)
        {
            firstBase = 0;
            secondBase = 1;
            thirdBase = 1;
            Score = Score + 1;
        }
    }

    void FixedUpdate()
    {
        if(actionMade)
        {
            baseBall.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.1f, -1.0f) * 20);
        }
        GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + Score;
        if(Score == 5)
        {
            SceneManager.LoadSceneAsync("WinEndScreen");
        }
        if(hitterOut == 3)
        {
            SceneManager.LoadSceneAsync("LoseEndScreen");
        }
        GameObject.Find("BaseText").GetComponent<TextMeshProUGUI>().text = "Base Status: " + firstBase + " " + secondBase + " " + thirdBase;
        
        GameObject.Find("StrikeText").GetComponent<TextMeshProUGUI>().text = "Strike: " + strikeCount;
        GameObject.Find("BadBallText").GetComponent<TextMeshProUGUI>().text = "Bad Ball: " + badBallCount;
        GameObject.Find("HitterOutText").GetComponent<TextMeshProUGUI>().text = "Hitter Out: " + hitterOut;
    }
}
