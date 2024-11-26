using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundDialogue : MonoBehaviour
{
    [SerializeField] private float groundDialogueSpeed = 0.05f;

    // UI Elements
    [SerializeField] private GameObject groundDialogueBox;
    [SerializeField] private TextMeshProUGUI groundDialogueText;

    // Text Array
    [SerializeField] private string[] groundDialogue; // Range of Dialogue Texts
    private int groundDialogueIndex; // Position of text within range

    // Text Colors
    private Color punkDialogueColor = new Color(245f / 255f, 5f / 255f, 100f / 255f, 1); // Pink
    private Color bikerDialogueColor = new Color(130f / 255f, 5f / 255f, 245f / 255f, 1f); // Purple

    private bool isPunkTurn = true; // Punk speaks first

    private void Start()
    {
        // Disable and clear dialogue text
        groundDialogueText.text = string.Empty;
        groundDialogueBox.SetActive(false);

        // Set dialogue at 1st line (index 0)
        groundDialogueIndex = 0;
    }

    private void Update()
    {
        if (groundDialogueBox.activeSelf)
        {
            HandleGroundDialogue(); // Continuously check for input
        }
    }

    private void HandleGroundDialogue()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // As text has displayed full dialogue, run next text
            if (groundDialogueText.text == groundDialogue[groundDialogueIndex])
            {
                NextGroundDialogue(); // Move to next text
            }
            else
            {
                // Stop typing, display full dialogue
                StopAllCoroutines();
                groundDialogueText.text = groundDialogue[groundDialogueIndex];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Rigidbody2D>() != null) // Only collide object with this component
        {
            // Run dialogue text
            groundDialogueBox.SetActive(true);
            StartCoroutine(TypeGroundDialogue());
        }
    }

    private IEnumerator TypeGroundDialogue()
    {
        // Set text color, alternate between 2 options from conditions
        groundDialogueText.color = isPunkTurn ? punkDialogueColor : bikerDialogueColor;

        // Transform text into array of individual characters
        foreach (char c in groundDialogue[groundDialogueIndex].ToCharArray())
        {
            groundDialogueText.text += c; // Spawn characters 1-by-1 (like typing)
            yield return new WaitForSeconds(groundDialogueSpeed); // Next character appears after x secs
        }
    }

    private void NextGroundDialogue()
    {
        // As text position is within range, run next dialogue text
        if (groundDialogueIndex < groundDialogue.Length - 1)
        {
            groundDialogueIndex++;
            groundDialogueText.text = string.Empty;
            isPunkTurn = !isPunkTurn; // Flip condition to alternate color
            StartCoroutine(TypeGroundDialogue());
        }
        else
        {
            // Disable dialogue text when out of range
            groundDialogueBox.SetActive(false);

            // Condition for [Game Manager]
            if (GameManager.Instance != null)
            {
                GameManager.Instance.InstantiateNukeCode();
            }
        }
    }
}