using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string dialogueName;

    [TextArea(3, 6)]
    public string[] ncpNameArray;
    [TextArea(3,10)]
    public string[] sentences;

}
