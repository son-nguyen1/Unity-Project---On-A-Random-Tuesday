using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeScript : MonoBehaviour
{
    [SerializeField] private float nukeMoveSpeed = 5f;

    // Game Objects and Prefabs
    [SerializeField] private GameObject asteroid;
    [SerializeField] private GameObject explosionPrefab;

    private void FixedUpdate()
    {
        MoveNukeToward(); // Move continuously
    }

    private void MoveNukeToward()
    {
        // Set movement from nuke condition
        if (GameManager.Instance.nukeIsActivated == true)
        {
            // Move nuke to asteroid’s position
            transform.position = Vector3.MoveTowards(transform.position, asteroid.transform.position, nukeMoveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Destroy, Spawn explosions and dialogue
        if (collider.gameObject.GetComponent<CircleCollider2D>() != null)
        {
            // Instantiation Script (in New Punk, created by Vehicle Script, from New Vehicle in Game Manager)
            NukeDialogue nukeDialogueScript = GameManager.Instance.vehicleScript.newPunk.GetComponent<NukeDialogue>();
            nukeDialogueScript.StartNukeDialogue(); // [Nuke Dialogue]

            // As dialogue is triggered, disable
            gameObject.SetActive(false);
            Destroy(collider.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}