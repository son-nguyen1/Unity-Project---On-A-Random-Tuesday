using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunkScript : MonoBehaviour
{
    [SerializeField] private float punkMoveSpeed = 1f;

    // Position
    private float xStopPoint = 0.25f;
    private Vector3 punkStopPosition;

    private float xBackPoint = 2.5f;
    private Vector3 punkBackPosition;

    // Animation
    private Animator punkAnimator;
    private string Punk_Is_Moving = "PunkIsMoving";
    private string Punk_Is_Returning = "PunkIsReturning";

    // Script Reference
    private NukeDialogue nukeDialogueScript;

    private bool asteroidIsDestroyed;

    private void Start()
    {
        // Set a stop position
        punkStopPosition = new Vector3(xStopPoint, transform.position.y, 0f);

        // Play moving animation
        punkAnimator = GetComponent<Animator>();
        punkAnimator.SetBool(Punk_Is_Moving, true);

        // Script Reference
        nukeDialogueScript = GetComponent<NukeDialogue>();
    }

    private void FixedUpdate()
    {
        if (!asteroidIsDestroyed) // Continuously check move condition
        {
            PunkMoveToward();
        }
        else
        {
            PunkMoveBack();
        }
    }

    private void PunkMoveToward()
    {
        // Move punk to stop position
        transform.position = Vector3.MoveTowards(transform.position, punkStopPosition, punkMoveSpeed * Time.deltaTime);

        if (transform.position == punkStopPosition)
        {
            // Remain at stop position
            transform.position = punkStopPosition;
            punkAnimator.SetBool(Punk_Is_Moving, false); // Play idle animation

            // Script Reference [Nuke Dialogue]
            if (nukeDialogueScript.dialogueIsOver)
            {
                asteroidIsDestroyed = true; // Check condition in [Nuke Dialogue]
            }
        }
    }

    private void PunkMoveBack()
    {
        // Move punk to back position
        punkBackPosition = new Vector3(-xBackPoint, transform.position.y, 0f);
        transform.position = Vector3.MoveTowards(transform.position, punkBackPosition, punkMoveSpeed * Time.deltaTime);

        punkAnimator.SetBool(Punk_Is_Returning, true); // Play return animation

        if (transform.position == punkBackPosition)
        {
            Destroy(gameObject); // Destroy as back point is reached
        }
    }
}