using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene, Paused, Menu, PartyScreen, Bag }

public class GameController : MonoBehaviour
{
    GameState state;
    GameState previousState;
    public static GameController Instance { get; private set; }

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    MenuController menuController;

    public void Awake()
    {
        Instance = this;
        ConditionsDB.Init();
        menuController = GetComponent<MenuController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        battleSystem.OnBattleOver += EndBattle;
        partyScreen.Init();

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if(state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
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
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PlayerParty>();
        var wildEncounter = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildEncounter();

        battleSystem.StartBattle(playerParty, wildEncounter);
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
