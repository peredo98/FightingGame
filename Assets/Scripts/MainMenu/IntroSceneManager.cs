﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour {

    public GameObject startText;
    float timer;
    bool loadingLevel;
    bool init;

    public int activeElement;
    public GameObject menuObj;
    public ButtonRef[] menuOptions;

	// Use this for initialization
	void Start () {
        menuObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!init) {
            timer += Time.deltaTime;
            if (timer > 0.6f) {
                timer = 0;
                startText.SetActive(!startText.activeInHierarchy);
            }

            if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump") || Input.GetButtonUp("Submit")) {
                init = true;
                startText.SetActive(false);
                menuObj.SetActive(true);
            }
        }
        else
        {
            if (!loadingLevel)
            {
                menuOptions[activeElement].selected = true;

                if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0.6) {
                    menuOptions[activeElement].selected = false;

                    if (activeElement > 0) {
                        activeElement--;
                    }
                    else {
                        activeElement = menuOptions.Length - 1;
                    }
                }
                if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetAxis("Vertical") < -0.6)
                {
                    menuOptions[activeElement].selected = false;

                    if(activeElement < menuOptions.Length - 1) {
                        activeElement++;
                    }
                    else {
                        activeElement = 0;
                    }
                }
                if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump") || Input.GetButtonUp("Submit"))
                {
                    Debug.Log("Load");
                    loadingLevel = true;
                    StartCoroutine("LoadLevel");
                    menuOptions[activeElement].transform.localScale *= 1.2f;

                }
            }
        }
    }

    void HandleSelectedOption()
    {
        switch (activeElement) {
            case 0: 
                CharacterManager.GetInstance().numberOfUsers = 1;
                break;
            case 1:
                CharacterManager.GetInstance().numberOfUsers = 2;
                CharacterManager.GetInstance().players[1].playerType = PlayerBase.PlayerType.user;
                break;
        }
    }

    IEnumerator LoadLevel() {
        HandleSelectedOption();
        yield return new WaitForSeconds(0.6f);
        startText.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("select", LoadSceneMode.Single);
    }
}
