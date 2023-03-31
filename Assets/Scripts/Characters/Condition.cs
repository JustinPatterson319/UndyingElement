using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public Action<Character> OnAfterTurn { get; set; }
    public Func<Character, bool> OnBeforeMove { get; set; }
}
