using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public float dropRate = 3f;
    public float musicVolume = 0.5f;
    public float soundEffectVolume = 0.5f;
    public float initialSpawnInterval = 2.0f;
    public float minimumSpawnInterval = 0.5f;
    public float spawnIntervalDecreaseRate = 0.1f;
    public bool infinteMode = false;
    public bool enableRockerAiders = false;
    public bool autoEnableShields = false;
    //public bool infinteMode = false;
    public int controlType = 0;
    public int highScore = 0;
    public int maxShields = 6;

}
