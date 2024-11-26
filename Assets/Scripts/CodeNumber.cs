using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CodeNumber : MonoBehaviour
{
    // UI Elements
    [SerializeField] private Button numberStartButton;
    [SerializeField] private TextMeshProUGUI numberPrompt;
    [SerializeField] private TMP_InputField playerNumberInput;

    // Sounds
    private AudioSource codeNumberAudio;
    [SerializeField] private AudioClip resetSound;

    private int randomNumber; // Store the random number
    private int rangeIndex; // Position of sub-range within range

    // Create a jagged array (array with many smaller arrays)
    private readonly long[][] rangesOfRangeIndex = new long[][]
    {
        new long [] { 1, 10 }, // Index 0: a range of single digit number
        new long [] { 10, 100 },
        new long [] { 100, 1000 },
        new long [] { 1000, 10000 },
        new long [] { 10000, 100000 },
        new long [] { 100000, 1000000 },
        new long [] { 1000000, 10000000 },
        new long [] { 10000000, 100000000 },
        new long [] { 100000000, 1000000000 },
        new long [] { 1000000000, 10000000000 } // Index 9: a range of 10-digit number
    };

    // 2nd Code Condition
    public bool codeNumberActivated;

    private void Start()
    {
        // Disable prompt number
        numberPrompt.gameObject.SetActive(false);

        // Get component in child object
        numberStartButton = GetComponentInChildren<Button>();
        codeNumberAudio = GetComponentInChildren<AudioSource>();

        // Trigger function by mouse click
        numberStartButton.onClick.AddListener(StartCodeNumber);
    }

    private void StartCodeNumber()
    {
        // Enable prompt number and disable Start Button
        numberPrompt.gameObject.SetActive(true);
        numberStartButton.gameObject.SetActive(false);

        // Player are unable to input
        playerNumberInput.interactable = false;

        // Set number sub-range at 1st range (index 0)
        rangeIndex = 0;

        // Run number prompt, let player input
        StartCoroutine(GenerateAndHideNumber(() => playerNumberInput.interactable = true)); // Coroutine Callback
    }

    private IEnumerator GenerateAndHideNumber(System.Action onCoroutineComplete) // Callback Parameter
    {
        // Index must be non-negative and within ranges (0 <= '0-9' < 10)
        if (rangeIndex >= 0 && rangeIndex < rangesOfRangeIndex.Length)
        {
            // Determine min and max number from a sub-range position
            long minNumberInRange = rangesOfRangeIndex[rangeIndex][0]; // access lower bound of range (0)
            long maxNumberInRange = rangesOfRangeIndex[rangeIndex][1]; // access upper bound of range (1)

            // Generate and display random number within that min and max number
            randomNumber = Random.Range((int)minNumberInRange, (int)maxNumberInRange);
            numberPrompt.text = randomNumber.ToString();

            // Hide prompt number after x seconds
            yield return new WaitForSeconds(4f);
            numberPrompt.text = "Answer";

            // As coroutine is done, run callback
            onCoroutineComplete?.Invoke();
        }
        else
        {
            Debug.LogError("rangeIndex out of bounds.");
        }
    }

    public void CheckPlayerNumberInput() // [Button Event - On End Edit]
    {
        // Remove whitespaces from input
        string playerTrueInput = playerNumberInput.text.Trim();

        // Check if input matches prompt number
        if (playerTrueInput == randomNumber.ToString())
        {
            // As number sub-range is within range, run next number prompt
            if (rangeIndex < rangesOfRangeIndex.Length - 1)
            {
                rangeIndex++;
                playerNumberInput.text = string.Empty;
                playerNumberInput.interactable = false;
                StartCoroutine(GenerateAndHideNumber(() => playerNumberInput.interactable = true));
                Debug.Log(randomNumber);
            }
            else
            {
                codeNumberActivated = true; // Confirm 2nd Code Condition
                numberPrompt.text = "Code Number Activated"; // Display activation text
            }
        }
        else
        {
            // Reset number prompt to first sub-range
            rangeIndex = 0;
            playerNumberInput.text = string.Empty;
            codeNumberAudio.PlayOneShot(resetSound, 1f); // Play reset sound
            playerNumberInput.interactable = false;
            StartCoroutine(GenerateAndHideNumber(() => playerNumberInput.interactable = true));
        }
    }
}