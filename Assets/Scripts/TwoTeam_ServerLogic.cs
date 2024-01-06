using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TwoTeam_ServerLogic : MonoBehaviour
{

    int strikes, balls, outs;
    int scoreA, scoreB;
    bool onFirst, onSecond, onThird;
    int inning;
    string teamAtBat;
    bool pitchThrown;
    string hiOutcome;
    string prompt, onBaseInfo, outsInfo, scoreInfo, inningInfo;
    string pitchType, hitType;

    public AudioClip hitSoundClip, outSoundClip;


    List<string> choices = new List<string> { "Single", "Double", "Triple", "HomeRun", "Out" };
    List<double> probabilities = new List<double> { 0.3, 0.15, 0.07, 0.06, 0.42 };

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
        GameObject.Find("SphereFirst").GetComponent<Renderer>().enabled = onFirst;
        GameObject.Find("SphereSecond").GetComponent<Renderer>().enabled = onSecond;
        GameObject.Find("SphereThird").GetComponent<Renderer>().enabled = onThird;
        GameObject.Find("SphereFirst").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue;
        GameObject.Find("SphereSecond").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue;
        GameObject.Find("SphereThird").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.red : Color.blue;
        GameObject.Find("CubePitcher").GetComponent<Renderer>().material.color = (teamAtBat == "A") ? Color.blue : Color.red;
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

        hiOutcome = "";

        prompt = "";
        onBaseInfo = "";
        outsInfo = "";
        scoreInfo = "";
        inningInfo = "";

        pitchType = GameObject.Find("PitchValueLabel").GetComponent<TextMeshProUGUI>().text;
        hitType = GameObject.Find("HitValueLabel").GetComponent<TextMeshProUGUI>().text;
    }

    public void PitchButtonPressed()
    {
        pitchType = GameObject.Find("PitchValueLabel").GetComponent<TextMeshProUGUI>().text;
        prompt = "The pitch was thrown. It's time to decide: Hit or Wait?";
    }

    public void HitButtonPressed()
    {
        hitType = GameObject.Find("HitValueLabel").GetComponent<TextMeshProUGUI>().text;
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
        //GameObject.Find("OutcomeText").GetComponent<TextMeshProUGUI>().text = "The batter hit a " + hiOutcome;
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
        } else {
            scoreB = scoreB + 1;
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
        prompt = "End of the inning. Switch sides!";
    }
}
