using Solution;
using UnityEngine;
using UnityEngine.Serialization;

public class NPC : Identity
{
    public DialogueUI dialogueUI;
    [FormerlySerializedAs("dialogueManager")]
    [FormerlySerializedAs("dialogueSeauen")]
    public DialogueSequen sequen;
    public bool canTalk = true;

    private void Awake()
    {
        if (sequen == null)
        {
            sequen = GetComponent<DialogueSequen>();
        }
    }

    public override bool Hit()
    {
        if (canTalk)
        {
            if (sequen == null)
            {
                sequen = GetComponent<DialogueSequen>();
            }

            if (sequen != null && dialogueUI != null)
            {
                dialogueUI.Setup(sequen);
            }
            else
            {
                Debug.LogError("NPC: dialogueUI or sequen is missing!");
            }
            return false;
        }
        else
        {
            Debug.Log("I not neet to talk to you");
            return false;
        }
    }
}
