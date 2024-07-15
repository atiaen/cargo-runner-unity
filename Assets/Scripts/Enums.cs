using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum ControlScheme
    {
        MouseFree,
        MouseFixedX,
        KeyboardFixedX,
        FreeMovementKeyboard
    }
}
[System.Serializable]
public class KeyBinding
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode activateShield = KeyCode.Space;
    public KeyCode pause = KeyCode.Escape;

    public void SetKey(string action, KeyCode key)
    {
        switch (action)
        {
            case "MoveLeft":
                moveLeft = key;
                break;
            case "MoveRight":
                moveRight = key;
                break;
            case "MoveUp":
                moveUp = key;
                break;
            case "MoveDown":
                moveDown = key;
                break;
            case "Shield":
                activateShield = key;
                break;
            case "Pause":
                pause = key;
                break;
        }
    }
}