using System;
using UnityEngine;

[System.Serializable]
public class SaveState
{

    public DateTime LastSaveTime { get; set; }
    public int SaveCount { get; set; }
}
