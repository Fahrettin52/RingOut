using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_CameraControl : MonoBehaviour
{

    public Code_CameraFocus focus; // A point of focus that helps to balance the center

    public List<GameObject> players; // A list of players filled in the editor

    public float speed; // Determines how fast the camera moves around

    public float zoomMax; // Determines how far out the camera is allowed to zoum
    public float zoomMin; // Determines how far in the camera is allowed to zoom

    [Header("Bonuses for the camera")]
    public float[] zoomBonus; // Adds an extra amount of range allowing for outside controll of the zomming
    private int curBonus; // The current bonus in both the yBonus and zoomBonus arrays
    public float[] yBonus; // Add an extra amount of range allowing for outside manipulation
    private float yStartPos; // Gets the transform.position at the start of the game

    [Header("ZoomBonus values based on the number of players selected")]
    public float[] twoPlayersBonus; // all but the last indexes of the zoomBonus and yBonus, when two players were selected
    public float[] threePlayersBonus; // all but the last indexes of the zoomBonus and yBonus, when three players were selected
    public float[] fourPlayersBonus; // all but the last indexes of the zoomBonus and yBonus, when four players were selected

    // Need to check and see if we want to move the angles of the camera
    //public float angleMax;
    //public float angleMin;

    //private float cameraEulerX;
    private Vector3 camPos; // The calculated future position of the camera

    // Use this for initialization
    void Start() {
        players.Add(focus.gameObject);
        yStartPos = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        CalculateCameraLocations();
        MoveCamera();
    }

    // Calculates where the camera has to move to
    private void CalculateCameraLocations() {
        Vector3 total = Vector3.zero;
        Bounds playerBounds = new Bounds();

        // Makes adds each members location to the foreach and then adds them to the total 
        foreach (GameObject p in players) {
            Vector3 pPos = p.transform.position;
            // Checks if the player is still on top of the arena
            if (!focus.focusBounds.Contains(pPos)) {
                pPos = Vector3.zero;
            }

            total += pPos;
            playerBounds.Encapsulate(pPos);
        }

        // Create a center point for the camera to focus with
        Vector3 centerPoint = (total / players.Count);

        // Calclates the lerpPrecentage
        float extents = (playerBounds.extents.x + playerBounds.extents.z);
        float lerpPercent = Mathf.InverseLerp(0, (focus.halfXBounds + focus.halfYBounds), extents);

        //float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);
        //cameraEulerX = angle;

        // Zoom determines the maximum and minimum movement range for the camera
        float zoom = Mathf.Lerp(zoomMax, zoomMin, lerpPercent);
        camPos = new Vector3(centerPoint.x, yStartPos - yBonus[curBonus], (zoom - zoomBonus[curBonus]) * -1f);
        print("Curbonuds is " + curBonus);
    }

    // The function that actually moves the camera proper
    private void MoveCamera() {
        // Moves the camera to the camPos if it's not aligned correctly
        Vector3 pos = transform.position;
        if (pos != camPos) {
            transform.position = Vector3.MoveTowards(pos, camPos, speed * Time.deltaTime);
        }

        //moves the angles of the camera
        //vector3 localeuler = transform.localeulerangles;
        //if (localeuler.x != cameraeulerx) {
        //    vector3 targeteuler = new vector3(cameraeulerx, localeuler.y, localeuler.z);
        //    transform.localeulerangles = vector3.movetowards(localeuler, targeteuler, cameraspeed * time.deltatime);
        //}
    }

    // Updates the curBonus which will the determine the value of both the yBonus and zoomBonus
    public void UpdateBonuses(int curRing) {
        curRing++;
        if (curRing < zoomBonus.Length) {
            print("curRing is valued customer");
            curBonus = curRing;
        }
    }

    // Change the values of the zoomBonus items, all but the last, according to the playersSelected
    public void ChangeBonusValues(int numOfPlayers) {
        // First determines which bonus array should be used
        List<float> playerBonusList = new List<float>();
        switch (numOfPlayers) {
            case 2:
                playerBonusList = new List<float>(twoPlayersBonus);
                break;
            case 3:
                playerBonusList = new List<float>(threePlayersBonus);
                break;
            case 4:
                playerBonusList = new List<float>(fourPlayersBonus);
                break;
        }

        // Then changes the values of the zoomBonus array, All but the last item
        for (int i = 0; i < zoomBonus.Length - 1; i++) {
            zoomBonus[i] = playerBonusList[i];
        }
    }
}
