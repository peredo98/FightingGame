using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectScreenManager : MonoBehaviour {

    public int numberOfPlayers = 1;
    public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();
    public PotraitInfo[] potraitPrefabs;
    public int maxX;
    public int maxY;
    PotraitInfo[,] charGrid;

    public GameObject potraitCanvas;

    bool loadLevel;
    public bool bothPlayersSelected;

    CharacterManager charMnager;

    public static SelectScreenManager instance;
    public static SelectScreenManager GetInstance() {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        charMnager = CharacterManager.GetInstance();
        numberOfPlayers = charMnager.numberOfUsers;

        charGrid = new PotraitInfo[maxX, maxY];

        int x = 0;
        int y = 0;

        potraitPrefabs = potraitCanvas.GetComponentsInChildren<PotraitInfo>();

        for (int i = 0; i<potraitPrefabs.Length; i++) {
            potraitPrefabs[i].posX += x;
            potraitPrefabs[i].posY += y;

            charGrid[x, y] = potraitPrefabs[i];
            if(x < maxX - 1) {
                x++;
            }
            else {
                x = 0;
                y++;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!loadLevel) { 
            for(int i = 0; i < plInterfaces.Count; i++) { 
                if(i < numberOfPlayers)
                {
                    if(Input.GetButtonUp("Fire2" + charMnager.players[i].inputId)) {
                        plInterfaces[i].playerBase.hasCharacter = false; 
                    }
                    if (!charMnager.players[i].hasCharacter) {
                        plInterfaces[i].playerBase = charMnager.players[i];

                        HandleSelectorPosition(plInterfaces[i]);
                        HandleSelectScreenInput(plInterfaces[i], charMnager.players[i].inputId);
                        HandleCharacterPreview(plInterfaces[i]);
                     }
                }
                else {
                    charMnager.players[i].hasCharacter = true; 
                }
            }
        }
        if (bothPlayersSelected) {
            StartCoroutine("LoadLevel");
            loadLevel = true;
        }
        else { 
            if(charMnager.players[0].hasCharacter && charMnager.players[1].hasCharacter) {
                bothPlayersSelected = true;
            }
        }
    }

    void HandleSelectScreenInput(PlayerInterfaces pl, string playerId) {
        float vertical = Input.GetAxis("Vertical" + playerId);

        if(vertical != 0)
        {
            if (!pl.hitInoutOnce) { 
                if(vertical > 0) {
                    pl.activeY = (pl.activeY > 0) ? pl.activeY - 1 : maxY - 1;
                }
                else {
                    pl.activeY = (pl.activeY < maxY - 1) ? pl.activeY + 1 : 0;
                }

                pl.hitInoutOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId);

        if(horizontal != 0)
        {
            if (!pl.hitInoutOnce)
            {
                if (horizontal > 0)
                {
                    pl.activeX = (pl.activeX > 0) ? pl.activeX - 1 : maxX - 1;
                }
                else
                {
                    pl.activeX = (pl.activeX < maxX - 1) ? pl.activeX + 1 : 0;
                }
                pl.timerToReset = 0;
                pl.hitInoutOnce = true;
            }
        }

        if(vertical == 0 && horizontal == 0) {
            pl.hitInoutOnce = false;
         }

        if (pl.hitInoutOnce) {
            pl.timerToReset += Time.deltaTime;

            if (pl.timerToReset > 0.8f) {
                pl.hitInoutOnce = false;
                pl.timerToReset = 0;
            }
        }

        if(Input.GetButtonUp("Jump" + playerId)) {
            //pl.createdCharacter.GetComponent<Animator>().Play("Kick");
            pl.playerBase.playerPrefab = charMnager.returnCharacterWithID(pl.activePotrait.characterId).prefab;
            pl.playerBase.hasCharacter = true;
        }
    }

    IEnumerator LoadLevel()
    {
        for (int i = 0; i < charMnager.players.Count; i++)
        {
            if (charMnager.players[i].playerType == PlayerBase.PlayerType.ai) {
                if(charMnager.players[i].playerPrefab == null) {
                    int ranValue = Random.Range(0, potraitPrefabs.Length);

                    charMnager.players[i].playerPrefab = charMnager.returnCharacterWithID(potraitPrefabs[ranValue].characterId
                    ).prefab;
                }
            }
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("stage", LoadSceneMode.Single);
    }

    void HandleSelectorPosition(PlayerInterfaces pl) {
        pl.selector.SetActive(true);

        pl.activePotrait = charGrid[pl.activeX
        , pl.activeY];

        Vector2 selectorPosition = pl.activePotrait.transform.localPosition;
        selectorPosition = selectorPosition + new Vector2(potraitCanvas.transform.localPosition.x, potraitCanvas.transform.localPosition.y);
        pl.selector.transform.localPosition = selectorPosition;
    }

    void HandleCharacterPreview(PlayerInterfaces pl)
    {
        if(pl.previewPotrait != pl.activePotrait) {
            if (pl.createdCharacter != null)
            {
                Destroy(pl.createdCharacter);
            }

            GameObject go = Instantiate(CharacterManager.GetInstance().returnCharacterWithID(pl.activePotrait.characterId).prefab , pl.charVisPos.position, Quaternion.identity) as GameObject;
 
            pl.createdCharacter = go;
            pl.previewPotrait = pl.activePotrait;

            if (!string.Equals(pl.playerBase.playerId, charMnager.players[0].playerId)) { 
                pl.createdCharacter.GetComponent<StateManager>().lookRight = false;
            }
        }


    }
}

[System.Serializable]
public class PlayerInterfaces {
    public PotraitInfo activePotrait;
    public PotraitInfo previewPotrait;
    public GameObject selector;
    public Transform charVisPos;
    public GameObject createdCharacter;

    public int activeX;
    public int activeY;

    public bool hitInoutOnce;
    public float timerToReset;

    public PlayerBase playerBase;


}
