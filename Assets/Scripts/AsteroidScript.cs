using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] private float asteroidMoveSpeed = 0.05f;

    // Position
    private float xStopPoint = 7.5f;
    private float yStopPoint = 2.5f;
    private Vector3 asteroidStopPosition;

    private void Start()
    {
        // Set a stop position
        asteroidStopPosition = new Vector3(xStopPoint, yStopPoint, 0f);
    }

    private void FixedUpdate()
    {
        MoveAsteroidToward(); // Move continuously
    }

    public void MoveAsteroidToward()
    {
        // Move asteroid to stop position
        transform.position = Vector3.MoveTowards(transform.position, asteroidStopPosition, asteroidMoveSpeed * Time.deltaTime);

        if (transform.position == asteroidStopPosition)
        {
            // Remain at stop position
            transform.position = asteroidStopPosition;
        }
    }
}