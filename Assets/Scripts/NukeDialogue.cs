using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NukeDialogue : MonoBehaviour
{
    [SerializeField] private float dialogueSpeed = 0.05f;

    // UI Elements
    [SerializeField] GameObject nukeDialogueBox;
    [SerializeField] TextMeshProUGUI nukeDialogueText;

    // Text Array
    [SerializeField] private string[] nukeDialogue; // Range of Dialogue Texts
    private int nukeDialogueIndex; // Position of text within range

    // Text Colors
    private Color punkDialogueColor = new Color(245f / 255f, 5f / 255f, 100f / 255f, 1f); // Pink
    private Color bikerDialogueColor = new Color(130f / 255f, 5f / 255f, 245f / 255f, 1f); // Purple

    private bool isPunkTurn = true; // Punk speaks first

    public bool dialogueIsOver = false;

    private void Start()
    {
        // Disable and clear dialogue text
        nukeDialogueText.text = string.Empty;
        nukeDialogueBox.SetActive(false);

        // Set dialogue at 1st line (index 0)
        nukeDialogueIndex = 0;
    }

    public void StartNukeDialogue()
    {
        // Run dialogue text
        nukeDialogueBox.SetActive(true);
        StartCoroutine(TypeNukeDialogue());
    }

    private IEnumerator TypeNukeDialogue()
    {
        // Set text color, alternate between 2 options from conditions
        nukeDialogueText.color = isPunkTurn ? punkDialogueColor : bikerDialogueColor;

        // Transform text into array of individual characters
        foreach (char c in nukeDialogue[nukeDialogueIndex].ToCharArray())
        {
            nukeDialogueText.text += c; // Spawn characters 1-by-1 (like typing)
            yield return new WaitForSeconds(dialogueSpeed); // Next character appears after x secs
        }

        // Wait before moving to next text
        yield return new WaitForSeconds(1f);

        // Move to next text
        NextNukeDialogue();
    }

    private void NextNukeDialogue()
    {
        // As text position is within range, run next dialogue text
        if (nukeDialogueIndex < nukeDialogue.Length - 1)
        {
            nukeDialogueIndex++;
            nukeDialogueText.text = string.Empty;
            isPunkTurn = !isPunkTurn; // Flip condition to alternate color
            StartCoroutine(TypeNukeDialogue());
        }
        else
        {
            // Disable dialogue text when out of range
            nukeDialogueBox.SetActive(false);
            dialogueIsOver = true; // [Punk Script] [Biker Script]
        }
    }
}