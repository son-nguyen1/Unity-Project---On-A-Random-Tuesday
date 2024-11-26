using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerScript : MonoBehaviour
{
    [SerializeField] private float bikerMoveSpeed = 0.75f;

    // Position
    private float xStopPoint = 1.5f;
    private Vector3 bikerStopPosition;

    private float xBackPoint = 2.5f;
    private Vector3 bikerBackPoint;

    // Animation
    private Animator bikerAnimator;
    private string Biker_Is_Moving = "BikerIsMoving";
    private string Biker_Is_Returning = "BikerIsReturning";

    // Script Reference
    private NukeDialogue nukeDialogueScript;

    private bool asteroidIsDestroyed;
    public bool bikerAndPunkInCar;

    private void Start()
    {
        // Set a stop point
        bikerStopPosition = new Vector3(xStopPoint, transform.position.y, 0f);

        // Play default moving animation
        bikerAnimator = GetComponent<Animator>();        
        bikerAnimator.SetBool(Biker_Is_Moving, true);
    }

    private void FixedUpdate()
    {
        if (!asteroidIsDestroyed) // Continuously check move condition
        {
            BikerMoveToward();
        }
        else
        {
            BikerMoveBack();
        }
    }

    private void BikerMoveToward()
    {
        // Move biker to stop position
        transform.position = Vector3.MoveTowards(transform.position, bikerStopPosition, bikerMoveSpeed * Time.deltaTime);

        if (transform.position == bikerStopPosition)
        {
            // Remain at stop point
            transform.position = bikerStopPosition;            
            bikerAnimator.SetBool(Biker_Is_Moving, false); // Play idle animation

            // Script Reference [Nuke Dialogue]
            nukeDialogueScript = GameManager.Instance.vehicleScript.newPunk.GetComponent<NukeDialogue>();

            if (nukeDialogueScript.dialogueIsOver)
            {
                asteroidIsDestroyed = true; // Check condition in [Nuke Dialogue]
            }
        }
    }

    private void BikerMoveBack()
    {
        // Move biker to back position
        bikerBackPoint = new Vector3(-xBackPoint, transform.position.y, 0f);
        transform.position = Vector3.MoveTowards(transform.position, bikerBackPoint, bikerMoveSpeed * Time.deltaTime);

        bikerAnimator.SetBool(Biker_Is_Returning, true); // Play return animation

        if (transform.position == bikerBackPoint)
        {
            gameObject.SetActive(false);
            bikerAndPunkInCar = true; // [Vehicle Script]
        }
    }
}