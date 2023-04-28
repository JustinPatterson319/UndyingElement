using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MoveSelectionUI : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> moveTexts;
    [SerializeField] AudioClip select;

    int currentSelection = 0;

    public void SetMoveData(List<MoveBase> currentMoves, MoveBase newMove)
    {
        for (int i = 0; i<currentMoves.Count; ++i)
        {
            moveTexts[i].text = currentMoves[i].Name;
        }

        moveTexts[currentMoves.Count].text = newMove.Name;
    }

    public void HandleMoveSelection(Action<int> onSelected)
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            ++currentSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            --currentSelection;
        }

        currentSelection = Mathf.Clamp(currentSelection, 0, 6);
        UpdateMoveSelection(currentSelection);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            onSelected?.Invoke(currentSelection);
        }
    }

    public void UpdateMoveSelection(int selection)
    {
        for (int i = 0; i < 7; i++)
        {
            if(i == selection)
            {
                moveTexts[i].color = Color.white;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }
    }
}
