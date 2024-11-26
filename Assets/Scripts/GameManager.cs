using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI Elements
    [SerializeField] private GameObject gameTitleScreen;
    [SerializeField] private Button gameStartButton;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Button gameRestartButton;

    [SerializeField] private GameObject thankYouScreen;
    [SerializeField] private Button playAgainButton;

    [SerializeField] private TextMeshProUGUI gameTimeText;

    // Prefabs
    [SerializeField] private GameObject codeTextPrefab;
    [SerializeField] private GameObject codeNumberPrefab;
    [SerializeField] private GameObject codeEnergyPrefab;

    [SerializeField] private GameObject vehiclePrefab;
    [SerializeField] private GameObject explosionPrefab;

    // Instantiation and Its Script References
    private GameObject newVehicle;
    public VehicleScript vehicleScript;

    private GameObject newCodeText;
    private CodeText codeTextScript;

    private GameObject newCodeNumber;
    private CodeNumber codeNumberScript;

    private GameObject newCodeEnergy;
    private CodeEnergy codeEnergyScript;

    // Game State
    private int gameTime = 300;
    public bool nukeIsActivated;

    // Position
    private float xCodeSpawnRange = 5.81f;
    private float yCodeSpawnRange = -2.09f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("An instance of Game Manager exists.");
        }
        Instance = this; // Declare an instance of Game Manager
    }

    private void Start()
    {
        // Disable time text
        gameTimeText.gameObject.SetActive(false);

        // Trigger functions by mouse click
        gameStartButton.onClick.AddListener(HandleGameStart);
        gameRestartButton.onClick.AddListener(HandleGameRestart);
        playAgainButton.onClick.AddListener(HandlePlayAgain);
    }

    private void Update()
    {
        ActivateNukeCode(); // Continuously check nuke condition
    }

    public void HandleGameStart() // Triggered by Start Button
    {
        // Disable button to prevent multiple clicks
        gameStartButton.interactable = false;

        // Key Objects and UI Elements
        SpawnAndMoveVehicle();
        StartCoroutine(HandleGameTime());
        StartCoroutine(HandleTitleScreen());
    }

    private void SpawnAndMoveVehicle()
    {
        // Instantiation References
        Vector3 vehicleSpawnPosition = new Vector3(-12f, -3.45f, 0f);
        newVehicle = Instantiate(vehiclePrefab, vehicleSpawnPosition, Quaternion.identity);

        // Script in Instantiation, Run Coroutine
        vehicleScript = newVehicle.GetComponent<VehicleScript>();
        StartCoroutine(vehicleScript.MoveVehicleToward());
    }

    public void InstantiateNukeCode()
    {
        // Instantiation References and Its Script
        Vector3 codeTextPosition = new Vector3(-xCodeSpawnRange, yCodeSpawnRange, 0f);
        newCodeText = Instantiate(codeTextPrefab, codeTextPosition, Quaternion.identity);
        codeTextScript = newCodeText.GetComponent<CodeText>();

        Vector3 codeNumberPosition = new Vector3(0f, yCodeSpawnRange, 0f);
        newCodeNumber = Instantiate(codeNumberPrefab, codeNumberPosition, Quaternion.identity);
        codeNumberScript = newCodeNumber.GetComponent<CodeNumber>();

        Vector3 codeEnergyPosition = new Vector3(xCodeSpawnRange, yCodeSpawnRange, 0f);
        newCodeEnergy = Instantiate(codeEnergyPrefab, codeEnergyPosition, Quaternion.identity);
        codeEnergyScript = newCodeEnergy.GetComponent<CodeEnergy>();
    }

    private void ActivateNukeCode() 
    {
        // Set nuke condition from 3 code conditions
        if (codeTextScript.codeTextActivated && codeNumberScript.codeNumberActivated && codeEnergyScript.codeEnergyActivated)
        {
            nukeIsActivated = true; // [Nuke Script]
            HandleDestroyObject(5); // Destroy first 5 objects in array
        }
    }

    private IEnumerator HandleGameTime()
    {
        while (!nukeIsActivated)
        {
            // Display and countdown time
            gameTimeText.gameObject.SetActive(true);
            gameTimeText.text = "Impact In: " + gameTime;
            yield return new WaitForSeconds(1f);
            gameTime--;

            if (gameTime < 0)
            {
                HandleGameOver();
                yield break; // Stop coroutine
            }
        }
    }

    private IEnumerator HandleTitleScreen()
    {
        float titleScreenSpeed = 5f;
        Vector3 titleScreenStopPoint = new Vector3(0f, 7.5f, 0f);

        // Move and disable Title Screen
        while (gameTitleScreen.transform.position != titleScreenStopPoint)
        {
            gameTitleScreen.transform.position = Vector3.MoveTowards(gameTitleScreen.transform.position, titleScreenStopPoint, titleScreenSpeed * Time.deltaTime);
            yield return null;
        }
        gameTitleScreen.gameObject.SetActive(false);
    }

    private void HandleGameOver()
    {
        // Set position values to spawn explosions
        float[] xExplosionPoint = { -5.5f, 0f, 5.5f };
        float yExplosionPoint = 1.5f;

        foreach (float x in xExplosionPoint) // Execute for each value respectively
        {
            Vector3 explosionPosition = new Vector3(x, -yExplosionPoint, 0f);
            Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);            
        }

        // Destroy all 8 objects in array
        HandleDestroyObject(8);

        // As game is over, enable Restart Screen
        StartCoroutine(HandleRestartScreen());
    }

    private void HandleDestroyObject(int objectCount) // As game is over, destroy irrelevant objects
    {
        // Store all objects to be destroyed in an array
        GameObject[] objectToDestroy =
        {
            newCodeText,
            newCodeNumber,
            newCodeEnergy,
            codeEnergyScript?.currentEnergy,
            codeEnergyScript?.newRobo,
            newVehicle,
            vehicleScript?.newBiker,
            vehicleScript?.newPunk,
        };

        // Limit count to within array length
        objectCount = Mathf.Clamp(objectCount, 0, objectToDestroy.Length);

        // Destroy i number of objects through array
        for (int i = 0; i < objectCount; i++)
        {
            if (objectToDestroy[i] != null)
            {
                Destroy(objectToDestroy[i]);
            }
        }
    }

    private IEnumerator HandleRestartScreen()
    {
        // As game is over, enable Restart Screen
        gameOverScreen.gameObject.SetActive(true);

        float gameOverScreenSpeed = 5f;
        Vector3 gameOverScreenStopPoint = new Vector3(0f, 3f, 0f);

        // Move and enable Restart Screen
        while (gameOverScreen.transform.position != gameOverScreenStopPoint)
        {
            gameOverScreen.transform.position = Vector3.MoveTowards(gameOverScreen.transform.position, gameOverScreenStopPoint, gameOverScreenSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void HandleGameRestart()
    {
        // Disable button to prevent multiple clicks
        gameRestartButton.interactable = false;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public IEnumerator HandleThankYouScreen() // [Vehicle Script]
    {
        // As game is won, move and enable ThankYou Screen
        thankYouScreen.gameObject.SetActive(true);

        float playAgainScreenSpeed = 0.025f; // Small due to Fixed Updated in Vehicle Script
        Vector3 playAgainScreenStopPoint = new Vector3(0f, 3f, 0f);
        
        while (thankYouScreen.transform.position != playAgainScreenStopPoint)
        {
            thankYouScreen.transform.position = Vector3.MoveTowards(thankYouScreen.transform.position, playAgainScreenStopPoint, playAgainScreenSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void HandlePlayAgain()
    {
        // Disable button to prevent multiple clicks
        playAgainButton.interactable = false;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}