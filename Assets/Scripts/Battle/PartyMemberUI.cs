using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] HPBar hpBar;
    [SerializeField] MPBar mpBar;
    [SerializeField] Image element;
    [SerializeField] Image face;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI magicText;
    [SerializeField] Image partyIcon;

    [SerializeField] Color highlightedColor;

    Character _character;

    public void SetData(Character character)
    {
        _character = character;

        levelText.text = "Lvl" + character.Level;
        nameText.text = character.Base.Name;
        healthText.text = character.currentHP + "/" + character.HP;
        magicText.text = character.currentMP + "/" + character.MP;
        hpBar.SetHP((float)character.currentHP / character.HP);
        mpBar.SetMP((float)character.currentMP / character.MP);

        //changes element and character face
        element.sprite = character.Base.ElementSprite;
        face.sprite = character.Base.FaceSprite;
        partyIcon.sprite = character.Base.PartySprite;
        
    }

    public void SetSelected(bool selected)
    {
        if(selected)
        {
            nameText.color = highlightedColor;
        }
        else
        {
            nameText.color = Color.white;
        }
    }
}
