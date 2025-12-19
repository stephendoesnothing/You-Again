using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpen : MonoBehaviour
{
    public GameObject panel;
    public GameObject prompt;
    public string playerTag = "Player";

    private bool inRange = false;
    private bool isOpen = false;

    void Update()
    {

        if (inRange)
        {
            if (panel != null && Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                panel.SetActive(isOpen);
            }

            if (prompt != null && PlayerReference.instance != null)
            {
                prompt.transform.position = PlayerReference.instance.transform.position + new Vector3(0, 5f, 0);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = true;
            if (prompt != null)
                prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = false;
            if (prompt != null)
                prompt.SetActive(false);

            if (isOpen)
            {
                isOpen = false;
                panel.SetActive(false);
            }
        }
    }
}
