using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletMoveSpeed = 8f;

    private Rigidbody2D bulletRB2D;
    private Vector3 bulletMoveDir;

    // Position
    private float xLeftRange = 3.75f;
    private float xRightRange = 8f;
    private float yUpperRange = -0.25f;
    private float yLowerRange = -4f;

    private void Start()
    {
        bulletRB2D = GetComponent<Rigidbody2D>(); // Get component
    }

    private void FixedUpdate()
    {
        // Continuously move bullet toward direction
        bulletRB2D.velocity = bulletMoveDir * bulletMoveSpeed;

        // Continuously destroy object out-of-bound
        CheckBoundsAndDestroyBullet();
    }

    public void SetBulletDirection(Vector3 machineMoveDirection) // [Robo Script]
    {
        // Set bullet movement as robo movement
        bulletMoveDir = machineMoveDirection;
    }

    private void CheckBoundsAndDestroyBullet()
    {
        // Destroy bullets when out of bounds
        if (transform.position.x < xLeftRange || transform.position.x > xRightRange ||
            transform.position.y > yUpperRange || transform.position.y < yLowerRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Destroy both when hit
        if (collider.gameObject.GetComponent<CircleCollider2D>() != null) // Only collide object with circle collider
        {
            Destroy(gameObject);
            Destroy(collider.gameObject);
        }
    }
}