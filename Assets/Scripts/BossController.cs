using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour, Interactable
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    Characters character;

    bool battleLost = false;

    private void Awake()
    {
        character = GetComponent<Characters>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    public void Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);
        if (!battleLost)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                GameController.Instance.StartBossBattle(this);
            }));
        }
        else
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterBattle));
        }
    }

    public IEnumerator TriggerBossBattle(PlayerController player)
    {
        //Show exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(.5f);
        exclamation.SetActive(false);

        //walk towards player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        // Show dialog
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
        {
            GameController.Instance.StartBossBattle(this);
        }));
    }

    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if(dir == FacingDirection.Right)
        {
            angle = 90f;
        }
        else if (dir == FacingDirection.Up)
        {
            angle = 180f;
        }
        else if (dir == FacingDirection.Left)
        {
            angle = 270f;
        }

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
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
