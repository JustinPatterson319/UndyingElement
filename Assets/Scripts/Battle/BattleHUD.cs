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

    Character _character;

    public void SetData(Character character)
    {
        _character = character;
        levelText.text = "Lvl" + character.Level;
        healthText.text = character.currentHP + "/" + character.HP;
        magicText.text = character.currentMP + "/" + character.MP;
        hpBar.SetHP((float) character.currentHP / character.HP);
        mpBar.SetMP((float) character.currentMP / character.MP);

        //changes element and character face
        element.sprite = character.Base.ElementSprite;
        face.sprite = character.Base.FaceSprite;
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
