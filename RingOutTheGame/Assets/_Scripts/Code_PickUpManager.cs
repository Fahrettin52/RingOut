using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_PickUpManager : MonoBehaviour {
    public float dropTimer;
    public List<GameObject> pickUps = new List<GameObject>();
    public Transform[] dropPoints;

    private void Start() {
        foreach (GameObject p in pickUps) {
            p.GetComponent<Code_PickUp>().pickUpMng = this;
        }

        StartCoroutine(DropPickUpCoroutine());
    }

    // Countsdown till next drop of pickup
    private IEnumerator DropPickUpCoroutine() {
        yield return new WaitForSeconds(dropTimer);
        SelectPickUp();
    }

    // Instantiate a pickup form the array in a random manner
    private void SelectPickUp() {
        int random = Random.Range(0, pickUps.Count);
        DropPickUp(pickUps[random]);
    }

    // Places and activates a pickUp
    private void DropPickUp(GameObject pickUp) {
        pickUps.Remove(pickUp);
        int random = Random.Range(0, dropPoints.Length);
        pickUp.transform.position = dropPoints[random].position;
        pickUp.SetActive(true);
        pickUp.GetComponent<Code_PickUp>().StartRepoolCountdown();
        StartCoroutine(DropPickUpCoroutine());
    }    
}
