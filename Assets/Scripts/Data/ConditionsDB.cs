using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.bleed,
            new Condition()
            {
                Name = "Bleed",
                StartMessage = "is starting to bleed out",
                OnAfterTurn = (Character character) =>
                {
                    character.UpdateHP(character.HP / 8);
                    character.StatusChanges.Enqueue($"{character.Base.Name} is damaged by bleeding!");
                }
            }
        },
        {
            ConditionID.venom,
            new Condition()
            {
                Name = "Venom",
                StartMessage = "is cursed with venom",
                OnAfterTurn = (Character character) =>
                {
                    character.UpdateMP(character.MP / 8);
                    character.StatusChanges.Enqueue($"{character.Base.Name} lost mana due to venom!");
                }
            }
        },
        {
            ConditionID.cure,
            new Condition()
            {
                Name = "Cure",
                StartMessage = "begins to replenish health",
                OnAfterTurn = (Character character) =>
                {
                    character.UpdateHp(character.HP / 8);
                    character.StatusChanges.Enqueue($"{character.Base.Name}'s health restored by cure!");
                }
            }
        },
        {
            ConditionID.charge,
            new Condition()
            {
                Name = "Charge",
                StartMessage = "is charging magic",
                OnAfterTurn = (Character character) =>
                {
                    character.UpdateMp(character.MP / 8);
                    character.StatusChanges.Enqueue($"{character.Base.Name} charged up more mana!");
                }
            }
        },
        {
            ConditionID.shock,
            new Condition()
            {
                Name = "Shock",
                StartMessage = "is jolted by shock",
                OnBeforeMove = (Character character) =>
                {
                    if (Random.Range(1, 5) == 1)
                    {
                        character.StatusChanges.Enqueue($"{character.Base.Name}'s shock prevents movement!");
                        return false;
                    }

                    return true;
                }
            }
        }
    };
}


//Status effect descriptions
//Bleed dcreases health
//Venom decreases mana
//Cure heals health
//Charge heals mana
public enum ConditionID
{ 
    none, bleed, venom, cure, charge, shock
}

