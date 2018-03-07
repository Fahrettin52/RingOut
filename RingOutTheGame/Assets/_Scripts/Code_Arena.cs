using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Arena : MonoBehaviour {

    public float wholeBlinkingTime;
    public float blinkingTime;
    public float initialCrumbleTime;
    public float timeBetweenCrumbles;
    public float timeTillDeactivation;
    public float crumbleSpeed;
    private float startPos;
    private int currentArenaPart;
    public int[] groupsToCrumble;
    private int currentGroupToCrumble;
    private List<Transform> arenaParts = new List<Transform>();

	// Use this for initialization
	void Start () {
        // Safe the arenas y position for later use
        startPos = transform.position.y;

        FillArenaPartsList();

        StartCoroutine(CrumbleTimer(initialCrumbleTime));
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
        CrumbleSelector();
    }

    // Counts down to when the fallen part should be deactivated
    private IEnumerator DeactivateFallenPart(float deactivationTime, Transform fallenPart) {
        yield return new WaitForSeconds(deactivationTime);
        fallenPart.gameObject.SetActive(false);
    }
    
    // Selects the group that should 
    private void CrumbleSelector() {
        int groupMembers = groupsToCrumble[currentGroupToCrumble];
        for (int i = 0; i < groupMembers ; i++) {
            StartCoroutine(Blink(wholeBlinkingTime, arenaParts[currentArenaPart]));
            currentArenaPart++;
        }

        // Invokes SelectNextGroupToCrumble after checking if it's not the round in the cycle
        if (currentGroupToCrumble < groupsToCrumble.Length - 1) {
            Invoke("SelectNextGroupToCrumble", timeBetweenCrumbles);
        }        
    }

    // Selects the next group of parts that need to fall down
    public void SelectNextGroupToCrumble() {
        currentGroupToCrumble++;
        // Call CrumbleTimer to continue the cycle
        StartCoroutine(CrumbleTimer(timeBetweenCrumbles));
    }

    // Indicates the parts that are about to crumble 
    public IEnumerator Blink(float waitTime, Transform arenaPart)
    {
        Renderer arenaRenderer = arenaPart.GetComponent<Renderer>();
        float endTime = Time.time + waitTime;
        while (Time.time < endTime)
        {
            // TODO change turning the renderer on and off to changing it's Colour
            arenaRenderer.enabled = false;
            yield return new WaitForSeconds(blinkingTime);
            arenaRenderer.enabled = true;
            yield return new WaitForSeconds(blinkingTime);
        }

        CrumbleProcess(arenaPart);
    }

    // Crumbles the current part of the arena.
    private void CrumbleProcess(Transform currentPart) {
        // Caching the arenaParts[currentArenaPart] into local variables
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
    }
}
