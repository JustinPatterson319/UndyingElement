using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    List<TextMeshProUGUI> menuItems;
    int selectedItem = 0;
    [SerializeField] AudioClip select;

    public event Action<int> onMenuSelected;
    public event Action onBack;

    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<TextMeshProUGUI>().ToList();
    }

    public void OpenMenu()
    {
        GetComponent<AudioSource>().clip = select;
        GetComponent<AudioSource>().Play(0);
        menu.SetActive(true);
        UpdateItemSelection();
    }

    public void CloseMenu()
    {
        GetComponent<AudioSource>().clip = select;
        GetComponent<AudioSource>().Play(0);
        menu.SetActive(false);
    }

    public void HandleUpdate()
    {
        int prevSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            ++selectedItem;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            --selectedItem;
        }
        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        if (prevSelection != selectedItem)
        {
            UpdateItemSelection();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            onMenuSelected?.Invoke(selectedItem);
            //BECAUSE NOTHING HAPPENS YET, CLOSE MENU
            CloseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            onBack?.Invoke();
            CloseMenu();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selectedItem)
            {
                menuItems[i].color = Color.white;
            }
            else
            {
                menuItems[i].color = Color.black;
            }
        }
    }
}
