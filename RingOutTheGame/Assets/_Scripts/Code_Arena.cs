using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Arena : MonoBehaviour {
    private GameObject gameMng; // The GameMng GameObject in the scene
    public BouncerControl bouncer;

    public GameObject[] rings; // The rings that have to be crumbled
    private int currentRing; // The current ring selecting in the rings Array
    public float ringActivationTime; // The time it takes between activating the rings
    public int ringsToBeSaved; // Determines when the activations stop, 0 means every member of rings gets activated
    private int ringsLengthDecreaser; // The value with which to determine the last index of rings, has to be a minimum of 1

    // Use this for initialization
    void Start () {
        // Selects the gameMng
        gameMng = GameObject.FindGameObjectWithTag("GameMng");

        // To set a minimum for this variable
        if (ringsToBeSaved == 0) {
            ringsToBeSaved = 1;
        }
    }

    // Calls the TimeBEtweenActivations so that the fall process doesn't start immediately
    public void ActivateFallProcess() {
        StartBouncer();
        StartCoroutine(TimeBetweenActivations(ringActivationTime));
    }

    // Waits for a time, before activating the next ring
    private IEnumerator TimeBetweenActivations(float timeBetweenActivations) {
        yield return new WaitForSeconds(timeBetweenActivations);
        ActivateRing();
    }

    // Activates the animation of the current ring
    private void ActivateRing() {
        if (currentRing < rings.Length - ringsLengthDecreaser) {
            rings[currentRing].GetComponent<Animator>().SetTrigger("Activate");
            currentRing++;
            StartCoroutine(TimeBetweenActivations(ringActivationTime));
        }        
    }    

    // To check if the player falls off the arena
    public void OnTriggerEnter(Collider col) {
        if (col.transform.CompareTag("Player")) {
            col.GetComponent<Code_Player>().Die();
            gameMng.GetComponent<Code_GameManager>().CheckForVictory(col.transform.gameObject);
        }
    }

    // Activates the bouncer
    public void StartBouncer() {
        // First has a check to determine if the bouncer should be activated this round
        int random = Random.Range(0, 2);
        if (random > 0) {
            bouncer.StartMoveBouncer();
        }
        else {
            ringsLengthDecreaser += ringsToBeSaved;
        }
    }
}
