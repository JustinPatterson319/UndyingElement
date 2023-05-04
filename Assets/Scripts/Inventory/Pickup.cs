using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactable
{
    [SerializeField] ItemBase item;
    public bool Used { get; set; } = false;

    [SerializeField] Sprite OpenSprite;
    [SerializeField] AudioClip open;
    
    GameController player;
    string objectName;

    public void Awake()
    {
        player = (GameObject.Find("GameController").GetComponent<GameController>());
        objectName = this.gameObject.name;
        //If opened, from player object
        
        if (GameObject.Find("Chest 1") != null)
        {
            if (player.chest1)
            {
                GameObject.Find("Chest 1").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 1").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 2") != null)
        {
            if (player.chest2)
            {
                GameObject.Find("Chest 2").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 2").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 3") != null) 
        {
            if (player.chest3)
            {
                GameObject.Find("Chest 3").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 3").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 4") != null) 
        {
            if (player.chest4)
            {
                GameObject.Find("Chest 4").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 4").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 5") != null) 
        {
            if (player.chest5)
            {
                GameObject.Find("Chest 5").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 5").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 6") != null) 
        {
            if (player.chest6)
            {
                GameObject.Find("Chest 6").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 6").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 7") != null) 
        {
            if (player.chest7)
            {
                GameObject.Find("Chest 7").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 7").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 8") != null) 
        {
            if (player.chest8)
            {
                GameObject.Find("Chest 8").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 8").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 9") != null) 
        {
            if (player.chest9)
            {
                GameObject.Find("Chest 9").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 9").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 10") != null)
        {
            if (player.chest10)
            {
                GameObject.Find("Chest 10").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 10").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 11") != null)
        {
            if (player.chest11)
            {
                GameObject.Find("Chest 11").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 11").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 12") != null)
        {
            if (player.chest12)
            {
                GameObject.Find("Chest 12").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 12").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 13") != null)
        {
            if (player.chest13)
            {
                GameObject.Find("Chest 13").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 13").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 14") != null)
        {
            if (player.chest14)
            {
                GameObject.Find("Chest 14").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 14").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 15") != null)
        {
            if (player.chest15)
            {
                GameObject.Find("Chest 15").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 15").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 16") != null)
        {
            if (player.chest16)
            {
                GameObject.Find("Chest 16").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 16").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 17") != null)
        {
            if (player.chest17)
            {
                GameObject.Find("Chest 17").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 17").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 18") != null)
        {
            if (player.chest18)
            {
                GameObject.Find("Chest 18").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 18").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 19") != null)
        {
            if (player.chest19)
            {
                GameObject.Find("Chest 19").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 19").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 20") != null)
        {
            if (player.chest20)
            {
                GameObject.Find("Chest 20").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 20").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 21") != null)
        {
            if (player.chest21)
            {
                GameObject.Find("Chest 21").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 21").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 22") != null)
        {
            if (player.chest22)
            {
                GameObject.Find("Chest 22").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 22").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 23") != null)
        {
            if (player.chest23)
            {
                GameObject.Find("Chest 23").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 23").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 24") != null)
        {
            if (player.chest24)
            {
                GameObject.Find("Chest 24").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 24").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }

        if (GameObject.Find("Chest 25") != null)
        {
            if (player.chest25)
            {
                GameObject.Find("Chest 25").GetComponent<Pickup>().Used = true;
                GameObject.Find("Chest 25").GetComponent<SpriteRenderer>().sprite = OpenSprite;
            }
        }
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            GetComponent<AudioSource>().clip = open;
            GetComponent<AudioSource>().Play(0);

            initiator.GetComponent<Inventory>().AddItem(item);
            Used = true;

            GetComponent<SpriteRenderer>().sprite = OpenSprite;

            yield return DialogManager.Instance.ShowDialogText($"Received {item.Name}!");

            if(objectName == "Chest 1")
            {
                Debug.Log("Activating 1");
                player.chest1 = true;
            }
            if (objectName == "Chest 2")
            {
                Debug.Log("Activating 2");
                player.chest2 = true;
            }
            if (objectName == "Chest 3")
            {
                Debug.Log("Activating 3");
                player.chest3 = true;
            }
            if (objectName == "Chest 4")
            {
                Debug.Log("Activating 4");
                player.chest4 = true;
            }
            if (objectName == "Chest 5")
            {
                Debug.Log("Activating 5");
                player.chest5 = true;
            }
            if (objectName == "Chest 6")
            {
                Debug.Log("Activating 6");
                player.chest6 = true;
            }
            if (objectName == "Chest 7")
            {
                player.chest7 = true;
            }
            if (objectName == "Chest 8")
            {
                player.chest8 = true;
            }
            if (objectName == "Chest 9")
            {
                player.chest9 = true;
            }
            if (objectName == "Chest 10")
            {
                player.chest10 = true;
            }
            if (objectName == "Chest 11")
            {
                player.chest11 = true;
            }
            if (objectName == "Chest 12")
            {
                player.chest12 = true;
            }
            if (objectName == "Chest 13")
            {
                player.chest13 = true;
            }
            if (objectName == "Chest 14")
            {
                player.chest14 = true;
            }
            if (objectName == "Chest 15")
            {
                player.chest15 = true;
            }
            if (objectName == "Chest 16")
            {
                player.chest16 = true;
            }
            if (objectName == "Chest 17")
            {
                player.chest17 = true;
            }
            if (objectName == "Chest 18")
            {
                player.chest18 = true;
            }
            if (objectName == "Chest 19")
            {
                player.chest19 = true;
            }
            if (objectName == "Chest 20")
            {
                player.chest20 = true;
            }
            if (objectName == "Chest 21")
            {
                player.chest21 = true;
            }
            if (objectName == "Chest 22")
            {
                player.chest22 = true;
            }
            if (objectName == "Chest 23")
            {
                player.chest23 = true;
            }
            if (objectName == "Chest 24")
            {
                player.chest24 = true;
            }
            if (objectName == "Chest 25")
            {
                player.chest25 = true;
            }
        }
    }
}
