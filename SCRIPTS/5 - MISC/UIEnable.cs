using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnable : MonoBehaviour
{

    public GameObject ui;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) ui.SetActive(true);
    }
}
