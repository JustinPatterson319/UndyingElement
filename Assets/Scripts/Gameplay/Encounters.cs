using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 4)
        {
            player.Characters.Animator.IsMoving = false;
            GameController.Instance.StartBattle();
        }
    }
}
