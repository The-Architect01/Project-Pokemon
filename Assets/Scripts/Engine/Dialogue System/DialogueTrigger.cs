using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue Dialogue;

    public void TriggerDialogue() {
        FindObjectOfType<DialogueSystem>().StartDialogue(Dialogue);
    }
}
