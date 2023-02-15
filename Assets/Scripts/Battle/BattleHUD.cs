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

    public void SetData(Character character)
    {
        levelText.text = "Lvl" + character.Level;
        hpBar.SetHP((float) character.currentHP / character.HP);
        mpBar.SetMP((float) character.currentMP / character.MP);

        //changes element and character face
        element.sprite = character.Base.ElementSprite;
        face.sprite = character.Base.FaceSprite;
    }
}
