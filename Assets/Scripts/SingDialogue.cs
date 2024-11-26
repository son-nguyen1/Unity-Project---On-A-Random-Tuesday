using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingDialogue : MonoBehaviour
{
    [SerializeField] private float singDialogueSpeed = 0.05f;

    // UI Elements
    [SerializeField] private GameObject singDialogueBox;
    [SerializeField] private TextMeshProUGUI singDialogueText;

    // Text Array
    [SerializeField] private string[] singDialogue; // Range of Dialogue Texts
    private int singDialogueIndex; // Position of text within range

    private void Start()
    {
        // Disable and clear dialogue text
        singDialogueText.text = string.Empty;
        singDialogueBox.SetActive(false);

        // Set dialogue at 1st line (index 0)
        singDialogueIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<SingDialogueTree>() != null) // Only collide object with this script
        {
            // Run dialogue text
            singDialogueBox.SetActive(true);
            StartCoroutine(TypeSingDialogue());
        }
    }

    private IEnumerator TypeSingDialogue()
    {
        // Transform text into array of individual characters
        foreach (char c in singDialogue[singDialogueIndex].ToCharArray())
        {
            singDialogueText.text += c; // Spawn characters 1-by-1 (like typing)
            yield return new WaitForSeconds(singDialogueSpeed); // Next character appears after x secs
        }

        // Wait before moving to next text
        yield return new WaitForSeconds(1.5f);

        // Move to next text
        NextSingDialogue();
    }

    private void NextSingDialogue()
    {
        // As text position is within range, run next dialogue text
        if (singDialogueIndex < singDialogue.Length - 1)
        {
            singDialogueIndex++;
            singDialogueText.text = string.Empty;
            StartCoroutine(TypeSingDialogue());
        }
        else
        {
            singDialogueBox.SetActive(false); // Disable dialogue text when out of range
        }
    }
}