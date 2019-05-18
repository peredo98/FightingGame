using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {

    CharacterManager charMnager;
    public PotraitInfo[] potraitPrefabs;
    public int maxX;
    public int maxY;
    PotraitInfo[,] charGrid;
    public GameObject potraitCanvas;
    public List<PlayerInterfaces> plInterfaces = new List<PlayerInterfaces>();
    bool loadLevel;
    public static StageManager instance;
    public static StageManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void HandleSelectorPosition(PlayerInterfaces pl)
    {
        pl.selector.SetActive(true);

        pl.activePotrait = charGrid[pl.activeX
        , pl.activeY];

        Vector2 selectorPosition = pl.activePotrait.transform.localPosition;
        selectorPosition = selectorPosition + new Vector2(potraitCanvas.transform.localPosition.x, potraitCanvas.transform.localPosition.y);
        pl.selector.transform.localPosition = selectorPosition;
    }

    void HandleSelectScreenInput(PlayerInterfaces pl, string playerId)
    {
        float vertical = Input.GetAxis("Vertical" + playerId);

        if (vertical != 0)
        {
            if (!pl.hitInoutOnce)
            {
                if (vertical > 0)
                {
                    pl.activeY = (pl.activeY > 0) ? pl.activeY - 1 : maxY - 1;
                }
                else
                {
                    pl.activeY = (pl.activeY < maxY - 1) ? pl.activeY + 1 : 0;
                }

                pl.hitInoutOnce = true;
            }
        }

        float horizontal = Input.GetAxis("Horizontal" + playerId);

        if (horizontal != 0)
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

        if (vertical == 0 && horizontal == 0)
        {
            pl.hitInoutOnce = false;
        }

        if (pl.hitInoutOnce)
        {
            pl.timerToReset += Time.deltaTime;

            if (pl.timerToReset > 0.8f)
            {
                pl.hitInoutOnce = false;
                pl.timerToReset = 0;
            }
        }

        if (Input.GetButtonUp("Fire1" + playerId) || Input.GetButtonUp("Jump" + playerId))
        {
            //pl.createdCharacter.GetComponent<Animator>().Play("Kick");
            //pl.playerBase.playerPrefab = charMnager.returnCharacterWithID(pl.activePotrait.characterId).prefab;
            //pl.playerBase.hasCharacter = true;
        }
    }


    IEnumerator LoadLevel()
    {

        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("level", LoadSceneMode.Single);
    }
    // Use this for initialization
    void Start()
    {
        charMnager = CharacterManager.GetInstance();
        //numberOfPlayers = charMnager.numberOfUsers;



        charGrid = new PotraitInfo[maxX, maxY];

        int x = 0;
        int y = 0;

        potraitPrefabs = potraitCanvas.GetComponentsInChildren<PotraitInfo>();

        for (int i = 0; i < potraitPrefabs.Length; i++)
        {
            potraitPrefabs[i].posX += x;
            potraitPrefabs[i].posY += y;

            charGrid[x, y] = potraitPrefabs[i];
            if (x < maxX - 1)
            {
                x++;
            }
            else
            {
                x = 0;
                y++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!loadLevel)
        {
            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Jump"))
            {
                //potraitPrefabs[ranValue].characterId;
                charMnager.selectedSprite = Resources.Load<Sprite>(plInterfaces[0].activePotrait.characterId);
                StartCoroutine("LoadLevel");
                loadLevel = true;

            }

            HandleSelectorPosition(plInterfaces[0]);
            HandleSelectScreenInput(plInterfaces[0], "");

        }
       
    }
}
