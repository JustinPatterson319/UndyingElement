using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Character
{
    [SerializeField] BaseCharacter _base;
    [SerializeField] int level;


    public BaseCharacter Base 
    { 
        get
        {
            return _base;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
    }

    public int Exp { get; set; }
    public int currentHP { get; set; }
    public int currentMP { get; set; }
    public CharacterElement currentElement { get; set; }
    public Sprite currentElementSprite { get; set; }

    //moves
    public List<Move> Moves { get; set; }

    public Move CurrentMove { get; set; }

    public Dictionary<Stat, int> Stats { get; private set; }

    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public Condition Status { get; private set; }

    public int StatusTime { get; set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();

    public event System.Action OnStatusChanged;
    public event System.Action OnHPChanged;
    public event System.Action OnMPChanged;
    public event System.Action OnElementChanged;

    public void Init()
    {
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

        Exp = Base.GetExpForLevel(Level);

        CalculateStats();
        currentHP = HP;
        currentMP = MP;
        

        ResetStatBoost();
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Strength, 0},
            {Stat.Magic, 0},
            {Stat.Defense, 0},
            {Stat.Warding, 0},
            {Stat.Speed, 0},
            {Stat.Accuracy, 0},
            {Stat.Evasion, 0},
        };
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Strength, Mathf.FloorToInt((Base.Strength * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.Magic, Mathf.FloorToInt((Base.Magic * Level) / 100f) + 5);
        Stats.Add(Stat.Warding, Mathf.FloorToInt((Base.Warding * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        HP = Mathf.FloorToInt((2 * Base.HP * Level) / 100f) + 10 + Level;
        MP = Mathf.FloorToInt((2 * Base.MP * Level) / 100f) + Level;
        currentElement = Base.Type;
        currentElementSprite = Base.ElementSprite;
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        //stat boost
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }

        return statVal;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if (boost > 0)
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} improved!");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} decreased!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }

    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BoostStatsAfterLevelUp()
    {
        var oldMaxHp = HP;
        CalculateStats();
        var diff = HP - oldMaxHp;

        UpdateHP(-diff);

        var oldMaxMp = MP;
        CalculateStats();
        var diff2 = MP - oldMaxMp;

        UpdateMP(-diff2);
    }

    public int Strength
    {
        get { return GetStat(Stat.Strength); }
    }

    public int Magic
    {
        get { return GetStat(Stat.Magic); }
    }

    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }

    public int Warding
    {
        get { return GetStat(Stat.Warding); }
    }

    public int HP
    {
        get;
        private set;
    }

    public int MP
    {
        get;
        private set;
    }

    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    public DamageDetails TakeDamage(Move move, Character attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 5.0f)
        {
            critical = 2f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.currentElement);

        var damageDetails = new DamageDetails()
        {
            Type = type,
            Critical = critical,
            Fainted = false
        };

        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.Magic : attacker.Strength;
        float defense = (move.Base.Category == MoveCategory.Special) ? Warding : Defense;

        float modifiers = Random.Range(.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);

        return damageDetails;
    }

    public void LoseMp(Move move)
    {
        currentMP -= move.Base.MagCost;
        OnMPChanged?.Invoke();
    }

    public void UpdateHP(int damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, HP);
        OnHPChanged?.Invoke();
    }

    public void UpdateMP(int damage)
    {
        currentMP = Mathf.Clamp(currentMP - damage, 0, MP);
        OnMPChanged?.Invoke();
    }

    public void UpdateHp(int damage)
    {
        currentHP = Mathf.Clamp(currentHP + damage, 0, HP);
        OnHPChanged?.Invoke();
    }

    public void UpdateMp(int damage)
    {
        currentMP = Mathf.Clamp(currentMP + damage, 0, MP);
        OnMPChanged?.Invoke();
    }

    public void UpdateElement(CharacterElement element, Sprite elementSprite)
    {
        currentElement = element;
        currentElementSprite = elementSprite;
        OnElementChanged?.Invoke();
    }

    public void SetStatus(ConditionID conditionId)
    {
        if (Status != null)
        {
            return;
        }

        Status = ConditionsDB.Conditions[conditionId];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}!");
        OnStatusChanged?.Invoke();
    }

    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public Move GetRandomMove()
    {
        var movesWithMag = Moves.Where(x => x.Base.MagCost <= currentMP).ToList();

        var matchingMoves = Moves.Where(x => x.Base.Type == this.currentElement).ToList();

        movesWithMag.AddRange(matchingMoves);

        int r = Random.Range(0, movesWithMag.Count);
        return movesWithMag[r];
    }

    public bool OnBeforeMove()
    {
        if (Status?.OnBeforeMove != null)
        {
            return Status.OnBeforeMove(this); 
        }
        return true;
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
    }

    public void OnBattleOver()
    {
        CureStatus();
        ResetStatBoost();
    }

    public void LearnMove(MoveBase moveToLearn)
    {
        if (Moves.Count > 6)
        {
            return;
        }
        Moves.Add(new Move(moveToLearn));
    }

    public bool HasMove(MoveBase moveToCheck)
    {
        return Moves.Count(m => m.Base == moveToCheck) > 0;
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float Type { get; set; }
}
