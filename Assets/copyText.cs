using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class copyText : MonoBehaviour
{
    public SpawnKeyboard keyboard;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = SpawnKeyboard.keyboardText;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(text.text);
    }
}
