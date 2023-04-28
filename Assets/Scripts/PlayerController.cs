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
