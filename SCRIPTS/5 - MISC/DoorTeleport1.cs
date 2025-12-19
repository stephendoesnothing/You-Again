using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleport1: MonoBehaviour
{
    public Transform teleportDestination;
    public GameObject prompt;
    public GameObject boss;
    public GameObject bossUI;
    public DialogueManager manager;

    public AudioClip bossBattle;

    private bool inRange;

    private void Start()
    {
        if(prompt != null)
        {
            prompt.SetActive(false);
        }
    }

    private void Update()
    {
        if(inRange && Input.GetKeyDown(KeyCode.E))
        {
            Teleport();
        }

        if(inRange && prompt != null && PlayerReference.instance.transform != null)
        {
            prompt.transform.position = PlayerReference.instance.transform.position + new Vector3(0, 5f, 0);
        }
    }

    void Teleport()
    {
        

        if(PlayerReference.instance.transform != null && teleportDestination != null && bossUI != null)
        {
            PlayerReference.instance.transform.position = teleportDestination.position;
            boss.SetActive(true);

            if(bossBattle != null)
            {
               SoundManager.Instance.PlayMusic(bossBattle, loop : true);
            }

            bossUI.transform.localScale = Vector3.one;

            if (manager != null) manager.PlayRespawnDialogue();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            if (prompt != null) prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            if (prompt != null) prompt.SetActive(false);
        }
    }
}
