using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    
    private Vector2 input;
    
    private Animator animator;

    public LayerMask collisionLayer;
    public LayerMask interactableLayer;
    public LayerMask encounterLayer;

    public event Action OnEncountered;

    // Start is called before the first frame update
   
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (isMoving == false)
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
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;
                if (CanWalk(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

        checkForEncounter();
    }

    private bool CanWalk(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, collisionLayer | interactableLayer) != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void checkForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, encounterLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                Debug.Log("Battle!");
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}
