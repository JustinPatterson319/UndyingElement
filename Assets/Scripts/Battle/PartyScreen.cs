using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PartyScreen : MonoBehaviour
{   
    PartyMemberUI[] memberSlots;
    List<Character> characters;

    PlayerParty party;

    [SerializeField] TextMeshProUGUI messageText;
    int currentMember = 0;
    [SerializeField] AudioClip select;

    public Character SelectedMember => characters[currentMember];

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
        party = PlayerParty.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    public void SetPartyData()
    {
        characters = party.Characters;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < characters.Count)
            {
                memberSlots[i].Init(characters[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        UpdateMemberSelection(currentMember);
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

    public void HandleUpdate(Action onSelected, Action onBack)
    {
        var prevSelection = currentMember;
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 0 || currentMember == 2)
            {
                currentMember++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 1 || currentMember == 3)
            {
                currentMember--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 0 || currentMember == 1)
            {
                currentMember = currentMember + 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            if (currentMember == 2 || currentMember == 3)
            {
                currentMember = currentMember - 2;
            }
        }
        if (currentMember != prevSelection)
        {
            UpdateMemberSelection(currentMember);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            onSelected?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
        }
    }
}
