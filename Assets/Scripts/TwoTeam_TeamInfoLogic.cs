using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TwoTeam_TeamInfo : MonoBehaviour
{
    public List<GameObject> teamAPlayerButtonObject;
    public List<GameObject> teamBPlayerButtonObject;
    public List<Sprite> playerSpriteList;
    //int totalPlayers = 18;
    public Canvas teamInfoCanvas, playerInfoCanvas;
    string selectedPlayerButtonName;
    public Image playerInfoImage;

    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
        teamInfoCanvas.enabled = false;
        playerInfoCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTeamInfoCanvasPressed()
    {
        teamInfoCanvas.enabled = true;
        TwoTeam_SharedData.canvasOpened = true;
    }

    public void CloseTeamInfoCanvasPressed()
    {
        teamInfoCanvas.enabled = false;
        TwoTeam_SharedData.canvasOpened = false;
    }

    public void PlayerButtonSelected(GameObject gameObj)
    {
        selectedPlayerButtonName = gameObj.name;
        //Button btnObject = gameObj.GetComponent<Button>();
        //string playerTextName = btnObject.GetComponentInChildren<TMP_Text>().text;
        //int playerIndexInMatrix = TwoTeam_SharedData.playerList.FindIndex(playerName => playerName==playerTextName);
        //List<string> playerInfo = TwoTeam_SharedData.playerList_Info[playerIndexInMatrix];

        //selectedTitle = playerTextName;
        //prompt = playerInfo[0] + " / " + playerInfo[1] + " / " + playerInfo[2] + " / " + playerInfo[3] + " / " + playerInfo[4];
        OpenPlayerInfoCanvas();
    }

    public void OpenPlayerInfoCanvas()
    {
        Button btnObject = GameObject.Find(selectedPlayerButtonName).GetComponent<Button>();

        // Save the player button clicked for later use
        //showInfoPlayerButtonName = selectedPlayerButtonName;

        string playerTextName = btnObject.GetComponentInChildren<TMP_Text>().text;

        int playerIndexInMatrix = TwoTeam_SharedData.playerList.FindIndex(playerName => playerName==playerTextName);

        selectedPlayerButtonName = "";

        // Show Canvas Panel and update information
        playerInfoCanvas.enabled = true;
        GameObject.Find("PlayerInfoName").GetComponent<TextMeshProUGUI>().text = playerTextName;
        List<string> playerInfo = TwoTeam_SharedData.playerList_Info[playerIndexInMatrix];
        //Height, Weight, Batting Hand, Age, Experience, AVG, HR, RBI
        string playerInfoDetailText = playerInfo[0] + "\n" 
                                    + playerInfo[1] + "\n" 
                                    + playerInfo[2] + "\n" 
                                    + playerInfo[3] + "\n"
                                    + playerInfo[4] + "\n" 
                                    + playerInfo[5] + "\n"
                                    + playerInfo[6] + "\n"
                                    + playerInfo[7] + "\n"
                                    + playerInfo[8] + "\n"
                                    + playerInfo[9] + "\n";
        GameObject.Find("PlayerInfoDetail").GetComponent<TextMeshProUGUI>().text = playerInfoDetailText;
        playerInfoImage.sprite = playerSpriteList[playerIndexInMatrix];
    }

    public void ClosePlayerInfoCanvasButtonPressed()
    {
        playerInfoCanvas.enabled = false;
        //GameObject restoreSelectedPlayerButtonObj = GameObject.Find(showInfoPlayerButtonName);
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(restoreSelectedPlayerButtonObj);
        //showInfoPlayerButtonName = "";
        //PlayerButtonSelected(restoreSelectedPlayerButtonObj);
    }

    void InitializeValues()
    {
        for(int i = 0;i<teamAPlayerButtonObject.Count;i++)
        {
            int playerIndex = TwoTeam_SharedData.teamAPlayerOrderedList[i];
            Button b = teamAPlayerButtonObject[i].GetComponent<Button>();
            b.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[playerIndex];
            b.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 0);
            b.GetComponent<Image>().sprite = playerSpriteList[playerIndex];
            int labelIndex = i + 1;
            GameObject.Find("TeamA_PlayerLabelNameText_"+labelIndex).GetComponent<TextMeshProUGUI>().text = TwoTeam_SharedData.playerList[playerIndex];
        }
        for(int i = 0;i<teamBPlayerButtonObject.Count;i++)
        {
            int playerIndex = TwoTeam_SharedData.teamBPlayerOrderedList[i];
            Button b = teamBPlayerButtonObject[i].GetComponent<Button>();
            b.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[playerIndex];
            b.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 0);
            b.GetComponent<Image>().sprite = playerSpriteList[playerIndex];
            int labelIndex = i + 1;
            GameObject.Find("TeamB_PlayerLabelNameText_"+labelIndex).GetComponent<TextMeshProUGUI>().text = TwoTeam_SharedData.playerList[playerIndex];
        }
    }
}
