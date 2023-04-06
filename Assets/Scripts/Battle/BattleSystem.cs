using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum BattleState { Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, BattleOver}
public enum BattleAction { Move, SwitchCharacter, UseItem, Run}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    //[SerializeField] BattleHUD playerHUD;

    [SerializeField] BattleUnit enemyUnit;
    //[SerializeField] BattleHUD enemyHUD;

    [SerializeField] BatlleDialogue dialogBox;

    [SerializeField] PartyScreen partyScreen;

    [SerializeField] Animator playerHitBox;
    [SerializeField] Animator enemyHitBox;
    [SerializeField] Image playerHit;
    [SerializeField] Image enemyHit;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip enemyDefeatSound;
    [SerializeField] AudioClip playerDefeatSound;

    public event Action<bool> OnBattleOver;

    BattleState state;
    BattleState? prevState;
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
        AudioSource audio = GetComponent<AudioSource>();
        playerUnit.Setup(playerParty.GetHealthy());
        enemyUnit.Setup(wildEncounter);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Character.Moves);

        yield return dialogBox.TypeDialog($"Enemy {enemyUnit.Character.Base.name} stands in the way!");

        StartCoroutine(ActionSelection());
    }


    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Characters.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);
    }

    public IEnumerator ActionSelection()
    {
        StartCoroutine(dialogBox.TypeDialog("Choose an action..."));
        yield return new WaitForSeconds(.7f);
        state = BattleState.ActionSelection;
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Characters);
        partyScreen.gameObject.SetActive(true);
        partyScreen.SetMessageText("Select a party member...");
    }

    public IEnumerator MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        StartCoroutine(dialogBox.TypeDialog("Choose an attack..."));
        yield return new WaitForSeconds(.7f);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;
        if (playerAction == BattleAction.Move)
        {
            playerUnit.Character.CurrentMove = playerUnit.Character.Moves[currentAction];
            enemyUnit.Character.CurrentMove = enemyUnit.Character.GetRandomMove();

            int playerMovePriority = playerUnit.Character.CurrentMove.Base.Priority;
            int enemyMovePriority = enemyUnit.Character.CurrentMove.Base.Priority;

            //check who goes first
            bool playerGoesFirst = true;
            if(enemyMovePriority > playerMovePriority)
            {
                playerGoesFirst = false;
            }
            else if (enemyMovePriority == playerMovePriority)
            {
                playerGoesFirst = playerUnit.Character.Speed >= enemyUnit.Character.Speed;
            }

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondCharacter = secondUnit.Character;

            //First turn
            if(firstUnit == playerUnit)
            {
                yield return RunMove(firstUnit, secondUnit, firstUnit.Character.CurrentMove, enemyHitBox, enemyHit, playerHitBox, playerHit);
            }
            else
            {
                yield return RunMove(firstUnit, secondUnit, firstUnit.Character.CurrentMove, playerHitBox, playerHit, enemyHitBox, enemyHit);
            }

            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver) yield break;

            if (secondCharacter.currentHP > 0)
            {
                //Second Turn
                if (secondUnit == playerUnit)
                {
                    yield return RunMove(secondUnit, firstUnit, secondUnit.Character.CurrentMove, enemyHitBox, enemyHit, playerHitBox, playerHit);
                }
                else
                {
                    yield return RunMove(secondUnit, firstUnit, secondUnit.Character.CurrentMove, playerHitBox, playerHit, enemyHitBox, enemyHit);
                }

                yield return RunAfterTurn(secondUnit);
                if (state == BattleState.BattleOver) yield break;
            }
        }
        else
        {
            if (playerAction == BattleAction.SwitchCharacter)
            {
                var selectedCharacter = playerParty.Characters[currentMember];
                state = BattleState.Busy;
                yield return SwitchCharacter(selectedCharacter);
            }

            //Enemy Turn
            var enemyMove = enemyUnit.Character.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove, playerHitBox, playerHit, enemyHitBox, enemyHit);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }

        if (state != BattleState.BattleOver)
        {
            StartCoroutine(ActionSelection());
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move, Animator targetHitBox, Image targetHit, Animator sourceHitBox, Image sourceHit)
    {
        bool canRunMove = sourceUnit.Character.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Character);
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Character);

        if (sourceUnit == playerUnit)
        {
            yield return dialogBox.TypeDialog($"{move.Base.Name}");
        }
        else
        {
            yield return dialogBox.TypeDialog($"Enemy attacks with {move.Base.Name}!");
        }


        //Move accuracy check
        if (CheckIfMoveHits(move, sourceUnit.Character, targetUnit.Character))
        {
            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffects(move.Base.Effects, sourceUnit.Character, targetUnit.Character, sourceHitBox, targetHitBox, targetUnit, sourceUnit, targetHit, sourceHit, move.Base.Target, move, false);
            }
            else
            {
                PlaySoundsAndAnimations(move, targetHit, targetHitBox, targetUnit, true);
                var damageDetails = targetUnit.Character.TakeDamage(move, sourceUnit.Character);
                sourceUnit.Character.currentMP -= move.Base.MagCost;
                yield return targetUnit.Hud.UpdateHP();
                yield return sourceUnit.Hud.UpdateMP();
                yield return ShowDamageDetails(damageDetails);
            }

            //run any possible secondary effects of a move
            if (move.Base.SecondaryEffects != null && move.Base.SecondaryEffects.Count > 0 && targetUnit.Character.currentHP > 0)
            {
                foreach (var secondary in move.Base.SecondaryEffects)
                {
                    var rnd = UnityEngine.Random.Range(1, 101);
                    if (rnd <= secondary.Chance)
                    {
                        yield return RunMoveEffects(secondary, sourceUnit.Character, targetUnit.Character, sourceHitBox, targetHitBox, targetUnit, sourceUnit, targetHit, sourceHit, secondary.Target, move, true);
                    }
                }
            }

            if (targetUnit.Character.currentHP <= 0)
            {
                if (targetUnit == playerUnit)
                {
                    GetComponent<AudioSource>().clip = playerDefeatSound;
                    GetComponent<AudioSource>().Play(0);
                    targetUnit.PlayRetreatAnimation();
                    yield return dialogBox.TypeDialog($"{targetUnit.Character.Base.name} retreats!");
                }
                else
                {
                    GetComponent<AudioSource>().clip = enemyDefeatSound;
                    GetComponent<AudioSource>().Play(0);
                    targetUnit.PlayFaintAnimation();
                    yield return dialogBox.TypeDialog($"Enemy {targetUnit.Character.Base.name} defeated!");
                }

                yield return new WaitForSeconds(2f);
                CheckForBattleOver(targetUnit);
            }
        }
        else
        {
            PlaySoundsAndAnimations(move, targetHit, targetHitBox, targetUnit, false);
            targetUnit.PlayDodgeAnimation();
            yield return new WaitForSeconds(1f);
            yield return dialogBox.TypeDialog($"The attack was dodged!");
        }
    }

    void PlaySoundsAndAnimations(Move move, Image hit, Animator hitbox, BattleUnit target, bool doesHit)
    {
        hit.color = move.Base.AttackColor;
        hitbox.Play(move.Base.AttackAnimationName);
        GetComponent<AudioSource>().clip = move.Base.AttackSound;
        GetComponent<AudioSource>().Play(0);
        if(doesHit)
        {
            target.PlayHitAnimation(move);
        }
    }

    IEnumerator RunMoveEffects(MoveEffects effects, Character source, Character target, Animator sourceHitBox, Animator targetHitBox, BattleUnit targetUnit, BattleUnit sourceUnit, Image targetHit, Image sourceHit, MoveTarget moveTarget, Move move, bool isSecondary)
    {

        //Stat Boosting
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
            {
                if (!isSecondary)
                {
                    PlaySoundsAndAnimations(move, sourceHit, sourceHitBox, sourceUnit, false);
                    source.currentMP -= move.Base.MagCost;
                    yield return sourceUnit.Hud.UpdateMP();
                }

                source.ApplyBoosts(effects.Boosts);
            }
            else
            {
                if (!isSecondary)
                {
                    PlaySoundsAndAnimations(move, targetHit, targetHitBox, targetUnit, true);
                    source.currentMP -= move.Base.MagCost;
                    yield return sourceUnit.Hud.UpdateMP();
                }

                target.ApplyBoosts(effects.Boosts);
            }
        }

        //Status Condition
        if (effects.Status != ConditionID.none)
        {
            if (moveTarget == MoveTarget.Foe)
            {
                target.SetStatus(effects.Status);
            }
            else
            {
                source.SetStatus(effects.Status);
            }
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == BattleState.BattleOver) yield break;
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        //status like bleed or venom will hurt after turn ends
        sourceUnit.Character.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Character);
        yield return sourceUnit.Hud.UpdateHP();
        yield return sourceUnit.Hud.UpdateMP();

        if (sourceUnit.Character.currentHP <= 0)
        {
            if (sourceUnit == playerUnit)
            {
                GetComponent<AudioSource>().clip = playerDefeatSound;
                GetComponent<AudioSource>().Play(0);
                sourceUnit.PlayRetreatAnimation();
                yield return dialogBox.TypeDialog($"{sourceUnit.Character.Base.name} retreats!");
            }
            else
            {
                GetComponent<AudioSource>().clip = enemyDefeatSound;
                GetComponent<AudioSource>().Play(0);
                sourceUnit.PlayFaintAnimation();
                yield return dialogBox.TypeDialog($"Enemy {sourceUnit.Character.Base.name} defeated!");
            }
            yield return new WaitForSeconds(2f);
            CheckForBattleOver(sourceUnit);
        }
    }

    bool CheckIfMoveHits(Move move, Character source, Character target)
    {
        if (move.Base.AlwaysHits)
        {
            return true;
        }

        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        if (accuracy > 0)
        {
            moveAccuracy *= boostValues[accuracy];
        }
        else
        {
            moveAccuracy /= boostValues[-accuracy];
        }

        if (evasion > 0)
        {
            moveAccuracy /= boostValues[evasion];
        }
        else
        {
            moveAccuracy *= boostValues[-evasion];
        }

        return UnityEngine.Random.Range(1, 100) <= moveAccuracy;
    }

    IEnumerator ShowStatusChanges(Character character)
    {
        while (character.StatusChanges.Count > 0)
        {
            var message = character.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextCharacter = playerParty.GetHealthy();
            if (nextCharacter != null)
            {
                OpenPartyScreen();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
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
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
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
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 0 || currentAction == 2)
            {
                currentAction++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 1 || currentAction == 3)
            {
                currentAction--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 0 || currentAction == 1)
            {
                currentAction = currentAction + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 2 || currentAction == 3)
            {
                currentAction = currentAction - 2;
            }
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!dialogBox.isTyping)
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);

                if (currentAction == 0)
                {
                    currentAction = 0;
                    //attack
                    StartCoroutine(MoveSelection());
                }
                if (currentAction == 1)
                {
                    currentAction = 0;
                    //swap
                    dialogBox.EnableActionSelector(false);
                    prevState = state;
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
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 0 || currentAction == 2 || currentAction == 4)
            {
                currentAction++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 1 || currentAction == 3 || currentAction == 5)
            {
                currentAction--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 0 || currentAction == 1 || currentAction == 2 || currentAction == 3) 
            {
                currentAction = currentAction + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentAction == 2 || currentAction == 3 || currentAction == 4 || currentAction == 5) 
            {
                currentAction = currentAction - 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentAction, playerUnit.Character.Moves[currentAction]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!dialogBox.isTyping)
            {
                var move = playerUnit.Character.Moves[currentAction];
                if (playerUnit.Character.currentMP < move.Base.MagCost)
                {
                    return;
                }
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                StartCoroutine(RunTurns(BattleAction.Move));
                currentAction = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (!dialogBox.isTyping)
            {
                currentAction = 0;
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                StartCoroutine(ActionSelection());
            }
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 0 || currentMember == 2)
            {
                currentMember++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 1 || currentMember == 3)
            {
                currentMember--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 0 || currentMember == 1)
            {
                currentMember = currentMember + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 2 || currentMember == 3)
            {
                currentMember = currentMember - 2;
            }
        }
        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
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

            if (prevState == BattleState.ActionSelection)
            {
                prevState = null;
                StartCoroutine(RunTurns(BattleAction.SwitchCharacter));
            }
            else
            {
                state = BattleState.Busy;
                StartCoroutine(SwitchCharacter(selectedMember));
            }
        }
        else if (Input.GetKeyDown(KeyCode.X) && playerUnit.Character.currentHP > 0)
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            partyScreen.gameObject.SetActive(false);
            StartCoroutine(ActionSelection());
        }
    }

    IEnumerator SwitchCharacter(Character newCharacter)
    {
        playerUnit.Character.CureStatus();

        if (playerUnit.Character.currentHP > 0)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Character.Base.Name} retreats!");
            GetComponent<AudioSource>().clip = playerDefeatSound;
            GetComponent<AudioSource>().Play(0);
            playerUnit.PlayRetreatAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newCharacter);
        dialogBox.SetMoveNames(newCharacter.Moves);
        yield return dialogBox.TypeDialog($"{newCharacter.Base.name} steps in to defend!");

        state = BattleState.RunningTurn;
    }
}
