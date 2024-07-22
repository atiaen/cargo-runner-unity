using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class SaveData
{
    public float musicVolume = 0.5f;
    public float soundEffectVolume = 0.5f;
    public float initialSpawnInterval = 3.5f;
    public float minimumSpawnInterval = 1f;
    public float spawnIntervalDecreaseRate = 1f;

    public float pickupinitialSpawnInterval = 6;
    public float pickupminimumSpawnInterval = 0.8f;
    public float pickupspawnIntervalDecreaseRate = 0.5f;

    public float moveSpeed = 10f;
    public bool playedTutorial = false;
    public bool infinteMode = false;
    public bool audioAlerts = false;
    public ControlScheme controlType = ControlScheme.MouseFixedX;
    public KeyBinding keyBindings = new KeyBinding();
    public int highScore = 0;
    public int maxShields = 6;
   

}
