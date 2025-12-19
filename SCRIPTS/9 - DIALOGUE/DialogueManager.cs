using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [System.Serializable]
    public class DialogueTrigger
    {
        [Range(0, 100)] public float hpThreshold; // e.g., 75 for 75%
        public List<DialogueManager.DialogueLine> lines;
        public bool triggered = false;
    }

    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(1, 3)]
        public string line;
        public Sprite pose;
        public float delay = 2f;
        public bool shake = false;
    }

    [Header("Dialogue Settings")]
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    [Header("HP-Based Dialogue")]
    public List<DialogueTrigger> dialogueTriggers = new List<DialogueTrigger>();

    [Header("Respawn Dialogue")]
    public List<DialogueLine> respawnDialogueLines = new List<DialogueLine>();

    [HideInInspector]
    public bool isTalking = false;

    [Header("References")]
    public GameObject floatingTextPrefab;
    public Transform textSpawnPoint;
    public SpriteRenderer bossSprite;
    public Canvas worldCanvas;
    public CameraShake shake;
    public float intensity = 1f;

    private Coroutine dialogueRoutine;

    public void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayDialogue()
    {
        dialogueRoutine = StartCoroutine(PlayDialogueRoutine());
    }

    public void PlayRespawnDialogue()
    {
        if (respawnDialogueLines != null && respawnDialogueLines.Count > 0)
            StartCoroutine(PlayCustomDialogue(respawnDialogueLines));
    }

    private IEnumerator PlayDialogueRoutine()
    {
        isTalking = true;

        foreach (var line in dialogueLines)
        {
            if (bossSprite != null && line.pose != null)
                bossSprite.sprite = line.pose;

            if (floatingTextPrefab != null && textSpawnPoint != null)
            {
                GameObject textObj = Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity, worldCanvas.transform);
                Text uiText = textObj.GetComponentInChildren<Text>();
                uiText.text = "";

                // Start the typewriter effect
                yield return StartCoroutine(TypeText(line.line, uiText));

                // Trigger screen shake after the full line is typed
                if (line.shake && shake != null)
                    shake.Shake(intensity);

                Destroy(textObj, line.delay + 1f);
            }

            yield return new WaitForSeconds(line.delay);
        }

        isTalking = false;
    }

    private IEnumerator TypeText(string fullText, Text uiText)
    {
        float typingSpeed = 0.02f; // Speed per character

        for (int i = 0; i < fullText.Length; i++)
        {
            uiText.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void TryTriggerHPDialogue(float currentHpPercent)
    {
        foreach (var trigger in dialogueTriggers)
        {
            if (!trigger.triggered && currentHpPercent <= trigger.hpThreshold)
            {
                trigger.triggered = true;
                StartCoroutine(PlayCustomDialogue(trigger.lines));
                break; // Only trigger one per frame
            }
        }
    }

    public IEnumerator PlayCustomDialogue(List<DialogueLine> lines)
    {
        isTalking = true;

        foreach (var line in lines)
        {
            if (bossSprite != null && line.pose != null)
                bossSprite.sprite = line.pose;

            if (floatingTextPrefab != null && textSpawnPoint != null)
            {
                GameObject textObj = Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity, worldCanvas.transform);
                Text uiText = textObj.GetComponentInChildren<Text>();
                uiText.text = "";
                yield return StartCoroutine(TypeText(line.line, uiText));

                if (line.shake && shake != null)
                    shake.Shake(intensity);

                Destroy(textObj, line.delay + 1f);
            }

            yield return new WaitForSeconds(line.delay);
        }

        isTalking = false;
    }

}
