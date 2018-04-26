using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Shield : MonoBehaviour {

    private Animator anim;
    private BoxCollider myCol;
    private Code_Player playerCode; // The Code_Player component on the parent of this shield
    public GameObject parentPlayer; // Has to be the parent with the Code_Player component
    private string playerTag = "Player"; // A tag that we are looking for when hitting an object, just meant for microoptimisation
    private string shieldTag = "Shield"; // A tag we are looking for when hitting an object, just meant for microoptimisation

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        myCol = GetComponent<BoxCollider>();
        playerCode = parentPlayer.GetComponent<Code_Player>();
    }

    // When the player attacks
    public void Attack() {
        // TODO change the Bash animation with the appropriate attack animation
        // Plays the "Bash" animation
        anim.SetTrigger("Bash");
    }

    // Toggles the Box Collider on or off
    public void ToggleShield(){
        myCol.enabled = !myCol.enabled;
    }

    // Signals its parent that the attack animation has stopped
    public void AttackEnds() {
        playerCode.NormalizeMoveState();
    }

    // When the shield touches a player it "knocks back the player"
    public void OnTriggerEnter(Collider col) {
        Transform colTrans = col.transform;
        if (col.gameObject != parentPlayer) {
            if (colTrans.CompareTag(playerTag) || colTrans.CompareTag(shieldTag)) {
                if (colTrans.GetComponent<Code_Player>() != null) {
                    colTrans.GetComponent<Code_Player>().StartKnockback(transform.position);
                }
            }
        }
    }
}
