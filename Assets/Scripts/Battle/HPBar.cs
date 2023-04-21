using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    public Image healthColor;

    public bool IsUpdating { get; private set; }

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHp)
    {
        IsUpdating = true;

        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHp;
        if(changeAmt < 0)
        {
            while (curHp - newHp < Mathf.Epsilon)
            {
                curHp += changeAmt * -1 * Time.deltaTime;
                health.transform.localScale = new Vector3(curHp, 1f);
                yield return null;
            }
        }
        else
        {
            while (curHp - newHp > Mathf.Epsilon)
            {
                curHp -= changeAmt * Time.deltaTime;
                health.transform.localScale = new Vector3(curHp, 1f);
                yield return null;
            }
        }
        health.transform.localScale = new Vector3(newHp, 1f);
        IsUpdating = false;
    }
}
