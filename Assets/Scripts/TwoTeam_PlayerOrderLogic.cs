using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TwoTeam_PlayerOrderLogic : MonoBehaviour
{
    public List<GameObject> teamAPlayerButtonObject;
    public List<GameObject> teamBPlayerButtonObject;
    public List<Sprite> playerSpriteList;
    int hadSelectedPlayers;
    int totalPlayers = 18;

    public GameObject autoOrderButton;

    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
    }

    // Update is called once per frame
    void Update()
    {
        if(hadSelectedPlayers >= totalPlayers) {
            StopAllCoroutines();
            autoOrderButton.SetActive(false);
            GameObject.Find("StartGameButton").GetComponent<Button>().interactable = true;
        } else {
            GameObject.Find("StartGameButton").GetComponent<Button>().interactable = false;
            autoOrderButton.SetActive(true);
        }
    }

    public void StartGameButtonPressed()
    {
        SceneManager.LoadSceneAsync("TwoTeams");
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

    public void UpdateGUIButtonText(string teamAtSelect)
    {
        // Update all button text when cancel
        if (teamAtSelect == "A") {
            for(int i = 0;i<teamAPlayerButtonObject.Count;i++)
            {
                Button b = teamAPlayerButtonObject[i].GetComponent<Button>();
                string bText = b.GetComponentInChildren<TMP_Text>().text;
                if(bText.Contains("第") && bText.Contains("棒")) {
                    int playerIndex = TwoTeam_SharedData.teamAPlayerList[i];
                    int playerOrderListIndex = TwoTeam_SharedData.teamAPlayerOrderedList.FindIndex(index => index==playerIndex);
                    b.GetComponentInChildren<TMP_Text>().text = "第 " + (playerOrderListIndex+1) + " 棒";
                }
            }
        } else {
            for(int i = 0;i<teamBPlayerButtonObject.Count;i++)
            {
                Button b = teamBPlayerButtonObject[i].GetComponent<Button>();
                string bText = b.GetComponentInChildren<TMP_Text>().text;
                if(bText.Contains("第") && bText.Contains("棒")) {
                    int playerIndex = TwoTeam_SharedData.teamBPlayerList[i];
                    int playerOrderListIndex = TwoTeam_SharedData.teamBPlayerOrderedList.FindIndex(index => index==playerIndex);
                    b.GetComponentInChildren<TMP_Text>().text = "第 " + (playerOrderListIndex+1) + " 棒";
                }
            }
        }
    }

    void PlayerButtonSelected(GameObject gameObj, string teamAtSelect, bool isAutoSelection)
    {
        Button btnObject = gameObj.GetComponent<Button>();
        string playerTextName = btnObject.GetComponentInChildren<TMP_Text>().text;
        if(playerTextName.Contains("第") && playerTextName.Contains("棒")) {
            if(isAutoSelection) {
                return;
            }
            // The button is selected, remove player from list and revert button
            hadSelectedPlayers = hadSelectedPlayers - 1;
            string extractOrderIndex = Regex.Match(playerTextName, @"\d+").Value;
            int playerOrdering = Int32.Parse(extractOrderIndex);
            if (teamAtSelect == "A") {
                int playerIndexInMatrix = TwoTeam_SharedData.teamAPlayerOrderedList[playerOrdering-1];
                TwoTeam_SharedData.teamAPlayerOrderedList.RemoveAt(playerOrdering-1);
                btnObject.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[playerIndexInMatrix];
                UpdateGUIButtonText("A");
            } else {
                int playerIndexInMatrix = TwoTeam_SharedData.teamBPlayerOrderedList[playerOrdering-1];
                TwoTeam_SharedData.teamBPlayerOrderedList.RemoveAt(playerOrdering-1);
                btnObject.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[playerIndexInMatrix];
                UpdateGUIButtonText("B");
            }
            //Changes the button's Selected color to the original color.
            ColorBlock cb = btnObject.colors;
            cb.normalColor = new Color(1f, 1f, 1f, 1f);
            cb.highlightedColor = new Color(1f, 1f, 1f, 1f);
            cb.selectedColor = new Color(1f, 1f, 1f, 1f);
            btnObject.colors = cb;
            btnObject.GetComponentInChildren<TMP_Text>().alpha = 0f;
        } else {
            // The button is not selected, add player to list
            hadSelectedPlayers = hadSelectedPlayers + 1;
            int playerIndexInMatrix = TwoTeam_SharedData.playerList.FindIndex(playerName => playerName==playerTextName);
            if (teamAtSelect == "A") {
                TwoTeam_SharedData.teamAPlayerOrderedList.Add(playerIndexInMatrix);
                btnObject.GetComponentInChildren<TMP_Text>().text = "第 " + TwoTeam_SharedData.teamAPlayerOrderedList.Count + " 棒";
            } else {
                TwoTeam_SharedData.teamBPlayerOrderedList.Add(playerIndexInMatrix);
                btnObject.GetComponentInChildren<TMP_Text>().text = "第 " + TwoTeam_SharedData.teamBPlayerOrderedList.Count+ " 棒";
            }
            //Changes the button's Selected color to the new color.
            ColorBlock cb = btnObject.colors;
            cb.normalColor = (teamAtSelect == "A") ? new Color(0.55f, 0.03f, 0f, 0.6f) : new Color(0f, 0.24f, 0.56f, 0.6f);
            cb.highlightedColor = (teamAtSelect == "A") ? new Color(0.55f, 0.03f, 0f, 0.6f) : new Color(0f, 0.24f, 0.56f, 0.6f);
            cb.selectedColor = (teamAtSelect == "A") ? new Color(0.55f, 0.03f, 0f, 0.6f) : new Color(0f, 0.24f, 0.56f, 0.6f);
            btnObject.colors = cb;
            btnObject.GetComponentInChildren<TMP_Text>().alpha = 1f;
        }
    }

    public void TeamAPlayerButtonSelected(GameObject gameObj)
    {
        PlayerButtonSelected(gameObj, "A", false);
    }

    public void TeamBPlayerButtonSelected(GameObject gameObj)
    {
        PlayerButtonSelected(gameObj, "B", false);
    }

    // Initialize the variables
    void InitializeValues()
    {
        // Debug Usage
        // for(int i = 0;i<teamAPlayerButtonObject.Count;i++) {
        //     TwoTeam_SharedData.teamAPlayerList.Add(i);
        // }
        // for(int i = 0;i<teamBPlayerButtonObject.Count;i++) {
        //     TwoTeam_SharedData.teamBPlayerList.Add(i+9);
        // }

        TwoTeam_SharedData.teamAPlayerOrderedList.Clear();
        TwoTeam_SharedData.teamBPlayerOrderedList.Clear();
        hadSelectedPlayers = 0;
        //Debug.Log(string.Join(", ", TwoTeam_SharedData.teamAPlayerList));
        //Debug.Log(string.Join(", ", TwoTeam_SharedData.teamBPlayerList));
        for(int i = 0;i<teamAPlayerButtonObject.Count;i++)
        {
            int playerIndex = TwoTeam_SharedData.teamAPlayerList[i];
            Button b = teamAPlayerButtonObject[i].GetComponent<Button>();
            b.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[playerIndex];
            b.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 0);
            b.GetComponent<Image>().sprite = playerSpriteList[playerIndex];
            int labelIndex = i + 1;
            GameObject.Find("TeamA_PlayerLabelNameText_"+labelIndex).GetComponent<TextMeshProUGUI>().text = TwoTeam_SharedData.playerList[playerIndex];
        }
        for(int i = 0;i<teamBPlayerButtonObject.Count;i++)
        {
            int playerIndex = TwoTeam_SharedData.teamBPlayerList[i];
            Button b = teamBPlayerButtonObject[i].GetComponent<Button>();
            b.GetComponentInChildren<TMP_Text>().text = TwoTeam_SharedData.playerList[playerIndex];
            b.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 0);
            b.GetComponent<Image>().sprite = playerSpriteList[playerIndex];
            int labelIndex = i + 1;
            GameObject.Find("TeamB_PlayerLabelNameText_"+labelIndex).GetComponent<TextMeshProUGUI>().text = TwoTeam_SharedData.playerList[playerIndex];
        }
    }

    public void AutoOrderButtonPressed() {
        //autoOrderButton.GetComponent<Button>().interactable = false;
        IEnumerator co;
        co = AutoOrderCoroutine();
        StartCoroutine(co);
        //StopCoroutine(co);
    }

    IEnumerator AutoOrderCoroutine()
    {
        List<int> SelectedIndex = new List<int>();
        //List<int> PlayerBSelectedIndex = new List<int>();
        int totalPlayersEachTeam = totalPlayers / 2;
        for(int i = 0;i<totalPlayersEachTeam;i++)
        {
            SelectedIndex.Add(i);
        }
        List<int> RandomPlayerASelectedIndex = SelectedIndex.OrderBy(x => Guid.NewGuid()).ToList();
        List<int> RandomPlayerBSelectedIndex = SelectedIndex.OrderBy(x => Guid.NewGuid()).ToList();
        for(int i = 0;i<totalPlayersEachTeam;i++)
        {
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(PlayerButtonObject[RandomSelectedIndex[i]]);
            //selectedPlayerButtonName = PlayerButtonObject[RandomSelectedIndex[i]].name;
            //PlayerButtonSelected(PlayerButtonObject[RandomSelectedIndex[i]]);
            PlayerButtonSelected(teamAPlayerButtonObject[RandomPlayerASelectedIndex[i]], "A", true);
            PlayerButtonSelected(teamBPlayerButtonObject[RandomPlayerBSelectedIndex[i]], "B", true);
            yield return new WaitForSeconds(0.2F);
        }
    }
}
