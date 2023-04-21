using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new recovery item")]
public class RecoveryItems : ItemBase
{
    [Header("HP")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHP;

    [Header("MP")]
    [SerializeField] int mpAmount;
    [SerializeField] bool restoreMaxMP;

    [Header("Status")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;

    [Header("Revive")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;


    public override bool Use(Character character)
    {
        //Revive
        if(revive || maxRevive)
        {
            if (character.currentHP > 0)
            {
                return false;
            }
            if(revive)
            {
                character.UpdateHp(character.HP / 2);
            }
            else if (maxRevive)
            {
                character.UpdateHp(character.HP);
            }
            character.CureStatus();
            return true;
        }

        //nothing else can be used on a fallen party member
        if(character.currentHP == 0)
        {
            return false;
        }

        //Potions
        if(restoreMaxHP || hpAmount > 0)
        {
            if(character.currentHP == character.HP)
            {
                return false;
            }

            if (restoreMaxHP)
            {
                character.UpdateHp(character.HP);
            }
            else
            {
                character.UpdateHp(hpAmount);
            }
            return true;
        }

        //Recover from status effects
        if(recoverAllStatus || status != ConditionID.none)
        {
            if (character.Status == null)
            {
                return false;
            }

            if(recoverAllStatus)
            {
                character.CureStatus();
            }
            else
            {
                if (character.Status.Id == status)
                {
                    character.CureStatus();
                }
                else
                {
                    return false;
                }
            }
        }

        //Restore MP
        if (restoreMaxMP || mpAmount > 0)
        {
            if (character.currentMP == character.MP)
            {
                return false;
            }

            if (restoreMaxMP)
            {
                character.UpdateMp(character.MP);
            }
            else
            {
                character.UpdateMp(mpAmount);
            }
            return true;
        }

        return false;
    }
}
