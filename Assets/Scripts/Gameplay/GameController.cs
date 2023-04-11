using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene }

public class GameController : MonoBehaviour
{
    GameState state;
    public static GameController Instance { get; private set; }

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    public void Awake()
    {
        Instance = this;
        ConditionsDB.Init();
    }

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.OnEnterBossView += (Collider2D bossCollider) =>
        {
            var boss = bossCollider.GetComponentInParent<BossController>();
            if (boss != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(boss.TriggerBossBattle(playerController));
            }
        };

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
    }

    void StartBattle()
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
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
