using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene, Paused, Menu, PartyScreen, Bag }

public class GameController : MonoBehaviour
{
    public GameState state;
    GameState previousState;
    public static GameController Instance { get; set; }

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    MenuController menuController;

    public bool chest1 = false;
    public bool chest2 = false;
    public bool chest3 = false;
    public bool chest4 = false;
    public bool chest5 = false;
    public bool chest6 = false;
    public bool chest7 = false;
    public bool chest8 = false;
    public bool chest9 = false;
    public bool chest10 = false;
    public bool chest11 = false;
    public bool chest12 = false;
    public bool chest13 = false;
    public bool chest14 = false;
    public bool chest15 = false;
    public bool chest16 = false;
    public bool chest17 = false;
    public bool chest18 = false;
    public bool chest19 = false;
    public bool chest20 = false;
    public bool chest21 = false;
    public bool chest22 = false;
    public bool chest23 = false;
    public bool chest24 = false;
    public bool chest25 = false;

    public bool button1 = false;
    public bool button2 = false;
    public bool button3 = false;
    public bool button4 = false;
    public bool button5 = false;
    public bool button6 = false;
    public bool button7 = false;
    public bool button8 = false;
    public bool button9 = false;
    public bool button10 = false;
    public bool button11 = false;

    public void Awake()
    {
        Instance = this;
        ConditionsDB.Init();
        menuController = GetComponent<MenuController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
}


    Fader fader;
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
        battleSystem.OnBattleOver += EndBattle;
        partyScreen.Init();

        DialogManager.Instance.OnShowDialog += () =>
        {
            previousState = state;
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if(state == GameState.Dialog)
            {
                state = previousState;
            }
        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;
    }

    public void PauseGame(bool pause)
    {
        if(pause)
        {
            previousState = state;
            state = GameState.Paused;
        }
        else
        {
            state = previousState;
        }
    }

    public void StartBattle()
    {
        state = GameState.Battle;
        
        //StartCoroutine(FadeIn());

        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        //StartCoroutine(FadeOut());

        var playerParty = playerController.GetComponent<PlayerParty>();
        var wildEncounter = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildEncounter();

        battleSystem.StartBattle(playerParty, wildEncounter);
    }

    public IEnumerator FadeIn()
    {
        yield return fader.FadeIn(2f);
    }

    public IEnumerator FadeOut()
    {
        yield return fader.FadeOut(2f);
    }

    BossController boss;
    public void StartBossBattle(BossController boss)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.boss = boss;
        var playerParty = playerController.GetComponent<PlayerParty>();
        var bossParty = boss.GetComponent<PlayerParty>();

        battleSystem.StartBossBattle(playerParty, bossParty);
    }

    public void OnEnterBossView(BossController boss)
    {
        state = GameState.Cutscene;
        StartCoroutine(boss.TriggerBossBattle(playerController));
    }

    void EndBattle(bool won)
    {
        if(boss != null && won == true)
        {
            boss.BattleLost();
            boss = null;
        }

        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
            if(Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action onSelected = () =>
            {
                // Show summary
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                menuController.OpenMenu();
                state = GameState.Menu;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                menuController.OpenMenu();
                state = GameState.Menu;
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }

    void OnMenuSelected(int selectedItem)
    {
        if(selectedItem == 0)
        {
            //Party
            partyScreen.gameObject.SetActive(true);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 1)
        {
            //Items
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Bag;
        }

        //state = GameState.FreeRoam;
    }
}
