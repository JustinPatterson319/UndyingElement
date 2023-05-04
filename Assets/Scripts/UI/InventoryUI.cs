using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum InventoryUIState { ItemSelection, PartySelection, MoveToForget, Busy, Forget }

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip heal;

    [SerializeField] Image itemIcon;
    [SerializeField] Sprite blank;

    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI categoryText;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    [SerializeField] PartyScreen partyScreen;
    [SerializeField] MoveSelectionUI moveSelectionUI;
    [SerializeField] GameObject dialogBox;

    int selectedItem = 0;

    int selectedCategory = 0;

    MoveBase moveToLearn;

    InventoryUIState state;

    List<ItemSlotUI> slotUIList;

    Inventory inventory;
    RectTransform itemListRect;

    Action<ItemBase> onItemUsed;

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

    private void Update()
    {
        if (state == InventoryUIState.Forget)
        {
            Debug.Log("Entering Move Selection");
            Action<int> onMoveSelected = (int moveIndex) =>
            {
                StartCoroutine(OnMoveToForgetSelected(moveIndex));
            };

            //Debug.Log("Select a move");
            moveSelectionUI.HandleMoveSelection(onMoveSelected);
        }
    }

    void UpdateItemList()
    {
        //Clear all existing items
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in inventory.GetSlotsByCategory(selectedCategory))
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }
        UpdateItemSelection();
    }

    public void HandleUpdate(Action onBack, Action<ItemBase> onItemUsed=null)
    {
        this.onItemUsed = onItemUsed;


        if(state == InventoryUIState.ItemSelection)
        {
            Debug.Log("Entering Item Selection");
            int prevSelection = selectedItem;
            int prevCategory = selectedCategory;

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
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                ++selectedCategory;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                --selectedCategory;
            }

            if(selectedCategory > Inventory.ItemCategories.Count - 1)
            {
                selectedCategory = 0;
            }
            else if (selectedCategory < 0)
            {
                selectedCategory = Inventory.ItemCategories.Count - 1;
            }

            selectedItem = Mathf.Clamp(selectedItem, 0, inventory.GetSlotsByCategory(selectedCategory).Count - 1);

            if (prevCategory != selectedCategory)
            {
                ResetSelection();
                categoryText.text = Inventory.ItemCategories[selectedCategory];
                UpdateItemList();
            }
            else if (prevSelection != selectedItem)
            {
                UpdateItemSelection();
            }

            if(Input.GetKeyDown(KeyCode.Z))
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                StartCoroutine(CheckUse());
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                onBack?.Invoke();
            }
        }
        else if(state == InventoryUIState.PartySelection)
        {
            Debug.Log("Entering Party Selection");
            Action onSelected = () =>
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
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
        
        else if (state == InventoryUIState.Busy)
        {
            Debug.Log("Busy");
        }

        else if (state == InventoryUIState.Forget)
        {
            Debug.Log("Entering Move Selection");
            Action<int> onMoveSelected = (int moveIndex) =>
            {
                StartCoroutine(OnMoveToForgetSelected(moveIndex));
            };

            //Debug.Log("Select a move");
            moveSelectionUI.HandleMoveSelection(onMoveSelected);
        }
    }

    IEnumerator CheckUse()
    {
        state = InventoryUIState.Busy;

        var item = inventory.GetItem(selectedItem, selectedCategory);

        if (GameController.Instance.state == GameState.Battle)
        {
            //In battle
            if (!item.CanUseInBattle)
            {
                yield return DialogManager.Instance.ShowDialogText("This item can't be used in battle!");
                state = InventoryUIState.ItemSelection;
                yield break;
            }
        }

        OpenPartyScreen();
    }

    IEnumerator UseItem()
    {
        state = InventoryUIState.Busy;

        yield return HandleTomeItem();

        var UsedItem = inventory.UseItem(selectedItem, partyScreen.SelectedMember, selectedCategory);
        if (UsedItem != null)
        {
            if (UsedItem is RecoveryItems || UsedItem is CrystalItems)
            {
                GetComponent<AudioSource>().clip = heal;
                GetComponent<AudioSource>().Play(0);
                yield return DialogManager.Instance.ShowDialogText($"Used {UsedItem.Name} on {partyScreen.SelectedMember.Base.Name}");
            }
            onItemUsed?.Invoke(UsedItem);
        }
        else
        {
            if (selectedCategory != 2)
            {
                GetComponent<AudioSource>().clip = select;
                GetComponent<AudioSource>().Play(0);
                yield return DialogManager.Instance.ShowDialogText("This item has no effect!");
            }
        }

        ClosePartyScreen();
    }

    IEnumerator HandleTomeItem()
    {
        var tomeItem = inventory.GetItem(selectedItem, selectedCategory) as TomeItems;
        if(tomeItem == null)
        {
            yield break;
        }

        var character = partyScreen.SelectedMember;

        if(character.HasMove(tomeItem.Move))
        {
            yield return DialogManager.Instance.ShowDialogText($"{character.Base.Name} can already use {tomeItem.Move.Name}!");
            yield break;
        }

        if (character.Moves.Count < 6)
        {
            character.LearnMove(tomeItem.Move);
            GetComponent<AudioSource>().clip = heal;
            GetComponent<AudioSource>().Play(0);
            yield return DialogManager.Instance.ShowDialogText($"{character.Base.Name} mastered {tomeItem.Move.Name}!");
        }
        else
        {
            //yield return DialogManager.Instance.ShowDialogText($"{character.Base.Name} is trying to master {tomeItem.Move.Name}.");
            //yield return DialogManager.Instance.ShowDialogText($"{character.Base.Name} is trying to master {tomeItem.Move.Name}.");
            yield return ChooseMoveToForget(character, tomeItem.Move);
            yield return new WaitUntil(() => state != InventoryUIState.Forget);
        }
    }

    IEnumerator ChooseMoveToForget(Character character, MoveBase newMove)
    {
        state = InventoryUIState.Busy;
        yield return DialogManager.Instance.ShowDialogText($"Swap out an ability to master {newMove.Name}?", true, false);
        moveSelectionUI.gameObject.SetActive(true);
        moveSelectionUI.SetMoveData(character.Moves.Select(x => x.Base).ToList(), newMove);
        moveToLearn = newMove;

        state = InventoryUIState.Forget;
        Debug.Log("State is now Forget");
    }

    void UpdateItemSelection()
    {
        var slots = inventory.GetSlotsByCategory(selectedCategory);

        selectedItem = Mathf.Clamp(selectedItem, 0, slots.Count - 1);

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

        if (slots.Count > 0)
        {
            var item = slots[selectedItem].Item;
            itemIcon.sprite = item.Icon;
            itemDescription.text = item.Description;
        }

        else
        {
            itemIcon.sprite = blank;
        }
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

    void ResetSelection()
    {
        selectedItem = 0;
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);

        itemIcon.sprite = null;
        itemDescription.text = "";
    }

    void OpenPartyScreen()
    {
        state = InventoryUIState.PartySelection;
        Debug.Log("We've opened the party screen!");
        GetComponent<AudioSource>().clip = select;
        GetComponent<AudioSource>().Play(0);
        partyScreen.gameObject.SetActive(true);
    }

    void ClosePartyScreen()
    {
        GetComponent<AudioSource>().clip = select;
        GetComponent<AudioSource>().Play(0);
        state = InventoryUIState.ItemSelection;
        partyScreen.gameObject.SetActive(false);
    }

    IEnumerator OnMoveToForgetSelected(int moveIndex)
    {
        var character = partyScreen.SelectedMember;

        DialogManager.Instance.CloseDialog();
        moveSelectionUI.gameObject.SetActive(false);
        if (moveIndex == 6)
        {
            var selectedMove = character.Moves[moveIndex-1].Base;
            //Don't learn move
            GetComponent<AudioSource>().clip = select;
            GetComponent<AudioSource>().Play(0);
            yield return DialogManager.Instance.ShowDialogText("Training for this move was abandoned...");
            state = InventoryUIState.ItemSelection;
        }
        else
        {
            //swap selected move for new move
            var selectedMove = character.Moves[moveIndex].Base;
            GetComponent<AudioSource>().clip = heal;
            GetComponent<AudioSource>().Play(0);
            yield return DialogManager.Instance.ShowDialogText($"{character.Base.Name} mastered {moveToLearn.Name} in place of {selectedMove.Name}!");
            character.Moves[moveIndex] = new Move(moveToLearn);
            state = InventoryUIState.ItemSelection;
        }
        ClosePartyScreen();
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
