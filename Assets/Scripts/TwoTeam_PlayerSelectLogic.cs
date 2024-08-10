using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class TwoTeam_PlayerSelectLogic : MonoBehaviour
{
    string teamAtSelect;
    int hadSelectedPlayers;
    int totalPlayers = 18;
    string prompt;
    string selectedPlayerButtonName, showInfoPlayerButtonName;
    string selectedTitle;
    public GameObject confirmButton, goOrderingButton, showInfoButton, autoSelectButton;
    public Canvas playerInfoCanvas;
    public List<GameObject> PlayerButtonObject;
    public List<Sprite> playerSpriteList;
    public Image playerInfoImage;
    bool randomSelecting;

    // Start is called before the first frame update
    void Start()
    {
        playerInfoCanvas.enabled = false;
        InitializeValues();
    }

    // Update is called once per frame
    void Update()
    {
        if(hadSelectedPlayers >= totalPlayers) {
            StopAllCoroutines();
            confirmButton.SetActive(false);
            showInfoButton.SetActive(false);
            goOrderingButton.SetActive(true);
            autoSelectButton.SetActive(false);
            //GameObject.Find("GoOrderingButton").GetComponent<Button>().interactable = true;
            selectedTitle = "兩隊皆已完成球員選擇";
            prompt = "點擊「下一步」繼續";
        } else {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject) {
                GameObject.Find("ConfirmButton").GetComponent<Button>().interactable = true;
                GameObject.Find("ShowInfoButton").GetComponent<Button>().interactable = true;
            } else {
                if (randomSelecting == false) {
                    selectedTitle = "請選擇各自隊伍的球員";
                    prompt = "";
                }
                GameObject.Find("ConfirmButton").GetComponent<Button>().interactable = false;
                GameObject.Find("ShowInfoButton").GetComponent<Button>().interactable = false;
            }
        }
        GameObject.Find("PromptText").GetComponent<TextMeshProUGUI>().text = prompt;
        GameObject.Find("SelectTitle").GetComponent<TextMeshProUGUI>().text = selectedTitle;
    }

    public void GoOrderingButtonPressed()
    {
        SceneManager.LoadSceneAsync("TwoTeams_PlayerOrdering");
    }

    public void ClickButtonSound()
    {
        GameObject.Find("ButtonClickAudioSource").GetComponent<AudioSource>().Play();
    }

    public void SelectButtonSound()
    {
        GameObject.Find("ButtonSelectAudioSource").GetComponent<AudioSource>().Play();
    }

    public void StartButtonSound()
    {
        GameObject.Find("ButtonStartAudioSource").GetComponent<AudioSource>().Play();
    }

    public void ConfirmButtonPressed()
    {

        Button btnObject = GameObject.Find(selectedPlayerButtonName).GetComponent<Button>();

        if(!btnObject.interactable)
        {
            return;
        }

        btnObject.interactable = false;

        //Changes the button's Selected color to the new color.
        ColorBlock cb = btnObject.colors;
        cb.disabledColor = (teamAtSelect == "A") ? new Color(0.55f, 0.03f, 0f, 0.6f) : new Color(0f, 0.24f, 0.56f, 0.6f);
        btnObject.colors = cb;

        string playerTextName = btnObject.GetComponentInChildren<TMP_Text>().text;
        //Debug.Log(playerTextName);

        int playerIndexInMatrix = TwoTeam_SharedData.playerList.FindIndex(playerName => playerName==playerTextName);

        if (teamAtSelect == "A") {
            TwoTeam_SharedData.teamAPlayerList.Add(playerIndexInMatrix);
            //btnObject.GetComponentInChildren<TMP_Text>().text = "A - " + TwoTeam_SharedData.teamAPlayerList.Count;
        } else {
            TwoTeam_SharedData.teamBPlayerList.Add(playerIndexInMatrix);
            //btnObject.GetComponentInChildren<TMP_Text>().text = "B - " + TwoTeam_SharedData.teamBPlayerList.Count;
        }
        btnObject.GetComponentInChildren<TMP_Text>().text = "已選擇";
        btnObject.GetComponentInChildren<TMP_Text>().alpha = 1f;

        //prompt = "Team " + teamAtSelect + " has selected " + playerTextName;
        
        teamAtSelect = (teamAtSelect == "A") ? "B" : "A";

        // Debug.Log(string.Join(", ", TwoTeam_SharedData.teamAPlayerList));
        // Debug.Log(string.Join(", ", TwoTeam_SharedData.teamBPlayerList));

        hadSelectedPlayers = hadSelectedPlayers + 1;
        // if(hadSelectedPlayers >= totalPlayers) {
        //     SceneManager.LoadSceneAsync("TwoTeams_PlayerOrdering");
        // }

        //int playerIndex  = TwoTeam_SharedData.playerList.FindIndex(playerName => playerName==btnObject.GetComponentInChildren<TMP_Text>().text);
        //Debug.Log(playerIndex+"_HAHA");

        selectedPlayerButtonName = "";

        GameObject.Find("ConfirmButton").GetComponent<Button>().interactable = false;
    }

    public void OpenInfoCanvasButtonPressed()
    {

        Button btnObject = GameObject.Find(selectedPlayerButtonName).GetComponent<Button>();

        // Save the player button clicked for later use
        showInfoPlayerButtonName = selectedPlayerButtonName;

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

    public void CloseInfoCanvasButtonPressed()
    {
        playerInfoCanvas.enabled = false;
        GameObject restoreSelectedPlayerButtonObj = GameObject.Find(showInfoPlayerButtonName);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(restoreSelectedPlayerButtonObj);
        showInfoPlayerButtonName = "";
        PlayerButtonSelected(restoreSelectedPlayerButtonObj);
    }

    public void PlayerButtonSelected(GameObject gameObj)
    {
        selectedPlayerButtonName = gameObj.name;

        Button btnObject = gameObj.GetComponent<Button>();
        string playerTextName = btnObject.GetComponentInChildren<TMP_Text>().text;
        if(playerTextName == "已選擇") {
            return;
        }
        int playerIndexInMatrix = TwoTeam_SharedData.playerList.FindIndex(playerName => playerName==playerTextName);
        List<string> playerInfo = TwoTeam_SharedData.playerList_Info[playerIndexInMatrix];

        selectedTitle = playerTextName;
        prompt = playerInfo[0] + " / " + playerInfo[1] + " / " + playerInfo[2] + " / " + playerInfo[3] + " / " + playerInfo[4];
    }

    // Initialize the variables
    void InitializeValues()
    {
        teamAtSelect = "A";
        TwoTeam_SharedData.teamAPlayerList.Clear();
        TwoTeam_SharedData.teamBPlayerList.Clear();
        hadSelectedPlayers = 0;
        selectedPlayerButtonName = "";
        prompt = "";
        selectedTitle = "請選擇各自隊伍的球員";
        randomSelecting = false;
        
        for(int i = 0;i<PlayerButtonObject.Count;i++)
        {
            Button b = PlayerButtonObject[i].GetComponent<Button>();
            b.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[i];
            b.GetComponent<Image>().sprite = playerSpriteList[i];
        }
    }

    public void AutoSelectButtonPressed() {
        randomSelecting = true;
        autoSelectButton.GetComponent<Button>().interactable = false;
        showInfoButton.GetComponent<Button>().interactable = false;
        confirmButton.GetComponent<Button>().interactable = false;
        IEnumerator co;
        co = AutoSelectCoroutine();
        StartCoroutine(co);
        //StopCoroutine(co);
    }

    IEnumerator AutoSelectCoroutine()
    {
        List<int> SelectedIndex = new List<int>();
        for(int i = 0;i<totalPlayers;i++)
        {
            SelectedIndex.Add(i);
        }
        List<int> RandomSelectedIndex = SelectedIndex.OrderBy(x => Guid.NewGuid()).ToList();
        for(int i = 0;i<totalPlayers;i++)
        {
            GameObject gameObj = PlayerButtonObject[RandomSelectedIndex[i]];
            Button btnObject = gameObj.GetComponent<Button>();
            string playerTextName = btnObject.GetComponentInChildren<TMP_Text>().text;
            if(playerTextName == "已選擇") {
                continue;
            }
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PlayerButtonObject[RandomSelectedIndex[i]]);
            //selectedPlayerButtonName = PlayerButtonObject[RandomSelectedIndex[i]].name;
            PlayerButtonSelected(PlayerButtonObject[RandomSelectedIndex[i]]);
            ConfirmButtonPressed();
            yield return new WaitForSeconds(0.2F);
        }
    }
}
