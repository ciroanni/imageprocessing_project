using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnKeyboard : MonoBehaviour
{
    UnityEngine.TouchScreenKeyboard keyboard;
    public static string keyboardText = "";
    public Text noteText;
    public resetText resetText;

    public void Update()
    {
        if (keyboard != null)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Done || Input.GetKey(KeyCode.KeypadEnter))
            {
                keyboardText = keyboard.text;
                noteText.text = keyboardText;
                resetText.resetAll();
                keyboard = null;
            }
        }
    }
}
