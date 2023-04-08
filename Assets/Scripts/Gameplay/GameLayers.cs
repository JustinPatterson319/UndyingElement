using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask encounterLayer;

    public static GameLayers i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask SolidLayer
    {
        get => collisionLayer;
    }

    public LayerMask InteractableLayer
    {
        get => interactableLayer;
    }

    public LayerMask EncounterLayer
    {
        get => encounterLayer;
    }
}
