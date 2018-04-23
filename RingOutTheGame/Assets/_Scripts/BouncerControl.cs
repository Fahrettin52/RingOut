using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerControl : MonoBehaviour {

    public Transform[] teleportSpot;
    public float teleportTimer;
    private Vector3 startPos;
    private Coroutine teleportRoutine;

    public void Teleport() {
        transform.position = teleportSpot[Random.Range(0, teleportSpot.Length)].position;
        teleportRoutine = StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown() {
        yield return new WaitForSeconds(teleportTimer);
        Teleport();
    }

    public void StopTeleport() {
        if (teleportRoutine != null) {
            StopCoroutine(teleportRoutine);
        }
    }
}
