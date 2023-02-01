using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    BaseCharacter _base;
    int level;

    public int currentHP { get; set; }

    //moves
    public List<Move> Moves { get; set; }

    public Character(BaseCharacter cBase, int cLevel)
    {
        _base = cBase;
        level = cLevel;
        currentHP = _base.HP;

        //generate moves
        Moves = new List<Move>();
        foreach (var move in _base.LearnableMoves)
        {
            if (move.Level <= level)
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
        get { return Mathf.FloorToInt((_base.RegAttack * level) / 100f) + 5; }
    }

    public int MagAttack
    {
        get { return Mathf.FloorToInt((_base.MagAttack * level) / 100f) + 5; }
    }

    public int RegDefense
    {
        get { return Mathf.FloorToInt((_base.RegDefense * level) / 100f) + 5; }
    }

    public int MagDefense
    {
        get { return Mathf.FloorToInt((_base.MagDefense * level) / 100f) + 5; }
    }

    public int HP
    {
        get { return Mathf.FloorToInt((_base.HP * level) / 100f) + 10; }
    }

    public int MP
    {
        get { return Mathf.FloorToInt((_base.MP * level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
    }
}
