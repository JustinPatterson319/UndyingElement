using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerParty : MonoBehaviour
{
    [SerializeField] List<Character> characters;
    
    public List<Character> Characters
    {
        get
        {
            return characters;
        }
    }

    private void Start()
    {
        foreach (var character in characters)
        {
            character.Init();
        }
    }

    public Character GetHealthy()
    {
        return characters.Where(x => x.currentHP > 0).FirstOrDefault();
    }
}
