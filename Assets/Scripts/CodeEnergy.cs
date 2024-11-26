using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CodeEnergy : MonoBehaviour
{
    public static CodeEnergy Instance { get; private set; }

    // Prefabs
    [SerializeField] private GameObject roboPrefab;
    [SerializeField] private GameObject energyPrefab;

    // UI Elements
    [SerializeField] private Button energyStartButton;
    [SerializeField] private TextMeshProUGUI energyPrompt;
    [SerializeField] private TextMeshProUGUI energyCountText;

    // Sounds
    private AudioSource codeEnergyAudio;
    [SerializeField] private AudioClip spawnSound;

    // Instantiation References [Game Manager]
    public GameObject newRobo;
    public GameObject currentEnergy;

    private Vector3 centralPoint = new Vector3(6f, -2f, 0f);
    private int energyCount = 25;

    // 3rd Code Condition
    public bool codeEnergyActivated;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 Code Energy instance");
        }
        Instance = this; // Declare an instance of Code Energy
    }

    private void Start()
    {
        // Enable energy prompt and disable count
        energyPrompt.gameObject.SetActive(true);
        energyCountText.gameObject.SetActive(false);

        // Get component in child object
        energyStartButton = GetComponentInChildren<Button>();
        codeEnergyAudio = GetComponent<AudioSource>();

        // Trigger function by mouse click
        energyStartButton.onClick.AddListener(StartCodeEnergy);
    }

    private void StartCodeEnergy()
    {
        // Key Objects and UI Elements
        SpawnRoboMan();
        SpawnEnergyCore();
        HandleUserInterface();
    }

    private void SpawnRoboMan()
    {
        // Instantiation Reference, Spawn
        newRobo = Instantiate(roboPrefab, centralPoint, Quaternion.identity);
    }

    public void SpawnEnergyCore()
    {
        // ONLY SPAWN WHEN COUNT IS OVER 0 [ENERGY SCRIPT)]
        if (energyCount > 0)
        {
            // Only 1 energy can exist at any time
            if (currentEnergy != null)
            {
                Destroy(currentEnergy);
            }

            // Spawn at random positions, Instantiation Reference
            float xPosition = 1.5f;
            float yPosition = 1.5f;
            float xRandomPosition = Random.Range(-xPosition, xPosition);
            float yRandomPosition = Random.Range(-yPosition, yPosition);
            Vector3 randomOffset = new Vector3(xRandomPosition, yRandomPosition, 0f);

            Vector3 energySpawnPosition = centralPoint + randomOffset;
            currentEnergy = Instantiate(energyPrefab, energySpawnPosition, Quaternion.identity);
            codeEnergyAudio.PlayOneShot(spawnSound, 1f); // Play spawn sound
        }
    }

    private void HandleUserInterface()
    {
        // Hide energy prompt and Start button
        energyPrompt.gameObject.SetActive(false);
        energyStartButton.gameObject.SetActive(false);

        // Display and update count
        energyCountText.gameObject.SetActive(true);
        energyCountText.text = energyCount.ToString();
    }

    public void HandleEnergyCount()
    {
        // UPDATE COUNT AS LONG AS OVER 0 [ENERGY SCRIPT)]
        if (energyCount > 0)
        {            
            energyCount--;
            energyCountText.text = energyCount.ToString();

            // AS COUNT REACHES 0
            if (energyCount == 0)
            {
                codeEnergyActivated = true; // Confirm 3rd Code Condition
                energyPrompt.gameObject.SetActive(true);
                energyCountText.gameObject.SetActive(false);
                energyPrompt.text = "Code Energy Activated"; // Display activation text
                Destroy(newRobo);
            }
        }
    }
}