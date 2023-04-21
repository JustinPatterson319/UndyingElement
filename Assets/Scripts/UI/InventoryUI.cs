using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public enum InventoryUIState { ItemSelection, PartySelection, Busy }

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip heal;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemDescription;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    [SerializeField] PartyScreen partyScreen;

    int selectedItem = 0;
    InventoryUIState state;

    List<ItemSlotUI> slotUIList;

    Inventory inventory;
    RectTransform itemListRect;

    Action onItemUsed;

    private void Awake()
    {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateItemList();

        inventory.OnUpdated += UpdateItemList;
    }

    void UpdateItemList()
    {
        //Clear all existing items
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in inventory.Slots)
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }
        UpdateItemSelection();
    }

    public void HandleUpdate(Action onBack, Action onItemUsed=null)
    {
        this.onItemUsed = onItemUsed;


        if(state == InventoryUIState.ItemSelection)
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
            selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

            if (prevSelection != selectedItem)
            {
                UpdateItemSelection();
            }

            if(Input.GetKeyDown(KeyCode.Z))
            {
                OpenPartyScreen();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                onBack?.Invoke();
            }
        }
        else if(state == InventoryUIState.PartySelection)
        {
            Action onSelected = () =>
            {
                //Use item on selected character
                StartCoroutine(UseItem());
            };

            Action onBackPartyScreen = () =>
            {
                ClosePartyScreen();
            };

            //Handle party selection
            partyScreen.HandleUpdate(onSelected, onBackPartyScreen);
        }    
    }

    IEnumerator UseItem()
    {
        state = InventoryUIState.Busy;

        var UsedItem = inventory.UseItem(selectedItem, partyScreen.SelectedMember);
        if (UsedItem != null)
        {
            GetComponent<AudioSource>().clip = heal;
            GetComponent<AudioSource>().Play(0);
            yield return DialogManager.Instance.ShowDialogText($"Used {UsedItem.Name} on {partyScreen.SelectedMember.Base.Name}");
            onItemUsed?.Invoke();
        }
        else
        {
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            yield return DialogManager.Instance.ShowDialogText("This item has no effect!");
        }

        ClosePartyScreen();
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
            {
                slotUIList[i].NameText.color = Color.white;
            }
            else
            {
                slotUIList[i].NameText.color = Color.black;
            }
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

        var item = inventory.Slots[selectedItem].Item;
        itemIcon.sprite = item.Icon;
        itemDescription.text = item.Description;

        HandleScrolling();
    }

    void HandleScrolling()
    {
        if (slotUIList.Count <= 7)
        {
            downArrow.gameObject.SetActive(false);
            upArrow.gameObject.SetActive(false);
            return;
        }

        float scrollPos = Mathf.Clamp(selectedItem - 3, 0, selectedItem) * 50;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);

        bool showUpArrow = selectedItem > 3;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selectedItem + 4 < slotUIList.Count;
        downArrow.gameObject.SetActive(showDownArrow);
    }

    void OpenPartyScreen()
    {
        GetComponent<AudioSource>().clip = select;
        GetComponent<AudioSource>().Play(0);
        state = InventoryUIState.PartySelection;
        partyScreen.gameObject.SetActive(true);
    }

    void ClosePartyScreen()
    {
        GetComponent<AudioSource>().clip = select;
        GetComponent<AudioSource>().Play(0);
        state = InventoryUIState.ItemSelection;
        partyScreen.gameObject.SetActive(false);
    }
}
