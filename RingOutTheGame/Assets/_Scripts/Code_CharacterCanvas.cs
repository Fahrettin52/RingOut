using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_CharacterCanvas : MonoBehaviour {

    public GameObject sceneCamera;
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(sceneCamera.transform);
	}
}
