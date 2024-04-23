using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createInputField : MonoBehaviour
{
    public GameObject inputField;
    public void instantiate()
    {
        Debug.Log("Instantiating");
        Instantiate(inputField, inputField.transform.position, inputField.transform.rotation);
    }
}
