using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resetText : MonoBehaviour
{
    private GameObject objectMenu;
    private GameObject canvas;
    private void Start()
    {
        objectMenu = GameObject.Find("Object menu");
        canvas = gameObject.transform.parent.gameObject;
    }

    public void resetAll()
    {
        for(int i = 0; i < objectMenu.transform.childCount; i++)
        {
            objectMenu.transform.GetChild(i).gameObject.SetActive(true);
        }
        Destroy(canvas);
        //Destroy(gameObject);
    }
}
