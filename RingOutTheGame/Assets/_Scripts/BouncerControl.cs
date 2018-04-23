using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerControl : MonoBehaviour {

    public Transform[] moveSpots;
    private int curMoveSpot = -1; // This tracks the curMoveSpot, The -1 makes sure that the first time is selected prefectly random
    private int lastMoveSpot; // The last index of moveSpots
    public float moveCooldownTimer; // A timer that determines when the bouncer will start moving again
    private Vector3 startPos; // The starting position of the bouncer, which will be used to reset it after every victory
    private Coroutine coroutineToStop; // The current Coroutine thatkeeps track of which Coroutine has to be stopped when the game is won
    public float speed; // How fast the bouncer moves
    public float minDist; // The minimal desired distance between the bouncer and its next target spot

    private void Start() {
        startPos = transform.position;
        lastMoveSpot = moveSpots.Length - 1;
    }

    // Starts the entire movement process of the bouncer
    public void StartMoveBouncer() {
        transform.position = startPos;
        coroutineToStop = StartCoroutine(MoveBouncer());
    }

    // Returns a index for the moveSpots array
    private int SelectSpotPos() {
        // Checks first if this is the first time this round that SelectSpotPos is being called
        if (curMoveSpot < 0) {
            return curMoveSpot = Random.Range(0, moveSpots.Length);
        }

        // Set the nextSpot variable for later use
        int nextSpot = Random.Range(0, 2);
        if (nextSpot < 1) {
            nextSpot = -1;
            // Check if the curMoveSpot is 0. If it is 0 then give it special treatment
            if (curMoveSpot == 0) {
                return curMoveSpot = lastMoveSpot;
            }
        }
        else {
            // Check if the curMoveSpot is the last item in the moveSpots array. If it is then give it special treatment
            if (curMoveSpot == lastMoveSpot) {
                return curMoveSpot = 0;
            }
        }

        // Add the nextSpot to the curMoveSpot
        return curMoveSpot = curMoveSpot + nextSpot;
    }

    // Moves the bouncer to a selected spot
    private IEnumerator MoveBouncer() {
        // Sets a couple of variables necessary for the next while loop
        Vector3 spotPos = moveSpots[SelectSpotPos()].position;
        float spotDist = Vector3.Distance(transform.position, spotPos);

        // This keeps the bouncer moving until it's reached the desired spot
        while (spotDist > minDist) {
            transform.position = Vector3.MoveTowards(transform.position, spotPos, speed * Time.deltaTime);
            spotDist = Vector3.Distance(transform.position, spotPos);
            yield return null;
        }

        // Call MoveCooldown IEnumerator and set it as the new coroutineToStop
        coroutineToStop = StartCoroutine(MoveCooldown());
    }

    // A Coroutine that merely acts as a timer so NextMovementSpot can start over again
    private IEnumerator MoveCooldown() {
        yield return new WaitForSeconds(moveCooldownTimer);
        coroutineToStop = StartCoroutine(MoveBouncer());
    }

    // Stops the currently active Coroutine: coroutineToStop
    public void StopCurrentCoroutine() {
        if (coroutineToStop != null) {
            StopCoroutine(coroutineToStop);
        }
    }

    // is called by the GameManager whenever there's been a Victory
    public void ResetPosition() {
        transform.position = startPos;
    }
}
