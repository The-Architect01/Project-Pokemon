using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {

    public Queue<string> Dialogue;
    public Image BG;
    public Text Text;

    private void Start() {
        Dialogue = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) {
        Debug.Log("Conversation " + dialogue.Name);
        Dialogue.Clear();
        foreach (string sentence in dialogue.Sentences) {
            Dialogue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (Dialogue.Count == 0) { EndConversation(); return; }
        string Sentence = Dialogue.Dequeue();
        Debug.Log(Sentence);
        StopAllCoroutines();
        StartCoroutine(AnimateText(Sentence));
    }

    IEnumerator AnimateText(string line) {
        BG.enabled = Text.enabled = true;
        Text.text = "";
        foreach(char letter in line) {
            Text.text += letter;
            yield return new WaitForSeconds(.03f);
        }
    }

    public void EndConversation() {
        Debug.Log("End of Conversation");
        BG.enabled = Text.enabled = false;
    }
}
