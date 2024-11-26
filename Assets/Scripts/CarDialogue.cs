using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarDialogue : MonoBehaviour
{
    [SerializeField] private float carDialogueSpeed = 0.025f;

    // UI Elements
    [SerializeField] private GameObject carDialogueBox;
    [SerializeField] private TextMeshProUGUI carDialogueText;

    // Text Array
    [SerializeField] private string[] carDialogue; // Range of Dialogue Texts
    private int carDialogueIndex; // Position of text within range

    // Text Colors
    private Color punkDialogueColor = new Color(245f / 255f, 5f / 255f, 100f / 255f, 1f); // Pink
    private Color bikerDialogueColor = new Color(130f / 255f, 5f / 255f, 245f / 255f, 1f); // Purple

    private bool isPunkTurn = true; // Punk speaks first

    private void Start()
    {
        // Disable and clear dialogue text
        carDialogueBox.SetActive(false);
        carDialogueText.text = string.Empty;

        // Set dialogue at 1st line (index 0)
        carDialogueIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<CarDialogueTree>() != null) // Only collide object with this script
        {
            // Run dialogue text
            carDialogueBox.SetActive(true);
            StartCoroutine(TypeCarDialogue());
        }
    }

    private IEnumerator TypeCarDialogue()
    {
        // Set text color, alternate between 2 options from conditions
        carDialogueText.color = isPunkTurn ?  punkDialogueColor : bikerDialogueColor;

        // Transform text into array of individual characters
        foreach (char c in carDialogue[carDialogueIndex].ToCharArray())
        {
            carDialogueText.text += c; // Spawn characters 1-by-1 (like typing)
            yield return new WaitForSeconds(carDialogueSpeed); // Next character appears after x secs
        }

        // Wait before moving to next text
        yield return new WaitForSeconds(1f);

        // Move to next text
        NextCarDialogue();
    }

    private void NextCarDialogue()
    {
        // As text position is within range, run next dialogue text
        if (carDialogueIndex < carDialogue.Length - 1)
        {
            carDialogueIndex++;
            carDialogueText.text = string.Empty;
            isPunkTurn = !isPunkTurn; // Flip condition to alternate color
            StartCoroutine(TypeCarDialogue());
        }
        else
        {
            carDialogueBox.SetActive(false); // Disable dialogue text when out of range
        }
    }
}