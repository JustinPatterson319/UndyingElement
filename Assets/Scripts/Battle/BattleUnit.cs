using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] BaseCharacter _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Character Character { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;
    public GameObject hitbox;

    private void Awake()
    {
        image = this.GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        Character = new Character(_base, level);
        image.sprite = Character.Base.BattleSprite;

        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if(isPlayerUnit)
        {
            
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 1.5f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if(isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, .25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, .25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, .25f));
    }

    public void PlayHitAnimation(Move move)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, .1f));
        sequence.Append(image.DOColor(originalColor, .1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.red, .1f));
        sequence.Append(image.DOFade(0f, 0.5f));
    }
}
