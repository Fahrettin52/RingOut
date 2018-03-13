using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Shield : MonoBehaviour {

    private Animator anim;
    private BoxCollider myCol;
    private Code_Player playerCode; // The Code_Player component on the parent of this shield
    private GameObject parent;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        myCol = GetComponent<BoxCollider>();
        playerCode = GetComponentInParent<Code_Player>();
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {

    }

    // When the player attacks
    public void Attack() {
        // Plays the "Bash" animation
        anim.SetTrigger("Bash");
    }

    // Toggles the Box Collider on or off
    public void ToggleShield(){
        myCol.enabled = !myCol.enabled;
    }

    // Signals it's parent that the attack animation has stopped
    public void AttackEnds() {
        playerCode.AttackEnded();
    }

    // When the shield touches a player it "knocks back the player"
    public void OnTriggerEnter(Collider col) {
        Transform colTrans = col.transform;
        if (col.gameObject != parent) {
            if (colTrans.tag == "Player") {
                colTrans.GetComponent<Code_Player>().StartKnockback(transform.position);
            }
            else if (colTrans.tag == "Shield") {
                colTrans.parent.GetComponent<Code_Player>().StartKnockback(transform.position);
            }
        }
    }
}
