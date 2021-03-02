using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;

    public Text nameText;
    public Text dialogueText;

    PlayerMovement pm;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        sentences = new Queue<string>();
    }

   public void StartDialogue(Dialogue dialogue)
    {
        PlayerMovement.canMove = false;
        pm.StopPlayer();

        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.015f);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        StartCoroutine(ActivateMovement());
    }

    IEnumerator ActivateMovement()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerMovement.canMove = true;
    }
}
