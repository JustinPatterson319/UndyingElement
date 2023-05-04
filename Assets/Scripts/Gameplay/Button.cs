using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, Interactable
{
    public bool Used { get; set; } = false;

    [SerializeField] Sprite PressedSprite;
    [SerializeField] AudioClip press;

    GameController player;
    string objectName;

    public GameObject obstacle;

    public void Awake()
    {
        player = (GameObject.Find("GameController").GetComponent<GameController>());
        objectName = gameObject.name;
        //If opened, from player object
        if (GameObject.Find("Button 1") != null)
        {
            if (GameObject.Find("Obstacle 1") == obstacle)
            {
                if (player.button1)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 2") != null)
        {
            if (GameObject.Find("Obstacle 2") == obstacle)
            {
                if (player.button2)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 3") != null)
        {
            if (GameObject.Find("Obstacle 3") == obstacle)
            {
                if (player.button3)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 4") != null)
        {
            if (GameObject.Find("Obstacle 4") == obstacle)
            {
                if (player.button4)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 5") != null)
        {
            if (GameObject.Find("Obstacle 5") == obstacle)
            {
                if (player.button5)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 6") != null)
        {
            if (GameObject.Find("Obstacle 6") == obstacle)
            {
                if (player.button6)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 7") != null)
        {
            if (GameObject.Find("Obstacle 7") == obstacle)
            {
                if (player.button7)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 8") != null)
        {
            if (GameObject.Find("Obstacle 8") == obstacle)
            {
                if (player.button8)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 9") != null)
        {
            if (GameObject.Find("Obstacle 9") == obstacle)
            {
                if (player.button9)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 10") != null)
        {
            if (GameObject.Find("Obstacle 10") == obstacle)
            {
                if (player.button10)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }

        if (GameObject.Find("Button 11") != null)
        {
            if (GameObject.Find("Obstacle 11") == obstacle)
            {
                if (player.button11)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Used = true;
                    GetComponent<SpriteRenderer>().sprite = PressedSprite;
                    obstacle.SetActive(false);
                }
            }
        }
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            GetComponent<AudioSource>().clip = press;
            GetComponent<AudioSource>().Play(0);

            GetComponent<BoxCollider2D>().enabled = false;
            Used = true;

            GetComponent<SpriteRenderer>().sprite = PressedSprite;

            if (objectName == "Button 1")
            {
                player.button1 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 2")
            {
                player.button2 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 3")
            {
                player.button3 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 4")
            {
                player.button4 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 5")
            {
                player.button5 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 6")
            {
                player.button6 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 7")
            {
                player.button7 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 8")
            {
                player.button8 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 9")
            {
                player.button9 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 10")
            {
                player.button10 = true;
                obstacle.SetActive(false);
            }
            if (objectName == "Button 11")
            {
                player.button11 = true;
                obstacle.SetActive(false);
            }

            yield return DialogManager.Instance.ShowDialogText($"Activated a switch!");
        }
    }
}
