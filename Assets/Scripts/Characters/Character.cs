using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public BaseCharacter Base { get; set; }
    public int Level { get; set; }

    public int currentHP { get; set; }
    public int currentMP { get; set; }

    //moves
    public List<Move> Moves { get; set; }

    public Character(BaseCharacter cBase, int cLevel)
    {
        Base = cBase;
        Level = cLevel;
        currentHP = HP;
        currentMP = MP;

        //generate moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 6)
            {
                break;
            }
        }
    }

    public int RegAttack
    {
        get { return Mathf.FloorToInt((Base.RegAttack * Level) / 100f) + 5; }
    }

    public int MagAttack
    {
        get { return Mathf.FloorToInt((Base.MagAttack * Level) / 100f) + 5; }
    }

    public int RegDefense
    {
        get { return Mathf.FloorToInt((Base.RegDefense * Level) / 100f) + 5; }
    }

    public int MagDefense
    {
        get { return Mathf.FloorToInt((Base.MagDefense * Level) / 100f) + 5; }
    }

    public int HP
    {
        get { return Mathf.FloorToInt((Base.HP * Level) / 100f) + 10; }
    }

    public int MP
    {
        get { return Mathf.FloorToInt((Base.MP * Level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
}
