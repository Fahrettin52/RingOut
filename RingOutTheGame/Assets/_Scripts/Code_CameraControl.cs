using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_CameraControl : MonoBehaviour {

    public Code_CameraFocus focus;

    public List<GameObject> players;

    public float cameraSpeed;

    public float zoomMax;
    public float zoomMin;

    public float angleMax;
    public float angleMin;

    private float cameraEulerX;
    private Vector3 camPos;

    // Use this for initialization
    void Start () {
        players.Add(focus.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        CalculateCameraLocations();
        MoveCamera();

    }

    private void CalculateCameraLocations() {
        Vector3 center = Vector3.zero;
        Vector3 total = Vector3.zero;
        Bounds playerBounds = new Bounds();

        foreach (GameObject p in players) {
            Vector3 pPos = p.transform.position;

            if (!focus.focusBounds.Contains(pPos)) {
                float pX = Mathf.Clamp(pPos.x, focus.focusBounds.min.x, focus.focusBounds.max.x);
                float pY = Mathf.Clamp(pPos.y, focus.focusBounds.min.y, focus.focusBounds.max.y);
                float pZ = Mathf.Clamp(pPos.z, focus.focusBounds.min.z, focus.focusBounds.max.z);
                pPos = new Vector3(pX, pY, pZ);
            }

            total += pPos;
            playerBounds.Encapsulate(pPos);
        }

        center = (total / players.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (focus.halfXBounds + focus.halfYBounds) / 2, extents);

        float zoom = Mathf.Lerp(zoomMax, zoomMin, lerpPercent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);

        cameraEulerX = angle;
        camPos = new Vector3(center.x, zoom, zoom*-1);
    }

    private void MoveCamera() {
        Vector3 position = transform.position;
        if (position != camPos) {
            Vector3 targetPos = Vector3.zero;
            targetPos.x = Mathf.MoveTowards(position.x, camPos.x, cameraSpeed * Time.deltaTime);
            targetPos.y = Mathf.MoveTowards(position.y, camPos.y, cameraSpeed * Time.deltaTime);
            targetPos.z = Mathf.MoveTowards(position.z, camPos.z, cameraSpeed * Time.deltaTime);
            transform.position = targetPos;
        }

        Vector3 localEuler = transform.localEulerAngles;
        if (localEuler.x != cameraEulerX) {
            Vector3 targetEuler = new Vector3(cameraEulerX, localEuler.y, localEuler.z);
            transform.localEulerAngles = Vector3.MoveTowards(localEuler, targetEuler, cameraSpeed * Time.deltaTime);
        }
    }    
}
