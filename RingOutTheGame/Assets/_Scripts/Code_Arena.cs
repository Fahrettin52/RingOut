using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Arena : MonoBehaviour {

    public float timeBetweenCrumbles;
    public float timeTillDeactivation;
    public float crumbleSpeed;
    private float startPos;
    private int currentArenaPart;
    private List<Transform> arenaParts = new List<Transform>();

	// Use this for initialization
	void Start () {
        // Safe the arenas y position for later use
        startPos = transform.position.y;

        FillArenaPartsList();
        StartCoroutine(CrumbleTimer(timeBetweenCrumbles));
    }

    // Fills the arenaParts List
    private void FillArenaPartsList() {
        foreach (Transform child in transform) {
            arenaParts.Add(child);
        }
    }

    // Counts down for each part that'll crumble
    private IEnumerator CrumbleTimer(float crumbleTimer) {
        yield return new WaitForSeconds(crumbleTimer);
        CrumbleProcess();
    }

    // Counts down to when the fallen part should be deactivated
    private IEnumerator DeactivateFallenPart(float deactivationTime, Transform fallenPart) {
        yield return new WaitForSeconds(deactivationTime);
        fallenPart.gameObject.SetActive(false);
    }

    // Crumbles the current part of the arena.
    private void CrumbleProcess() {
        // Caching the arenaParts[currentArenaPart] into local variables
        Transform currentPart = arenaParts[currentArenaPart];
        Rigidbody currentPartRigidbody = currentPart.gameObject.GetComponent<Rigidbody>();

        // Moving the currentpart under the arena so the convexed collider doesn't glitch into the other parts of the arena
        while (currentPart.position.y > (startPos - currentPart.localScale.y )) {
            currentPart.Translate(-Vector3.up * crumbleSpeed * Time.deltaTime);
        }

        // Once the currentPart is under the arena, we let gravity do the rest naturally
        currentPartRigidbody.useGravity = true;
        currentPartRigidbody.isKinematic = false;

        // Calls the DeactivateFallenPart Coroutine so that after a while the currentPart gets deactivated
        StartCoroutine(DeactivateFallenPart(timeTillDeactivation, currentPart));

        // Increase cuurentArenaPart with 1, so we can call the next one
        currentArenaPart++;

        // Checking if currentpart is not the final piece of the arena
        if (currentArenaPart != arenaParts.Count - 1) { 
            // Call CrumbleTimer to continue the cycle
            StartCoroutine(CrumbleTimer(timeBetweenCrumbles));
        }        
    }
}
