using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour
{
    [SerializeField] private float vehicleMoveSpeed = 1.25f;

    // Prefabs
    [SerializeField] private GameObject bikerPrefab;
    [SerializeField] private GameObject punkPrefab;

    // Instantiation and Its Script References [Game Manager]
    public GameObject newBiker;
    private BikerScript bikerScript;

    public GameObject newPunk;

    // Position
    private float xStopPoint = 2.5f;
    private Vector3 vehicleStopPoint;

    private float xOffscreenPoint = 12f;

    // Animation
    private Animator vehicleAnimator;
    private string Vehicle_Is_Moving = "VehicleIsMoving";

    private void Start()
    {
        // Set a stop position
        vehicleStopPoint = new Vector3(-xStopPoint, transform.position.y, 0f);

        // Play moving animation
        vehicleAnimator = GetComponent<Animator>();
        vehicleAnimator.SetBool(Vehicle_Is_Moving, true);
    }

    private void FixedUpdate()
    {
        // Instantiation, get Its Script
        bikerScript = newBiker.GetComponent<BikerScript>();

        if (bikerScript.bikerAndPunkInCar)        
        {
            MoveVehicleOffscreen(); // Continuously check condition in [Biker Script]  
        }
    }

    public IEnumerator MoveVehicleToward() // [Game Manager]
    {
        // Wait before movement
        yield return new WaitForSeconds(2f);

        // Move vehicle to stop position
        while (transform.position != vehicleStopPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, vehicleStopPoint, vehicleMoveSpeed * Time.deltaTime);
            yield return null; // Run over multiple frames, not instantly
        }
        vehicleAnimator.SetBool(Vehicle_Is_Moving, false); // Play idle animation

        // Key Objects
        SpawnAndMoveBiker();
        SpawnAndMovePunk();
    }

    private void MoveVehicleOffscreen()
    {
        // [Game Manager]
        StartCoroutine(GameManager.Instance.HandleThankYouScreen());

        Vector3 offscreenPoint = new Vector3(xOffscreenPoint, transform.position.y, 0f);
        transform.position = Vector3.MoveTowards(transform.position, offscreenPoint, vehicleMoveSpeed * Time.deltaTime);

        vehicleAnimator.SetBool(Vehicle_Is_Moving, true); // Play moving animation

        // Destroy as offscreen position is reached
        if (transform.position == offscreenPoint)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnAndMoveBiker()
    {
        // Spawn Position, Instantiation Reference
        Vector3 bikerOffset = new Vector3(1f, -0.2f, 0f);
        Vector3 bikerSpawnPosition = transform.position + bikerOffset;
        newBiker = Instantiate(bikerPrefab, bikerSpawnPosition, Quaternion.identity);
    }

    public void SpawnAndMovePunk()
    {
        // Spawn Position, Instantiation Reference
        Vector3 punkOffset = new Vector3(-1f, -0.2f, 0f);
        Vector3 punkSpawnPosition = transform.position + punkOffset;
        newPunk = Instantiate(punkPrefab, punkSpawnPosition, Quaternion.identity);
    }
}