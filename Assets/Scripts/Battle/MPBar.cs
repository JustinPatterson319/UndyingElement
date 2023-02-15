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
}
