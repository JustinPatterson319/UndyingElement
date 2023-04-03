using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] MPBar mpBar;
    [SerializeField] Image element;
    [SerializeField] Image face;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI magicText;

    [SerializeField] Color bleedColor;
    [SerializeField] Color venomColor;
    [SerializeField] Color cureColor;
    [SerializeField] Color chargeColor;
    [SerializeField] Color shockColor;
    [SerializeField] Color chillColor;
    [SerializeField] Color slumberColor;

    Dictionary<ConditionID, Color> statusColors;

    Character _character;

    public void SetData(Character character)
    {
        _character = character;
        levelText.text = "Lvl" + character.Level;
        healthText.text = character.currentHP + "/" + character.HP;
        magicText.text = character.currentMP + "/" + character.MP;
        hpBar.SetHP((float) character.currentHP / character.HP);
        mpBar.SetMP((float) character.currentMP / character.MP);

        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.bleed, bleedColor },
            {ConditionID.venom, venomColor },
            {ConditionID.cure, cureColor },
            {ConditionID.charge, chargeColor },
            {ConditionID.shock, shockColor },
            {ConditionID.chill, chillColor },
            {ConditionID.slumber, slumberColor },
        };

        //changes element and character face
        element.sprite = character.Base.ElementSprite;
        face.sprite = character.Base.FaceSprite;
        _character.OnStatusChanged += SetStatusColor;
    }

    void SetStatusColor()
    {
        if (_character.Status == null)
        {
            hpBar.healthColor.color = Color.green;
            mpBar.magicColor.color = Color.blue;
        }
        else if (_character.Status.Id == ConditionID.venom || _character.Status.Id == ConditionID.charge)
        {
            mpBar.magicColor.color = statusColors[_character.Status.Id];
        }
        else
        {
            hpBar.healthColor.color = statusColors[_character.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
            healthText.text = _character.currentHP + "/" + _character.HP;
            yield return hpBar.SetHPSmooth((float)_character.currentHP / _character.HP);
    }

    public IEnumerator UpdateMP()
    {
            magicText.text = _character.currentMP + "/" + _character.MP;
            yield return mpBar.SetMPSmooth((float)_character.currentMP / _character.MP);
    }
}
