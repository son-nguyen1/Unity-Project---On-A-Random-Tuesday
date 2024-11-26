using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyScript : MonoBehaviour
{
    [SerializeField] private float energyMoveSpeed = 5f;
    [SerializeField] private GameObject roboPrefab;

    private Rigidbody2D energyRB2D;    

    // Position
    private float xLeftRange = 4f;
    private float xRightRange = 7.75f;
    private float yUpperRange = -0.5f;
    private float yLowerRange = -3.75f;
    private Vector3 targetPosition;

    // Invoke Repeating
    private const string Set_Random_Position = "SetRandomPosition";
    private float repeatInterval = 0.5f;

    private void Start()
    {
        energyRB2D = GetComponent<Rigidbody2D>(); // Get component

        // Trigger function at a repeated rate
        targetPosition = transform.position; // Avoid unpredictable movement
        InvokeRepeating(Set_Random_Position, 0f, repeatInterval);
    }

    private void Update()
    {
        SetInboundPosition(); // Continuously keep object in-bound

        // Continuously move energy to target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, energyMoveSpeed * Time.deltaTime);
    }

    private void SetRandomPosition()
    {
        // Set random in-bound positions
        float xRandomPosition = Random.Range(xLeftRange, xRightRange);
        float yRandomPosition = Random.Range(yLowerRange, yUpperRange);
        targetPosition = new Vector3(xRandomPosition, yRandomPosition, 0f);
    }

    private void SetInboundPosition()
    {
        // Always get values within the ranges, no lower or higher
        float xMoveRange = Mathf.Clamp(transform.position.x, xLeftRange, xRightRange);
        float yMoveRange = Mathf.Clamp(transform.position.y, yLowerRange, yUpperRange);

        // Never go beyond the ranges
        transform.position = new Vector3(xMoveRange, yMoveRange, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Destroy both when hit
        if (collider.gameObject.GetComponent<CircleCollider2D>() != null) // Only collide object with circle collider
        {
            Destroy(gameObject);
            Destroy(collider.gameObject);

            // CALL [CODE ENERGY] TO UPDATE COUNT (ALLOW TO SPAWN IF > 0)
            if (CodeEnergy.Instance != null)
            {
                CodeEnergy.Instance.HandleEnergyCount();
            }

            // CALL [CODE ENERGY] TO SPAWN (IF COUNT > 0, GO AFTER COUNT CALL SO NO EXTRA SPAWN)
            if (CodeEnergy.Instance != null)
            {
                CodeEnergy.Instance.SpawnEnergyCore();
            }
        }        
    }
}