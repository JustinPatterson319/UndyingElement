using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyScreen : MonoBehaviour
{   
    PartyMemberUI[] memberSlots;
    List<Character> characters;
    [SerializeField] TextMeshProUGUI messageText;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Character> characters)
    {
        this.characters = characters;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < characters.Count)
            {
                memberSlots[i].SetData(characters[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == selectedMember)
            {
                memberSlots[i].SetSelected(true);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
