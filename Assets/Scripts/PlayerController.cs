using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    private Vector2 input;
    
    private Characters characters;

    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterBossView;

    // Start is called before the first frame update
   
    private void Awake()
    {
        characters = GetComponent<Characters>();
    }
    
    void Start()
    {
        
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
            Interact();
        }
    }

    private void OnMoveOver()
    {
        checkForEncounter();
        checkForBoss();
    }

    private void checkForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.EncounterLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                Debug.Log("Battle!");
                characters.Animator.IsMoving = false;
                OnEncountered();
            }
        }
    }

    private void checkForBoss()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FOVLayer);
        if (collider != null)
        {
            characters.Animator.IsMoving = false;
            OnEnterBossView?.Invoke(collider);
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(characters.Animator.MoveX, characters.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
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
}
