using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    WaitForSeconds oneSec;
    public Transform[] spawnPositions;

    CharacterManager charM;
    LevelUI levelUI;

    public int maxturns = 2;
    int currentTurn = 1;

    public bool countdown;
    public int maxTurnTimer = 30;
    int currentTimer;
    float internalTimer;

	// Use this for initialization
	void Start () {
        charM = CharacterManager.GetInstance();
        levelUI = LevelUI.GetInstance();

        oneSec = new WaitForSeconds(1);

        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        StartCoroutine("StartGame");
	}

    void FixedUpdate()
    {
        //States
    }
    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator StartGame() {
        yield return CreatePlayers();

        yield return InitTurn();
    }

    IEnumerator InitTurn() {
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        currentTimer = maxTurnTimer;
        countdown = false;

        yield return InitPlayers();

        yield return EnableControl(); 
    }

    IEnumerator CreatePlayers()
    {

        for (int i = 0; i < charM.players.Count; i++) {

            GameObject go = Instantiate(charM.players[i].playerPrefab, spawnPositions[i].position, Quaternion.identity) as GameObject;

            //charM.players[i].playerStates = go.GetComponent<StateManager>();
            //charM.players[i].playerStates.healthSlider = levelUI.healthSlider[i];

        }
        yield return null;
    } 

    IEnumerator InitPlayers() { 
        for(int i = 0; i < charM.players.Count; i++) {
            //charM.players[i].playerStates
            //charM.players[i].playerStates
            //charM.players[i].playerStates
        }
        yield return null;
    }

    IEnumerator EnableControl() {
        levelUI.AnnouncerTextLine1.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine1.text = "Turn" + currentTurn;
        levelUI.AnnouncerTextLine1.color = Color.white;

        yield return oneSec;
        yield return oneSec;


        levelUI.AnnouncerTextLine1.text = "3";
        levelUI.AnnouncerTextLine1.color = Color.green;
        yield return oneSec;

        levelUI.AnnouncerTextLine1.text = "2";
        levelUI.AnnouncerTextLine1.color = Color.yellow;
        yield return oneSec;

        levelUI.AnnouncerTextLine1.text = "1";
        levelUI.AnnouncerTextLine1.color = Color.red;
        yield return oneSec;

        levelUI.AnnouncerTextLine1.text = "FIGHT!";
        levelUI.AnnouncerTextLine1.color = Color.red;

        for (int i = 0; i < charM.players.Count; i++) {
            if(charM.players[i].playerType == PlayerBase.PlayerType.user) { 
                //Input Handler
            }
        }

        yield return oneSec;
        levelUI.AnnouncerTextLine1.gameObject.SetActive(false);
        countdown = true;

    }
}
