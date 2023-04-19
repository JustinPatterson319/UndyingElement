using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFov : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Characters.Animator.IsMoving = false;
        GameController.Instance.OnEnterBossView(GetComponentInParent<BossController>());
    }
}
