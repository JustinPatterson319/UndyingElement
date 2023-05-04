using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    private Vector2 input;
    
    private Characters characters;

    GameObject forSounds;
    [SerializeField] AudioClip cave;
    [SerializeField] AudioClip town;
    [SerializeField] AudioClip home;
    [SerializeField] AudioClip woods;
    [SerializeField] AudioClip boss;

    Scene currentScene;
    String sceneName;

    // Start is called before the first frame update

    private void Awake()
    {
        characters = GetComponent<Characters>();
    }
    
    void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        //Disable music during battle
        forSounds = (GameObject.Find("Battle"));
        if (forSounds != null)
        {
            if (forSounds.activeSelf)
            {
                this.GetComponent<AudioSource>().enabled = false;
            }
        }
        else
        {
            this.GetComponent<AudioSource>().enabled = true;
        }

        //Scene music
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "Town")
        {
            if (GetComponent<AudioSource>().clip != town)
            {
                GetComponent<AudioSource>().clip = town;
                GetComponent<AudioSource>().pitch = 0.9f;
                GetComponent<AudioSource>().volume = 0.1f;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (sceneName == "Woods")
        {
            if (GetComponent<AudioSource>().clip != woods)
            {
                GetComponent<AudioSource>().clip = woods;
                GetComponent<AudioSource>().pitch = 1f;
                GetComponent<AudioSource>().volume = 0.15f;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (sceneName == "House 1" || sceneName == "House 2" || sceneName == "House 3" || sceneName == "Starting Room" || sceneName == "Starting Room (Empty)" || sceneName == "Cave Room")
        {
            if (GetComponent<AudioSource>().clip != home)
            {
                GetComponent<AudioSource>().clip = home;
                GetComponent<AudioSource>().pitch = 1f;
                GetComponent<AudioSource>().volume = 0.15f;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (sceneName == "Cave Floor 1" || sceneName == "Cave Floor 2" || sceneName == "Cave Floor 3" || sceneName == "Cave Room 2" || sceneName == "Cave Room 3")
        {
            if (GetComponent<AudioSource>().clip != cave)
            {
                GetComponent<AudioSource>().clip = cave;
                GetComponent<AudioSource>().pitch = 0.77f;
                GetComponent<AudioSource>().volume = 0.1f;
                GetComponent<AudioSource>().Play();
            }
        }
        else if (sceneName == "Boss Room" || sceneName == "Cave Treasure")
        {
            if (GetComponent<AudioSource>().clip != boss)
            {
                GetComponent<AudioSource>().clip = boss;
                GetComponent<AudioSource>().pitch = .9f;
                GetComponent<AudioSource>().volume = 0.3f;
                GetComponent<AudioSource>().Play();
            }
        }


    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (characters.IsMoving == false)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            
            //removes diagonal movement
            if (input.x != 0)
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                StartCoroutine(characters.Move(input, OnMoveOver));
            }
        }

        characters.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Interact());
        }
    }

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, GameLayers.i.TriggerableLayers);
        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if(triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

    IEnumerator Interact()
    {
        var facingDir = new Vector3(characters.Animator.MoveX, characters.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    }

    public Characters Characters => characters;
}
