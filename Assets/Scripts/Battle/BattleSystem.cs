using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;

    [SerializeField] BatlleDialogue dialogBox;

    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMember;

    PlayerParty playerParty;
    Character wildEncounter;

    public void StartBattle(PlayerParty playerParty, Character wildEncounter)
    {
        this.playerParty = playerParty;
        this.wildEncounter = wildEncounter;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthy());
        playerHUD.SetData(playerUnit.Character);

        enemyUnit.Setup(wildEncounter);
        enemyHUD.SetData(enemyUnit.Character);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Character.Moves);

        yield return dialogBox.TypeDialog($"Enemy {enemyUnit.Character.Base.name} stands in the way!");

        StartCoroutine(PlayerAction());
    }

    public IEnumerator PlayerAction()
    {
        StartCoroutine(dialogBox.TypeDialog("Choose an action..."));
        yield return new WaitForSeconds(.7f);
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Characters);
        partyScreen.gameObject.SetActive(true);
    }

    public IEnumerator PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        StartCoroutine(dialogBox.TypeDialog("Choose an attack..."));
        yield return new WaitForSeconds(.7f);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        var move = playerUnit.Character.Moves[currentAction];
        yield return dialogBox.TypeDialog($"{move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation(move);
        var damageDetails = enemyUnit.Character.TakeDamage(move, playerUnit.Character);
        playerUnit.Character.currentMP -= move.Base.MagCost;
        yield return enemyHUD.UpdateHP();
        yield return playerHUD.UpdateMP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            enemyUnit.PlayFaintAnimation();
            yield return dialogBox.TypeDialog($"Enemy {enemyUnit.Character.Base.name} defeated!");

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Character.GetRandomMove();
        yield return dialogBox.TypeDialog($"Enemy uses {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation(move);
        var damageDetails = playerUnit.Character.TakeDamage(move, enemyUnit.Character);
        enemyUnit.Character.currentMP -= move.Base.MagCost;
        yield return playerHUD.UpdateHP();
        yield return enemyHUD.UpdateMP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Character.Base.Name} defeated!");
            playerUnit.PlayRetreatAnimation();

            yield return new WaitForSeconds(2f);
            
            var nextCharacter = playerParty.GetHealthy();
            if (nextCharacter != null)
            {
                OpenPartyScreen();
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            StartCoroutine(PlayerAction());
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
        }

        if(damageDetails.Type > 1f)
        {
            yield return dialogBox.TypeDialog("It exploited a weakness!");
        }

        if (damageDetails.Type < 1f)
        {
            yield return dialogBox.TypeDialog("Attack resisted!");
        }
    }


    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction == 0 || currentAction == 2)
            {
                currentAction++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction == 1 || currentAction == 3)
            {
                currentAction--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction == 0 || currentAction == 1)
            {
                currentAction = currentAction + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction == 2 || currentAction == 3)
            {
                currentAction = currentAction - 2;
            }
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                currentAction = 0;
                //attack
                StartCoroutine(PlayerMove());
            }
            if (currentAction == 1)
            {
                currentAction = 0;
                //swap
                dialogBox.EnableActionSelector(false);
                OpenPartyScreen();
            }
            if (currentAction == 2)
            {
                currentAction = 0;
                //item
            }
            if (currentAction == 3)
            {
                currentAction = 0;
                //flee
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction == 0 || currentAction == 2 || currentAction == 4)
            {
                currentAction++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction == 1 || currentAction == 3 || currentAction == 5)
            {
                currentAction--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction == 0 || currentAction == 1 || currentAction == 2 || currentAction == 3) 
            {
                currentAction = currentAction + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction == 2 || currentAction == 3 || currentAction == 4 || currentAction == 5) 
            {
                currentAction = currentAction - 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentAction, playerUnit.Character.Moves[currentAction]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
            currentAction = 0;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerAction());
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMember == 0 || currentMember == 2)
            {
                currentMember++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMember == 1 || currentMember == 3)
            {
                currentMember--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMember == 0 || currentMember == 1)
            {
                currentMember = currentMember + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMember == 2 || currentMember == 3)
            {
                currentMember = currentMember - 2;
            }
        }
        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Characters[currentMember];
            if (selectedMember.currentHP <= 0)
            {
                partyScreen.SetMessageText("That member is too weak to fight!");
                return;
            }
            if (selectedMember == playerUnit.Character)
            {
                partyScreen.SetMessageText("That member is already fighting!");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchCharacter(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            StartCoroutine(PlayerAction());
        }
    }

    IEnumerator SwitchCharacter(Character newCharacter)
    {
        if (playerUnit.Character.currentHP > 0)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Character.Base.Name} retreats!");
            playerUnit.PlayRetreatAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newCharacter);
        playerHUD.SetData(newCharacter);
        dialogBox.SetMoveNames(newCharacter.Moves);

        yield return dialogBox.TypeDialog($"{newCharacter.Base.name} steps in to defend!");

        StartCoroutine(EnemyMove());
    }
}
