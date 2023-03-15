using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;

    [SerializeField] BatlleDialogue dialogBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Character);

        enemyUnit.Setup();
        enemyHUD.SetData(enemyUnit.Character);

        dialogBox.SetMoveNames(playerUnit.Character.Moves);

        yield return dialogBox.TypeDialog($"Enemy {enemyUnit.Character.Base.name} stands in the way!");

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choosing an action..."));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        StartCoroutine(dialogBox.TypeDialog("Choosing an attack..."));
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
        yield return enemyHUD.UpdateHP();
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
        yield return playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Character.Base.Name} defeated!");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
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
                PlayerMove();
            }
            if (currentAction == 1)
            {
                currentAction = 0;
                //swap
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
    }
}
