using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Arena : MonoBehaviour {
    [Header("Blinking Process")]
    public float wholeBlinkingTime;
    public float blinkingTime;
    public Color blinkingColor;
    [Header("Crumble Process")]
    public float initialCrumbleTime;
    public float timeBetweenCrumbles;
    public float crumbleSpeed;
    [Header("(De)Activating Objects")]
    public float timeTillDeactivation;
    [Header("Arena Parts to Crumble Collectively")]
    public int[] groupsToCrumble;

    private float startPos;
    private int currentArenaPart;    
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
        Color arenaPartColor = arenaRenderer.material.color;
        float endTime = Time.time + waitTime;
        while (Time.time < endTime)
        {
            // PingPongs the color of crumbling arenaPart
            float lerpBlinkingColor = Mathf.PingPong(Time.time, blinkingTime) / blinkingTime;
            arenaRenderer.material.color = Color.Lerp(arenaPartColor, blinkingColor, lerpBlinkingColor); 
            yield return new WaitForSeconds(blinkingTime);
        }

        //Sets the color of the arenaPart back to it's original so that it never crumbles whilst in the blinkingColors value
        arenaRenderer.material.color = arenaPartColor;

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
