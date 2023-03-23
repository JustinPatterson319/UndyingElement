using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Character/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] CharacterElement type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int magCost;
    [SerializeField] Sprite[] sprite;

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

    public int MagCost
    {
        get { return magCost; }
    }

    public Sprite[] Sprite
    {
        get { return sprite; }
    }

    public bool IsSpecial
    {
        get
        {
            if (type == CharacterElement.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
