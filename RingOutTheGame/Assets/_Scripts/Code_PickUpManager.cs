using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_PickUpManager : MonoBehaviour {
    public Code_GameManager gameMng; // The gameMng in the scene
    public float dropTimer; // Timer for between drops
    public Code_PickUp[] pickUps; // Array of pickUp objects
    private int pooledCount; // Keeps track of how many items in the pickUps Array are pooled  
    public Code_Arena arena; // The arena in the scene and given by the gameMng  
    public int[] dropRadius; // The various maximum distance the pickups can drop in

    private void Start() {
        foreach (Code_PickUp p in pickUps) {
            p.pickUpMng = this;
        }
        pooledCount = pickUps.Length;
        arena = gameMng.arena;
        StartDropProcess();
    }

    // The beginning of the "spawning" of the pickups
    private void StartDropProcess() {
        StartCoroutine(DropPickUpCoroutine());
    }

    // Countsdown till next drop of pickup
    private IEnumerator DropPickUpCoroutine() {
        yield return new WaitForSeconds(dropTimer);
        if (pooledCount > 0) {
            SelectPickUp();
        }
        StartDropProcess();
    }

    // Selects a pickup from the array in a random manner
    private void SelectPickUp() {
        List<Code_PickUp> pList = new List<Code_PickUp>();
        foreach (Code_PickUp p in pickUps) {
            if (!p.isUnpooled) {
                pList.Add(p);
            }
        }

        // Decrement the pooledCount
        ChangePooledCount(-1); 
        // Drop the selected pickUp
        DropPickUp(pList[Random.Range(0, pList.Count)]);
    }

    // Centralization of the decrementation and incrementation of the pooledCount value
    public void ChangePooledCount(int pooledValue) {
        pooledCount += pooledValue;
    }

    // Unpools the pickUp
    private void DropPickUp(Code_PickUp pickUp) {
        // Get arena's currentRingValue
        int ringV = arena.GetCurrentRingValue();

        // Check if ringV isn't bigger than the last item in dropRadius
        if (ringV > dropRadius.Length-1) {
            ringV = dropRadius.Length-1;
        }

        // Get a Vector2 based on the curDR
        Vector2 dropSpot = Random.insideUnitCircle * dropRadius[ringV];

        // Give Unpool the dropSpots
        pickUp.Unpool(new Vector3(dropSpot.x, transform.position.y, dropSpot.y)); 
    }

    // Resets the pickUps Array when the game/match resets
    public void ResetPickUps() {
        // Repool any unpooled members of the pickUps array
        foreach (Code_PickUp p in pickUps) {
            if (p.isUnpooled) {
                p.PoolPickUp();
            }
        }

        // Assign a new arena based on the gameMngers
        arena = gameMng.arena;
    }
}
