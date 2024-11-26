using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboScript : MonoBehaviour
{
    [SerializeField] private float machineMoveSpeed = 3f;
    [SerializeField] private GameObject bulletPrefab;

    private Vector3 machineMoveDir;

    // Position
    private float xLeftRange = 4f;
    private float xRightRange = 7.75f;
    private float yUpperRange = -0.5f;
    private float yLowerRange = -3.75f;

    // Animation
    private Animator machineAnim;
    private const string Machine_Is_Moving = "MachineIsMoving";
    private bool machineIsMoving;
    private const string Machine_Is_Shooting = "MachineIsShooting";

    private void Start()
    {
        machineAnim = GetComponent<Animator>(); // Get component
    }

    private void Update()
    {
        MachineGuyShoot();
        MachineGuyMove();
        MachineMoveInbound();
    }

    private void MachineGuyMove()
    {
        // Press key to move on axis by 1
        Vector2 inputVector = new Vector2(0f, 0f).normalized;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }

        // Moves on axis with input
        machineMoveDir = new Vector3(inputVector.x, inputVector.y, 0f).normalized;
        transform.position += machineMoveDir * machineMoveSpeed * Time.deltaTime;

        // When not standing still, play moving animation
        machineIsMoving = (machineMoveDir != Vector3.zero);
        machineAnim.SetBool(Machine_Is_Moving, machineIsMoving);
    }

    private void MachineMoveInbound()
    {
        // Always get values within the ranges, no lower or higher
        float xMoveRange = Mathf.Clamp(transform.position.x, xLeftRange, xRightRange);
        float yMoveRange = Mathf.Clamp(transform.position.y, yLowerRange, yUpperRange);

        // Never go beyond the ranges
        transform.position = new Vector3(xMoveRange, yMoveRange, transform.position.z);
    }

    private void MachineGuyShoot()
    {
        // Press to spawn bullets
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Play shooting animation
            machineAnim.SetBool(Machine_Is_Shooting, true);

            // Instantiation Reference (AN INSTANTIATION IS DIFFERENT THAN A PREFAB)
            GameObject spawnBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Instantiation Script, Send Movement Position [Bullet Script]
            BulletScript bulletScript = spawnBullet.GetComponent<BulletScript>();
            bulletScript.SetBulletDirection(machineMoveDir);

            // Send right movement when not moving (so bullet can still fly)
            if (machineMoveDir == Vector3.zero)
            {
                bulletScript.SetBulletDirection(Vector3.right);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Stop shooting animation
            machineAnim.SetBool(Machine_Is_Shooting, false);
        }
    }
}