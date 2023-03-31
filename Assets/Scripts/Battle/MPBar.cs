using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPBar : MonoBehaviour
{
    [SerializeField] GameObject magic;

    public void SetMP(float mpNormalized)
    {
        magic.transform.localScale = new Vector3(mpNormalized, 1f);
    }

    public IEnumerator SetMPSmooth(float newMp)
    {
        float curMp = magic.transform.localScale.x;
        float changeAmt = curMp - newMp;
        if(changeAmt < 0)
        {
            while (curMp - newMp < Mathf.Epsilon)
            {
                curMp += changeAmt * -1 * Time.deltaTime;
                magic.transform.localScale = new Vector3(curMp, 1f);
                yield return null;
            }
        }
        else
        {
            while (curMp - newMp > Mathf.Epsilon)
            {
                curMp -= changeAmt * Time.deltaTime;
                magic.transform.localScale = new Vector3(curMp, 1f);
                yield return null;
            }
        }
        magic.transform.localScale = new Vector3(newMp, 1f);
    }
}
