using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new tome item")]
public class TomeItems : ItemBase
{
    [SerializeField] MoveBase move;
    [SerializeField] bool isHM = true;

    public override bool Use(Character character)
    {
        //Learning move handled from inventory UI, if learned return true
        return character.HasMove(move);
    }

    public override bool IsReusable => true;

    public override bool CanUseInBattle => false;

    public MoveBase Move => move;
    public bool IsHM => isHM;
}
