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
        get { return Mathf.FloorToInt((2 * Base.HP * Level) / 100f) + 10 + Level; }
    }

    public int MP
    {
        get { return Mathf.FloorToInt((Base.MP * Level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }

    public DamageDetails TakeDamage(Move move, Character attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 5.0f)
        {
            critical = 2f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type);

        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = critical,
            Fainted = false
        };

        float modifiers = Random.Range(.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.RegAttack / RegDefense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float Type { get; set; }
}
