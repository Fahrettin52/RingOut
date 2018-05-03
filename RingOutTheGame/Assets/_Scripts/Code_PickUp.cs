using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_PickUp : MonoBehaviour {
    public Code_PickUpManager pickUpMng;
    public float repoolTimer;

    private Coroutine repoolCoroutine;

    // Calls the Repooling coroutine
    public void StartRepoolCountdown() {
        repoolCoroutine = StartCoroutine(Repooling());
    }

    // Timer for repooling itself, so it doesn't stay on the field forever
    private IEnumerator Repooling() {
        yield return new WaitForSeconds(repoolTimer);
        PoolPickUp();
    }

    // When it's hit by a player
    public void PickedUp(Code_Player player) {
        if (repoolCoroutine != null) {
            StopCoroutine(repoolCoroutine);
        }
        ActivatePickUpEffect(player);
        PoolPickUp();
    }

    // Pools the pickup and stores it once more
    private void PoolPickUp() {
        pickUpMng.pickUps.Add(gameObject);
        gameObject.SetActive(false);
    }

    // Overidable function for it's children
    public virtual void ActivatePickUpEffect(Code_Player player) {
        print("Activated by player"); // For when a dev made a child but did not override this function
    }
}
