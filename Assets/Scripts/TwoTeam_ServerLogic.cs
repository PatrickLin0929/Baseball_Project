using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TwoTeam_ServerLogic : MonoBehaviour
{

    int strikes, balls, outs;
    int scoreA, scoreB;
    List<int> inningScoresA = new List<int> {0, 0, 0, 0};
    List<int> inningScoresB = new List<int> {0, 0, 0, 0};
    bool onFirst, onSecond, onThird;
    int inning;
    string teamAtBat;
    bool pitchThrown;
    bool gameEnded;
    string hiOutcome;
    string prompt, onBaseInfo, outsInfo, scoreInfo, inningInfo, finalScore;
    string pitchType, hitType;

    public AudioClip hitSoundClip, outSoundClip;

    public Material pitcherIconBlue, pitcherIconRed, hitterIconBlue, hitterIconRed, runnerIconBlue, runnerIconRed, catcherIconBlue, catcherIconRed;

    public Image imageToAnimate;
    float frameRate = 5.0f;    // Frames per second.
    int currentFrame = 0;     // Index of the current frame.
    // float timer = 0.0f;       // Timer to control frame rate.
    public List<Sprite> pitcherBlueAnimation, pitcherRedAnimation, hitterBlueAnimation, hitterRedAnimation, winBlueAnimation, winRedAnimation;
    List<Sprite> frames;


    List<string> choices = new List<string> { "Single", "Double", "Triple", "HomeRun", "Out" };
    // Probabilities for Team A
    List<double> probabilitiesTeamA = new List<double> { 0.32, 0.13, 0.05, 0.05, 0.45 };
    // Probabilities for Team B
    List<double> probabilitiesTeamB = new List<double> { 0.28, 0.15, 0.07, 0.03, 0.47 };


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
        GameObject.Find("PromptText").GetComponent<TextMeshProUGUI>().text = prompt;
        GameObject.Find("OnBaseInfoText").GetComponent<TextMeshProUGUI>().text = onBaseInfo;
        GameObject.Find("OutsInfoText").GetComponent<TextMeshProUGUI>().text = outsInfo;
        GameObject.Find("ScoreInfoText").GetComponent<TextMeshProUGUI>().text = scoreInfo;
        GameObject.Find("InningInfoText").GetComponent<TextMeshProUGUI>().text = inningInfo;
        GameObject.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = finalScore;
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

        pitchThrown = false;
        gameEnded = false;

        hiOutcome = "";

        prompt = "";
        onBaseInfo = "";
        outsInfo = "";
        scoreInfo = "";
        inningInfo = "";
        finalScore= "";

        pitchType = GameObject.Find("PitchValueLabel").GetComponent<TextMeshProUGUI>().text;
        hitType = GameObject.Find("HitValueLabel").GetComponent<TextMeshProUGUI>().text;
    }

    public void PitchButtonPressed()
    {
        List<Sprite> pitcherAnimation = (teamAtBat == "A") ? pitcherBlueAnimation : pitcherRedAnimation;
        StartImageAnimation(pitcherAnimation);
        pitchThrown = true;
        pitchType = GameObject.Find("PitchValueLabel").GetComponent<TextMeshProUGUI>().text;
        prompt = "The pitch was thrown. It's time to decide: Hit or Wait?";
    }

    public void HitButtonPressed()
    {
        pitchThrown = false;
        hitType = GameObject.Find("HitValueLabel").GetComponent<TextMeshProUGUI>().text;
        if(hitType == "Hit") {
            List<Sprite> hitterAnimation = (teamAtBat == "A") ? hitterRedAnimation : hitterBlueAnimation;
            StartImageAnimation(hitterAnimation);
        }
        List<double> probabilities = (teamAtBat == "A") ? probabilitiesTeamA : probabilitiesTeamB;
        hiOutcome = SelectChoiceWithProbability(choices, probabilities);
        if(hitType == "Hit") {
            HitOutcomeProcessing();
        } else {
            WaitOutcomeProcessing();
        }
    }

    void HitOutcomeProcessing()
    {
        // Reset the strike and ball count after each hit or wait
        strikes = 0;
        balls = 0;

        if(hiOutcome == "Out") {
            outs = outs + 1;
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(outSoundClip);
        } else {
            if (hiOutcome == "Single") {
                MoveRunners(1);
            } else if (hiOutcome == "Double") {
                MoveRunners(2);
            } else if (hiOutcome == "Triple") {
                MoveRunners(3);
            } else if (hiOutcome == "HomeRun") {
                MoveRunners(4);
            }
        }

        // Check if the inning should end
        if(outs >= 3) {
            EndInning();
        } else {
            UpdateGameStatus();
        }
        
        // Update the text prompt
        prompt = "The batter hit a " + hiOutcome;
    }

    void WaitOutcomeProcessing() 
    {
        if(pitchType == "Fastball") {
            balls = Mathf.Min(balls + 1, 4);
        } else {
            strikes = Mathf.Min(strikes + 1, 3);
        }
        
        if(balls == 4) {
            MoveRunners(1);
            balls = 0;  // Reset balls count after a walk
        } else if(strikes == 3) {
            outs = outs + 1;
            strikes = 0;  // Reset strikes count after a strikeout
            GameObject.Find("Audio Source").GetComponent<AudioSource>().Stop();
            GameObject.Find("Audio Source").GetComponent<AudioSource>().PlayOneShot(outSoundClip);
        }
        
        // Check if the inning should end
        if(outs >= 3) {
            EndInning();
        } else {
            UpdateGameStatus();
        }
    
        // Update the text prompt
        prompt = "The batter decided to wait. Balls: " + balls + " Strikes: " + strikes;
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
    }

    void UpdateGameStatus() {
        onBaseInfo = "Runners on base - First: ";
        if (onFirst) {
            onBaseInfo = onBaseInfo + "Yes";
        } else {
            onBaseInfo = onBaseInfo + "No";
        }
        onBaseInfo = onBaseInfo + " Second: ";
        if (onSecond) {
            onBaseInfo = onBaseInfo + "Yes";
        } else {
            onBaseInfo = onBaseInfo + "No";
        }
        onBaseInfo = onBaseInfo + " Third: ";
        if (onThird) {
            onBaseInfo = onBaseInfo + "Yes";
        } else {
            onBaseInfo = onBaseInfo + "No";
        }

        outsInfo = "Strikes: " + strikes + " Balls: " + balls + " Outs: " + outs;
        
        scoreInfo = "Score for Team A: " + scoreA + " Score for Team B: " + scoreB;
        
        inningInfo = "Inning: " + inning + " Team at bat: " + teamAtBat;
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
        if (inning > 3) {
            gameEnded = true;
            prompt = "Game over!";
            finalScore = "Final Scores:\n" + "Team A: " + inningScoresA[0] + " (Inning 1: " + inningScoresA[1] + ", Inning 2: " + inningScoresA[2] + ")\n"
                + "Team B: " + inningScoresB[0] + " (Inning 1: " + inningScoresB[1] + ", Inning 2: " + inningScoresB[2] + ")";
            List<Sprite> winAnimation = (inningScoresA[0] > inningScoresB[0]) ? winRedAnimation : winBlueAnimation;
            StartImageAnimation(winAnimation);
        } else {
            prompt  = "End of the inning. Switch sides!";
        }
    }
}
