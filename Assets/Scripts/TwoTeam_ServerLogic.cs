using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class TwoTeam_ServerLogic : MonoBehaviour
{
    int strikes, balls, outs;
    int scoreA, scoreB;
    List<int> inningScoresA = new List<int> {0, 0, 0, 0, 0, 0, 0};
    List<int> inningScoresB = new List<int> {0, 0, 0, 0, 0, 0, 0};
    int totalInning = 6;
    int teamMaxPlayer = 9;
    bool onFirst, onSecond, onThird;
    int inning;
    string teamAtBat;
    bool pitchThrown;
    bool gameEnded;
    string hiOutcome;
    string prompt, onBaseInfo, outsInfo, scoreInfo, inningInfo, finalScore;
    string playerStatusPrompt;
    string pitchType, hitType;

    public GameObject tryAgainButton, settingsButton;

    public AudioClip hitSoundClip, outSoundClip;
    public AudioClip firstBaseSoundClip, secondBaseSoundClip, thirdBaseSoundClip, gameOverSoundClip;
    public AudioClip homeRunAnnounceClip, firstBaseAnnounceClip, secondBaseAnnounceClip, thirdBaseAnnounceClip, missAnnounceClip, outAnnounceClip;
    public AudioClip strikeAnnounceClip, ballAnnounceClip;

    public Material pitcherIconBlue, pitcherIconRed, hitterIconBlue, hitterIconRed, runnerIconBlue, runnerIconRed, catcherIconBlue, catcherIconRed;

    public Image imageToAnimate, teamACurrentPlayerImage, teamBCurrentPlayerImage;
    public GameObject teamACurPlayerObj, teamBCurPlayerObj, teamACurPitcherObj, teamBCurPitcherObj;
    float frameRate = 5.0f;    // Frames per second.
    int currentFrame = 0;     // Index of the current frame.
    // float timer = 0.0f;       // Timer to control frame rate.
    public List<Sprite> pitcherBlueAnimation, pitcherRedAnimation, hitterBlueAnimation, hitterRedAnimation, winBlueAnimation, winRedAnimation, playerSpriteList;
    List<Sprite> frames;
    bool strikeOrBall; // 1 = Strike, 0 = Ball
    int teamAPlayerListIndex, teamBPlayerListIndex;
    string teamACurrentPlayer, teamBCurrentPlayer;
    static System.Random rnd = new System.Random();

    // "Miss", "Single", "Double", "Triple", "HomeRun", "Out" 

    List<string> choices = new List<string> { "揮棒落空", "一壘安打", "二壘安打", "三壘安打", "全壘打", "接殺" };
    // Probabilities for Team A
    //List<double> probabilitiesTeamA = new List<double> { 0.32, 0.13, 0.05, 0.05, 0.45 };
    /*List<double> probabilitiesTeamAFastball = new List<double> { 0, 0.32, 0.13, 0.05, 0.05, 0.45 };
    List<double> probabilitiesTeamACurveball = new List<double> { 0, 0.32, 0.13, 0.05, 0.05, 0.45 };
    List<double> probabilitiesTeamASlider = new List<double> { 0, 0.32, 0.13, 0.05, 0.05, 0.45 };*/
    
    List<double> probabilitiesTeamAFastball = new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 };
    List<double> probabilitiesTeamACurveball = new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 };
    List<double> probabilitiesTeamASlider = new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 };

    // Probabilities for Team B
    //List<double> probabilitiesTeamB = new List<double> { 0.28, 0.15, 0.07, 0.03, 0.47 };
    /*List<double> probabilitiesTeamBFastball = new List<double> { 0, 0.28, 0.15, 0.07, 0.03, 0.47 };
    List<double> probabilitiesTeamBCurveball = new List<double> { 0, 0.28, 0.15, 0.07, 0.03, 0.47 };
    List<double> probabilitiesTeamBSlider = new List<double> { 0, 0.28, 0.15, 0.07, 0.03, 0.47 };*/

    List<double> probabilitiesTeamBFastball = new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 };
    List<double> probabilitiesTeamBCurveball = new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 };
    List<double> probabilitiesTeamBSlider = new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 };

    List<float> probabilitiesWaitTeamA = new List<float> { 0.7f, 0.4f, 0.5f }; // Fastball, Curveball, Slider
    List<float> probabilitiesWaitTeamB = new List<float> { 0.7f, 0.37f, 0.5f }; // Fastball, Curveball, Slider

    List<string> pitcherTypeList = new List<string> { "快速球", "曲球", "滑球"};
    List<string> hitterTypeList = new List<string> { "揮棒", "等待"};

    public TMP_Dropdown pitchDropdown, hitDropdown;

    static string SelectChoiceWithProbability(List<string> choices, List<double> probabilities)
    {

        double randValue = new System.Random().NextDouble();
        double cumulativeProbability = 0.0;

        for (int i = 0; i < choices.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randValue < cumulativeProbability)
            {
                return choices[i];
            }
        }

        // If no choice is selected (shouldn't happen if probabilities sum to 1.0)
        return choices[choices.Count - 1];
    }



    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTextGUI();
        UpdateButtonGUI();
        UpdateBaseBallFieldObject();
    }

    void StartImageAnimation(List<Sprite> animation)
    {
        frames = animation;
        currentFrame = 0;
        // timer = 0.0f;
        imageToAnimate.sprite = frames[currentFrame];
        StartCoroutine(AnimateImage());
    }

    private IEnumerator AnimateImage()
    {
        while (currentFrame < frames.Count)
        {
            yield return new WaitForSeconds(1.0f / frameRate);
            currentFrame++;
            if (currentFrame < frames.Count)
            {
                imageToAnimate.sprite = frames[currentFrame];
            }
        }
    }


    void UpdateBaseBallFieldObject()
    {
        // Object Level 1 for location id (version 1)

        /* GameObject.Find("SphereFirst").GetComponent<Renderer>().enabled = onFirst;
        GameObject.Find("SphereSecond").GetComponent<Renderer>().enabled = onSecond;
        GameObject.Find("SphereThird").GetComponent<Renderer>().enabled = onThird;
        GameObject.Find("SphereFirst").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue;
        GameObject.Find("SphereSecond").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue;
        GameObject.Find("SphereThird").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue;
        
        GameObject.Find("CubePitcher").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.blue : Color.red;
        GameObject.Find("CubeHitter").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue; */

        GameObject.Find("SphereFirst").GetComponent<Renderer>().enabled = false;
        GameObject.Find("SphereSecond").GetComponent<Renderer>().enabled = false;
        GameObject.Find("SphereThird").GetComponent<Renderer>().enabled = false;

        // Object Level 2 for baseball player icon plane (version 2)

        GameObject.Find("PlaneFirst").GetComponent<Renderer>().enabled = onFirst;
        GameObject.Find("PlaneSecond").GetComponent<Renderer>().enabled = onSecond;
        GameObject.Find("PlaneThird").GetComponent<Renderer>().enabled = onThird;
        GameObject.Find("PlaneFirst").GetComponent<Renderer>().material = (teamAtBat == "A") ? runnerIconRed : runnerIconBlue;
        GameObject.Find("PlaneSecond").GetComponent<Renderer>().material = (teamAtBat == "A") ? runnerIconRed : runnerIconBlue;
        GameObject.Find("PlaneThird").GetComponent<Renderer>().material = (teamAtBat == "A") ? runnerIconRed : runnerIconBlue;

        GameObject.Find("PlanePitcher").GetComponent<Renderer>().material = (teamAtBat == "A") ? pitcherIconBlue : pitcherIconRed;
        GameObject.Find("PlaneHitter").GetComponent<Renderer>().material = (teamAtBat == "A") ? hitterIconRed : hitterIconBlue;

        GameObject.Find("PlaneCatcher").GetComponent<Renderer>().material = (teamAtBat == "A") ? catcherIconBlue : catcherIconRed;
    }

    void UpdateButtonGUI()
    {
        if(gameEnded) {
            GameObject.Find("PitchButton").GetComponent<Button>().interactable = false;
            GameObject.Find("HitButton").GetComponent<Button>().interactable = false;
            GameObject.Find("AutoplayButton").GetComponent<Button>().interactable = false;
            settingsButton.SetActive(false);
            tryAgainButton.SetActive(true);
        } else {
            if(pitchThrown) {
                GameObject.Find("PitchButton").GetComponent<Button>().interactable = false;
                GameObject.Find("HitButton").GetComponent<Button>().interactable = true;
            } else {
                GameObject.Find("PitchButton").GetComponent<Button>().interactable = true;
                GameObject.Find("HitButton").GetComponent<Button>().interactable = false;
            }
        }
    }

    void UpdateTextGUI()
    {
        GameObject.Find("TeamAScore").GetComponent<TextMeshProUGUI>().text = scoreA.ToString();
        GameObject.Find("TeamBScore").GetComponent<TextMeshProUGUI>().text = scoreB.ToString();
        teamACurPlayerObj.GetComponent<TextMeshProUGUI>().text = teamACurrentPlayer;
        teamBCurPlayerObj.GetComponent<TextMeshProUGUI>().text = teamBCurrentPlayer;
        GameObject.Find("PromptText").GetComponent<TextMeshProUGUI>().text = prompt;
        GameObject.Find("OnBaseInfoText").GetComponent<TextMeshProUGUI>().text = onBaseInfo;
        GameObject.Find("OutsInfoText").GetComponent<TextMeshProUGUI>().text = outsInfo;
        GameObject.Find("ScoreInfoText").GetComponent<TextMeshProUGUI>().text = scoreInfo;
        GameObject.Find("InningInfoText").GetComponent<TextMeshProUGUI>().text = inningInfo;
        GameObject.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = finalScore;
        //GameObject.Find("PlayerStatusText").GetComponent<TextMeshProUGUI>().text = playerStatusPrompt;
        if(inning <= totalInning) {
            GameObject.Find("TeamAScoreBoardTextInning"+inning).GetComponent<TextMeshProUGUI>().text = inningScoresA[inning].ToString();
            GameObject.Find("TeamBScoreBoardTextInning"+inning).GetComponent<TextMeshProUGUI>().text = inningScoresB[inning].ToString();
            GameObject.Find("ScoreBoardTextInning"+(inning-1)).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            GameObject.Find("TeamAScoreBoardTextInning"+(inning-1)).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            GameObject.Find("TeamBScoreBoardTextInning"+(inning-1)).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            GameObject.Find("ScoreBoardTextInning"+inning).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 0, 255);
            GameObject.Find("TeamAScoreBoardTextInning"+inning).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 0, 255);
            GameObject.Find("TeamBScoreBoardTextInning"+inning).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 0, 255);
        }
        GameObject.Find("TeamAScoreBoardTextInningTotal").GetComponent<TextMeshProUGUI>().text = inningScoresA[0].ToString();
        GameObject.Find("TeamBScoreBoardTextInningTotal").GetComponent<TextMeshProUGUI>().text = inningScoresB[0].ToString();
    }

    // Initialize the variables
    void InitializeValues()
    {
        strikes = 0;
        balls = 0;
        outs = 0;
        
        scoreA = 0;
        scoreB = 0;
        
        onFirst = false;
        onSecond = false;
        onThird = false;
        
        inning = 1;

        teamAtBat = "A";

        teamACurPlayerObj.SetActive(true);
        teamACurPitcherObj.SetActive(false);
        teamBCurPlayerObj.SetActive(false);
        teamBCurPitcherObj.SetActive(true);

        pitchThrown = false;
        gameEnded = false;

        hiOutcome = "";

        prompt = "";
        onBaseInfo = "";
        outsInfo = "";
        scoreInfo = "";
        inningInfo = "";
        finalScore= "";
        playerStatusPrompt = "";

        pitchType = GameObject.Find("PitchValueLabel").GetComponent<TextMeshProUGUI>().text;
        hitType = GameObject.Find("HitValueLabel").GetComponent<TextMeshProUGUI>().text;

        strikeOrBall = false;
        teamAPlayerListIndex = 0;
        teamBPlayerListIndex = 0;

        teamACurrentPlayer = TwoTeam_SharedData.playerList[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
        teamBCurrentPlayer = TwoTeam_SharedData.playerList[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];
        teamACurrentPlayerImage.sprite = playerSpriteList[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
        teamBCurrentPlayerImage.sprite = playerSpriteList[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];

    }

    public void PitchButtonPressed()
    {
        List<Sprite> pitcherAnimation = (teamAtBat == "A") ? pitcherBlueAnimation : pitcherRedAnimation;
        StartImageAnimation(pitcherAnimation);
        pitchThrown = true;
        pitchType = GameObject.Find("PitchValueLabel").GetComponent<TextMeshProUGUI>().text;
        prompt = "投手已經投球。請做出選擇：「揮棒」或「等待」？";
    }

    public void HitButtonPressed()
    {
        pitchThrown = false;
        hitType = GameObject.Find("HitValueLabel").GetComponent<TextMeshProUGUI>().text;
        if(hitType == hitterTypeList[0]) {
            List<Sprite> hitterAnimation = (teamAtBat == "A") ? hitterRedAnimation : hitterBlueAnimation;
            StartImageAnimation(hitterAnimation);
        }
        if(teamAtBat == "A") {
            if(pitchType == pitcherTypeList[0]) {
                hiOutcome = SelectChoiceWithProbability(choices, probabilitiesTeamAFastball);
            } else if(pitchType == pitcherTypeList[1]) {
                hiOutcome = SelectChoiceWithProbability(choices, probabilitiesTeamACurveball);
            } else if(pitchType == pitcherTypeList[2]) { 
                hiOutcome = SelectChoiceWithProbability(choices, probabilitiesTeamASlider);
            }
        } else {
            if(pitchType == pitcherTypeList[0]) {
                hiOutcome = SelectChoiceWithProbability(choices, probabilitiesTeamBFastball);
            } else if(pitchType == pitcherTypeList[1]) {
                hiOutcome = SelectChoiceWithProbability(choices, probabilitiesTeamBCurveball);
            } else if(pitchType == pitcherTypeList[2]) { 
                hiOutcome = SelectChoiceWithProbability(choices, probabilitiesTeamBSlider);
            }
        }
        if(hitType == hitterTypeList[0]) {
            HitOutcomeProcessing();
        } else {
            WaitOutcomeProcessing();
        }
    }

    void HitOutcomeProcessing()
    {

        if(hiOutcome == choices[5]/*Out*/) {
            outs = outs + 1;
            strikes = 0;
            balls = 0;
            changePlayer();
            GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(outAnnounceClip);
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(outSoundClip);
        } else {
            if (hiOutcome == choices[1]/*Single*/) {
                // Reset the strike and ball count after each hit or wait
                strikes = 0;
                balls = 0;
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(firstBaseAnnounceClip);
                GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(firstBaseSoundClip);
                MoveRunners(1);
            } else if (hiOutcome == choices[2]/*Double*/) {
                // Reset the strike and ball count after each hit or wait
                strikes = 0;
                balls = 0;
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(secondBaseAnnounceClip);
                GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(secondBaseSoundClip);
                MoveRunners(2);
            } else if (hiOutcome == choices[3]/*Triple*/) {
                // Reset the strike and ball count after each hit or wait
                strikes = 0;
                balls = 0;
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(thirdBaseAnnounceClip);
                GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(thirdBaseSoundClip);
                MoveRunners(3);
            } else if (hiOutcome == choices[4]/*HomeRun*/) {
                // Reset the strike and ball count after each hit or wait
                strikes = 0;
                balls = 0;
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(homeRunAnnounceClip);
                MoveRunners(4);
            } else if (hiOutcome == choices[0]/*Miss*/) {
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
                GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(missAnnounceClip);
                strikes = Mathf.Min(strikes + 1, 3);
                if(strikes == 3) {
                    outs = outs + 1;
                    strikes = 0;  // Reset strikes count after a strikeout
                    balls = 0;
                    changePlayer();
                    GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
                    GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(outSoundClip);
                }
            }
        }
        
        // Update the text prompt
        prompt = "打者擊出了一個「" + hiOutcome + "」。";

        // Check if the inning should end
        if(outs >= 3) {
            EndInning();
        } else {
            UpdateGameStatus();
        }
    }

    void WaitOutcomeProcessing() 
    {
        if(teamAtBat == "A") {
            // Adjust the strikes and balls based on pitch type for Team A
            if(pitchType == pitcherTypeList[0]) {
                strikeOrBall = (UnityEngine.Random.value < probabilitiesWaitTeamA[0]) ? true : false;
            } else if(pitchType == pitcherTypeList[1]) {
                strikeOrBall = (UnityEngine.Random.value < probabilitiesWaitTeamA[1]) ? true : false;
            } else if(pitchType == pitcherTypeList[2]) {
                strikeOrBall = (UnityEngine.Random.value < probabilitiesWaitTeamA[2]) ? true : false;
            }
        } else {
            // Adjust the strikes and balls based on pitch type for Team B
            if(pitchType == pitcherTypeList[0]) {
                strikeOrBall = (UnityEngine.Random.value < probabilitiesWaitTeamB[0]) ? true : false;
            } else if(pitchType == pitcherTypeList[1]) {
                strikeOrBall = (UnityEngine.Random.value < probabilitiesWaitTeamB[1]) ? true : false;
            } else if(pitchType == pitcherTypeList[2]) {
                strikeOrBall = (UnityEngine.Random.value < probabilitiesWaitTeamB[2]) ? true : false;
            }
        }

        if(strikeOrBall) {
            strikes = Mathf.Min(strikes + 1, 3);
            GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(strikeAnnounceClip);
        } else {
            balls = Mathf.Min(balls + 1, 4);
            GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source TTS").GetComponent<AudioSource>().PlayOneShot(ballAnnounceClip);
        }
        
        if(balls == 4) {
            MoveRunners(1);
            balls = 0;  // Reset balls count after a walk
        } else if(strikes == 3) {
            outs = outs + 1;
            strikes = 0;  // Reset strikes count after a strikeout
            changePlayer();
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(outSoundClip);
        }
    
        // Update the text prompt
        prompt = "打者決定等待不揮棒。好球數: " + strikes + " 壞球數: " + balls;

        // Check if the inning should end
        if(outs >= 3) {
            EndInning();
        } else {
            UpdateGameStatus();
        }
    }

    void MoveRunners(int bases)
    {
        // Score any runners that would be pushed in by the hit
        if (bases >= 3 && onFirst) { UpdateScore(); }
        if (bases >= 2 && onSecond) { UpdateScore(); }
        if (onThird) { UpdateScore(); }
        
        // Update base runners based on the hit
        if (bases == 4) {  // Home Run
            UpdateScore();  // Batter scores on a home run
            onFirst = false;
            onSecond = false;
            onThird = false;
        } else {
            if (bases == 3) {
                onThird = true;
                onSecond = false;
                onFirst = false;
            } else if (bases == 2) {
                onThird = onFirst;
                onSecond = true;
                onFirst = false;
            } else if (bases == 1) {
                onThird = onSecond;
                onSecond = onFirst;
                onFirst = true;
            }
        }
    }

    void UpdateScore()
    {
        GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
        GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(hitSoundClip);
        if (teamAtBat == "A") {
            scoreA = scoreA + 1;
            inningScoresA[inning] = inningScoresA[inning] + 1;
            inningScoresA[0] = inningScoresA[0] + 1;
        } else {
            scoreB = scoreB + 1;
            inningScoresB[inning] = inningScoresB[inning] + 1;
            inningScoresB[0] = inningScoresB[0] + 1;
        }
        // Clear the bases after scoring
        onThird = false;
        changePlayer();
    }

    void UpdateGameStatus() {
        onBaseInfo = "上壘情況 - \n一壘: ";
        if (onFirst) {
            onBaseInfo = onBaseInfo + "有";
        } else {
            onBaseInfo = onBaseInfo + "無";
        }
        onBaseInfo = onBaseInfo + " 二壘: ";
        if (onSecond) {
            onBaseInfo = onBaseInfo + "有";
        } else {
            onBaseInfo = onBaseInfo + "無";
        }
        onBaseInfo = onBaseInfo + " 三壘: ";
        if (onThird) {
            onBaseInfo = onBaseInfo + "有";
        } else {
            onBaseInfo = onBaseInfo + "無";
        }

        outsInfo = "好球數: " + strikes + " 壞球數: " + balls + " 出局人數: " + outs;
        
        scoreInfo = "紅隊得分: " + scoreA + " 藍隊得分: " + scoreB;
        string teamNameAtBat = (teamAtBat == "A") ? "紅隊" : "藍隊";
        inningInfo = "局數: " + inning + " 進攻方: " + teamNameAtBat;
        finalScore = "";
        //playerStatusPrompt = "Team A: Index - " + teamAPlayerListIndex + " ID -  " + TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex] + " Matrix - " + string.Join(", ", probabilitiesTeamAFastball) + " >< " + string.Join(", ", probabilitiesTeamACurveball) + " >< " + string.Join(", ", probabilitiesTeamASlider)
        //                    + "\n" + "Team B: Index - " + teamBPlayerListIndex + " ID -  " + TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex] + " Matrix - " + string.Join(", ", probabilitiesTeamBFastball) + " >< " + string.Join(", ", probabilitiesTeamBCurveball) + " >< " + string.Join(", ", probabilitiesTeamBSlider);
    }

    void EndInning()
    {
        outs = 0;
        balls = 0;
        strikes = 0;
        onFirst = false;
        onSecond = false;
        onThird = false;
        // Switch sides after half-inning
        teamAtBat = (teamAtBat == "A") ? "B" : "A";
        // Increment inning after bottom half
        if (teamAtBat == "A") { inning  = inning + 1; }
        // prompt = "End of the inning. Switch sides!";

        // Check if the game should end after the second inning
        // The game should also end when the changing side to team B is the last one and team B is ahead of team A
        if ((inning > totalInning) || ((inning == totalInning) && (teamAtBat == "B") && (inningScoresA[0] < inningScoresB[0]))) {
            gameEnded = true;
            UpdateButtonGUI();
            finalScore = "最終得分:\n" + "紅隊: " + inningScoresA[0] + " \n(第1局: " + inningScoresA[1] + ", 第2局: " + inningScoresA[2] + ", 第3局: " + inningScoresA[3] 
                                                                        + ", 第4局: " + inningScoresA[4] + ", 第5局: " + inningScoresA[5] + ", 第6局: " + inningScoresA[6] + ")\n"
                + "籃隊: " + inningScoresB[0] + " \n(第1局: " + inningScoresB[1] + ", 第2局: " + inningScoresB[2] + ", 第3局: " + inningScoresB[3] 
                + ", 第4局: " + inningScoresB[4] + ", 第5局: " + inningScoresB[5] + ", 第6局: " + inningScoresB[6] + ")";
            List<Sprite> winAnimation = (inningScoresA[0] > inningScoresB[0]) ? winRedAnimation : winBlueAnimation;
            StartImageAnimation(winAnimation);
            prompt = "比賽結束！";
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(gameOverSoundClip);
            if (((inning == totalInning) && (teamAtBat == "B") && (inningScoresA[0] < inningScoresB[0]))) {
                prompt = "比賽結束！上半局結果已定。";
            }
        } else {
            finalScore  = "這局結束，攻守交換。";
            if (teamAtBat == "A") {
                teamACurPlayerObj.SetActive(true);
                teamACurPitcherObj.SetActive(false);
                teamBCurPlayerObj.SetActive(false);
                teamBCurPitcherObj.SetActive(true);
            } else {
                teamACurPlayerObj.SetActive(false);
                teamACurPitcherObj.SetActive(true);
                teamBCurPlayerObj.SetActive(true);
                teamBCurPitcherObj.SetActive(false);
            }
        }
    }

    void changePlayer() {
        if(teamAtBat == "A") {
            teamAPlayerListIndex = (teamAPlayerListIndex + 1 ) % teamMaxPlayer;
            probabilitiesTeamAFastball = TwoTeam_SharedData.playerMatrix_Fastball[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
            probabilitiesTeamACurveball = TwoTeam_SharedData.playerMatrix_Curveball[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
            probabilitiesTeamASlider = TwoTeam_SharedData.playerMatrix_Slider[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
            teamACurrentPlayer = TwoTeam_SharedData.playerList[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
            teamACurrentPlayerImage.sprite = playerSpriteList[TwoTeam_SharedData.teamAPlayerOrderedList[teamAPlayerListIndex]];
        } else {
            teamBPlayerListIndex = (teamBPlayerListIndex + 1 ) % teamMaxPlayer;
            probabilitiesTeamBFastball = TwoTeam_SharedData.playerMatrix_Fastball[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];
            probabilitiesTeamBCurveball = TwoTeam_SharedData.playerMatrix_Curveball[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];
            probabilitiesTeamBSlider = TwoTeam_SharedData.playerMatrix_Slider[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];
            teamBCurrentPlayer = TwoTeam_SharedData.playerList[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];
            teamBCurrentPlayerImage.sprite = playerSpriteList[TwoTeam_SharedData.teamBPlayerOrderedList[teamBPlayerListIndex]];
        }
    }

    public void AutoCompletePressed() {
        if(pitchThrown) {
            int rnd_val = rnd.Next(hitterTypeList.Count);
            //Dropdown m_Dropdown = GameObject.Find("HitDropdown").GetComponent<Dropdown>();
            hitDropdown.value = rnd_val;
            HitButtonPressed();
        } else {
            int rnd_val = rnd.Next(pitcherTypeList.Count);
            //Dropdown m_Dropdown = GameObject.Find("PitchDropdown").GetComponent<Dropdown>();
            pitchDropdown.value = rnd_val;
            PitchButtonPressed();
        }
    }
}
