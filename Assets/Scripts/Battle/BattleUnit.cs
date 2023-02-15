using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] BaseCharacter _base;
    [SerializeField] int level;

    public Character Character { get; set; }

    public void Setup()
    {
        Character = new Character(_base, level);
        this.GetComponent<Image>().sprite = Character.Base.BattleSprite;
    }
}
