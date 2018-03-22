using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_PlayerAnimations : MonoBehaviour {
    private Code_Player playerParent;

	// Use this for initialization
	void Start () {
        playerParent = transform.parent.GetComponent<Code_Player>();
    }

    // Called from any player gameobject specific code
    public void CallAnimationEnded() {
        playerParent.NormalizeMoveState();
    }
}
