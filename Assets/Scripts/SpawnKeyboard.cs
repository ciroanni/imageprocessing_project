using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnKeyboard : MonoBehaviour
{
    public TouchScreenKeyboard keyboard;
    public static string keyboardText = "";
    private PhotonView photonView;
    [SerializeField] private Text noteText;
    [SerializeField] private resetText resetText;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine)
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }
    public void Update()
    {
        if (keyboard != null)
        {
            keyboardText = keyboard.text;
            noteText.text = keyboardText;
            if (keyboard.status == TouchScreenKeyboard.Status.Done || keyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                resetText.resetAll();
                keyboard = null;
            }
            
        }
    }
}
