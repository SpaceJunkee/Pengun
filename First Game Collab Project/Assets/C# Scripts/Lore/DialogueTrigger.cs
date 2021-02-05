using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    private bool istriggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && istriggered == false)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        istriggered = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
