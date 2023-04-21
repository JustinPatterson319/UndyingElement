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

    public void Init(Character character)
    {
        _character = character;
        UpdateData();

        _character.OnHPChanged += UpdateData;
        _character.OnMPChanged += UpdateData;
        _character.OnElementChanged += UpdateData;
    }

    public void UpdateData()
    {
        levelText.text = "Lvl" + _character.Level;
        nameText.text = _character.Base.Name;
        healthText.text = _character.currentHP + "/" + _character.HP;
        magicText.text = _character.currentMP + "/" + _character.MP;
        hpBar.SetHP((float)_character.currentHP / _character.HP);
        mpBar.SetMP((float)_character.currentMP / _character.MP);

        //changes element and character face
        element.sprite = _character.currentElementSprite;
        face.sprite = _character.Base.FaceSprite;
        partyIcon.sprite = _character.Base.PartySprite;
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
