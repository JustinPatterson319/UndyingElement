using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new crystal item")]
public class CrystalItems : ItemBase
{
    [SerializeField] CharacterElement type;
    [SerializeField] Sprite elementSprite;

    public override bool Use(Character character)
    {
        character.UpdateElement(type, elementSprite);
        return true;
    }
}
