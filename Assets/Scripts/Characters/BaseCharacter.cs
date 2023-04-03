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
    [SerializeField] Sprite partySprite;
    [SerializeField] Sprite faceSprite;
    [SerializeField] Sprite elementSprite;
    [SerializeField] Sprite fleeSprite;

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

    public Sprite FleeSprite
    {
        get { return fleeSprite; }
    }

    public Sprite PartySprite
    {
        get { return partySprite; }
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

    public int Strength
    {
        get { return regAttack; }
    }

    public int Magic
    {
        get { return magAttack; }
    }

    public int Defense
    {
        get { return regDefense; }
    }

    public int Warding
    {
        get { return magDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

public enum CharacterElement
{
    Non,
    None,
    Flm,
    Ltng,
    Mind,
    Ntre,
    Ice,
    Erth,
    Wind,
    Aqua,
    Lght,
    Dark
}

public enum Stat
{
    Strength,
    Magic,
    Defense,
    Warding,
    Speed,
    //Not actual stats but can change accuracy
    Accuracy,
    Evasion
}

public class TypeChart
{
    static float[][] chart =
    {
      //                      Fl    Lg    Md    Nt    Ic    Er    Wd    Aq    Lt    Dk
      /*Flme*/ new float[] { 0.5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f },
      /*Ltng*/ new float[] { 1.0f, 0.5f, 1.0f, 0.5f, 1.0f, 0.5f, 2.0f, 2.0f, 0.5f, 1.0f },
      /*Mind*/ new float[] { 0.5f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f },
      /*Ntre*/ new float[] { 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 2.0f, 0.5f, 1.0f },
      /* Ice*/ new float[] { 1.0f, 2.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f },
      /*Erth*/ new float[] { 1.0f, 2.0f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f },
      /*Wind*/ new float[] { 2.0f, 1.0f, 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f },
      /*Aqua*/ new float[] { 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 0.5f },
      /*Lght*/ new float[] { 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 2.0f },
      /*Dark*/ new float[] { 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f },
    };

    public static float GetEffectiveness(CharacterElement attackType, CharacterElement defenseType)
    {
        if (attackType == CharacterElement.None || defenseType == CharacterElement.None)
        {
            return 1;
        }

        int row = (int)attackType - 2;
        int col = (int)defenseType - 2;
        return chart[row][col];
    }
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
