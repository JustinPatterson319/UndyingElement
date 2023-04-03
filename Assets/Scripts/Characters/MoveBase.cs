using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Move", menuName = "Character/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] CharacterElement type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] bool alwaysHits;
    [SerializeField] int magCost;
    [SerializeField] int priority;
    [SerializeField] string attackAnimationName;
    [SerializeField] Color attackColor;
    [SerializeField] AudioClip attackSound;
    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaryEffects;
    [SerializeField] MoveTarget target;

    public AudioClip AttackSound
    {
        get { return attackSound; }
    }

    public Color AttackColor
    {
        get { return attackColor; }
    }

    public string AttackAnimationName
    {
        get { return attackAnimationName; }
    }

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public CharacterElement Type
    {
        get { return type; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public bool AlwaysHits
    {
        get { return alwaysHits; }
    }

    public int MagCost
    {
        get { return magCost; }
    }

    public int Priority
    {
        get { return priority; }
    }

    public MoveCategory Category
    {
        get { return category; }
    }

    public MoveEffects Effects
    {
        get
        {
            return effects;
        }
    }

    public List<SecondaryEffects> SecondaryEffects
    {
        get
        {
            return secondaryEffects;
        }
    }

    public MoveTarget Target
    {
        get
        {
            return target;
        }
    }
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;

    public List<StatBoost> Boosts
    {
        get
        {
            return boosts;
        }
    }

    public ConditionID Status
    {
        get
        {
            return status;
        }
    }
}

[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;

    public int Chance
    {
        get
        {
            return chance;
        }
    }

    public MoveTarget Target
    {
        get
        {
            return target;
        }
    }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum MoveCategory
{
    Physical, Special, Status
}

public enum MoveTarget
{
    Foe, Self
}
