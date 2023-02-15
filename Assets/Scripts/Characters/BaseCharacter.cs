using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Create new character")]
public class BaseCharacter : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite battleSprite;
    [SerializeField] Sprite faceSprite;
    [SerializeField] Sprite elementSprite;

    [SerializeField] CharacterElement type;

    //Stats
    [SerializeField] int hp;
    [SerializeField] int mp;
    [SerializeField] int regAttack;
    [SerializeField] int magAttack;
    [SerializeField] int regDefense;
    [SerializeField] int magDefense;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public string GetName()
    {
        return name;
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

    public Sprite BattleSprite
    {
        get { return battleSprite; }
    }

    public Sprite FaceSprite
    {
        get { return faceSprite; }
    }

    public Sprite ElementSprite
    {
        get { return elementSprite; }
    }

    public int HP
    {
        get { return hp; }
    }

    public int MP
    {
        get { return mp; }
    }

    public int RegAttack
    {
        get { return regAttack; }
    }

    public int MagAttack
    {
        get { return magAttack; }
    }

    public int RegDefense
    {
        get { return regDefense; }
    }

    public int MagDefense
    {
        get { return magDefense; }
    }

    public int Speed
    {
        get { return Speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

public enum CharacterElement
{
    None,
    Neutral,
    Flame,
    Lightning,
    Mind,
    Nature,
    Ice,
    Earth,
    Wind,
    Aqua,
    Light,
    Darkness
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }
}
