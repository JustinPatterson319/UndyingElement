using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHUD hud;

    public bool IsPlayerUnit
    {
        get
        {
            return isPlayerUnit;
        }
    }

    public BattleHUD Hud
    {
        get
        {
            return hud;
        }
    }

    public Character Character { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = this.GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Character character)
    {
        Character = character;
        image.sprite = Character.Base.BattleSprite;

        hud.gameObject.SetActive(true);
        hud.SetData(character);

        image.color = originalColor;
        
        PlayEnterAnimation();
    }

    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    public void PlayEnterAnimation()
    {
        if(isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if(isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 30f, .25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 30f, .25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, .25f));
    }

    public void PlayDodgeAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 80f, .8f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 80f, .8f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, .35f));
    }

    public void PlayHitAnimation(Move move)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.red, .08f));
        sequence.Append(image.DOColor(originalColor, .08f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.red, .1f));
        sequence.Append(image.DOFade(0f, 0.5f));
    }

    public void PlayRetreatAnimation()
    {
        image.sprite = Character.Base.FleeSprite;
        image.transform.DOLocalMoveX(-500f, 1f);
    }

    public void PlayBossAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0f, 0.5f));
        image.transform.DOLocalMoveX(500f, 1f);
    }
}
