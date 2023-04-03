using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach(var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

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
                    int removeStatus = Random.Range(1,6);
                    if(removeStatus == 1)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name} stopped the bleeding!");
                    }
                    else
                    {
                        character.UpdateHP(character.HP / 8);
                        character.StatusChanges.Enqueue($"{character.Base.Name} is damaged by bleeding!");
                    }
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
                    int removeStatus = Random.Range(1,6);
                    if(removeStatus == 1)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name} is no longer venomized!");
                    }
                    else
                    {
                        character.UpdateMP(character.MP / 8);
                        character.StatusChanges.Enqueue($"{character.Base.Name} lost mana due to venom!");
                    }
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
                    int removeStatus = Random.Range(1,6);
                    if(removeStatus == 1)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name} is no longer curing!");
                    }
                    else
                    {
                        character.UpdateHp(character.HP / 8);
                        character.StatusChanges.Enqueue($"{character.Base.Name}'s health restored by cure!");
                    }
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
                    int removeStatus = Random.Range(1,6);
                    if(removeStatus == 1)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name} is no longer charging!");
                    }
                    else
                    {
                        character.UpdateMp(character.MP / 8);
                        character.StatusChanges.Enqueue($"{character.Base.Name} charged up more mana!");
                    }
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
                    int canMove = Random.Range(1,5);
                    if (canMove == 1 || canMove == 2)
                    {
                        character.StatusChanges.Enqueue($"{character.Base.Name}'s shock prevents movement!");
                        return false;
                    }
                    else if (canMove == 3)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name}'s shook off the shock!");
                        return true;
                    }

                    return true;
                }
            }
        },
        {
            ConditionID.chill,
            new Condition()
            {
                Name = "Chill",
                StartMessage = "is freezing from chill",
                OnBeforeMove = (Character character) =>
                {
                    if (Random.Range(1, 5) == 1)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name} is no longer chilled!");
                        return true;
                    }
                    else
                    {
                        character.StatusChanges.Enqueue($"{character.Base.Name} is too cold to move!");
                        return false;
                    }
                }
            }
        },
        {
            ConditionID.slumber,
            new Condition()
            {
                Name = "Slumber",
                StartMessage = "is drifting into slumber",
                OnStart = (Character character) =>
                {
                    //slumber for 1-3turns
                    character.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {character.StatusTime} turns");
                },
                OnBeforeMove = (Character character) =>
                {
                    if(character.StatusTime <= 0)
                    {
                        character.CureStatus();
                        character.StatusChanges.Enqueue($"{character.Base.Name} no longer tired!");
                        return true;
                    }
                    character.StatusTime--;
                    character.StatusChanges.Enqueue($"{character.Base.Name} is dozing off!");
                    return false;
                }
            }
        }
    };
}


//Status effect descriptions
//Bleed dcreases health //RED HEALTH BAR
//Venom decreases mana //PURPLE MAGIC BAR
//Cure heals health //LIGHT GREEN HEALTH BAR
//Charge heals mana //LIGHT BLUE MAGIC BAR
//Shock may prevent actions //YELLOW HEALTH BAR
//Chill prevents actions //LIGHT BLUE HEALTH BAR
//Slumber prevents actions //PINK HEALTH BAR
public enum ConditionID
{ 
    none, bleed, venom, cure, charge, shock, chill, slumber
}

