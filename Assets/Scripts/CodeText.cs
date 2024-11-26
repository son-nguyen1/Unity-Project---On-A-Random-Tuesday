using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CodeText : MonoBehaviour
{
    // UI Elements
    [SerializeField] private Button textStartButton;
    [SerializeField] private TextMeshProUGUI textPrompt;
    [SerializeField] private TMP_InputField playerTextInput;

    // Sounds
    private AudioSource codeTextAudio;
    [SerializeField] private AudioClip resetSound;

    private string displayPrompt = "Code Red: Asteroid impact imminent. Activate 'PROTOCOL-16/07#'";

    // 1st Code Condition
    public bool codeTextActivated;

    private void Start()
    {
        // Disable prompt text
        textPrompt.gameObject.SetActive(false);

        // Get components in child object
        textStartButton = GetComponentInChildren<Button>();
        codeTextAudio = GetComponent<AudioSource>();

        // Trigger function by mouse click
        textStartButton.onClick.AddListener(StartCodeText);

        if (textPrompt != null)
        {
            // Prompt text is displayed
            textPrompt.text = displayPrompt;
        }
    }

    private void Update()
    {
        CheckPlayerTextInput(); // Continuously check characters 1-by-1
    }

    private void StartCodeText()
    {
        // Enable prompt and disable Start Button
        textPrompt.gameObject.SetActive(true);
        textStartButton.gameObject.SetActive(false);
    }

    private void CheckPlayerTextInput()
    {
        string playerInputText = playerTextInput.text;

        // Check if input matches prompt text
        if (playerInputText.Length > 0)
        {
            // Look at prompt, then check input
            if (!displayPrompt.StartsWith(playerInputText))
            {
                // Reset player's input
                playerTextInput.text = string.Empty;
                codeTextAudio.PlayOneShot(resetSound, 1f); // Play reset sound
            }
            else if (playerInputText == displayPrompt)
            {
                codeTextActivated = true; // Confirm 1st Code Condition
                textPrompt.text = "Code Text Activated"; // Display activation text
            }
        }
    }
}
