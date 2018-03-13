using UnityEngine;
using System.Collections;

public class Code_Player : MonoBehaviour {

    public int movementSpeed;
    public int rotationSpeed;
    public int stamina;
    [Header("Knockback")]
    public int knockbackSpeed;    
    
    public float knockbackTime;
    public float smoothingRate;
    private float knockbackDecreaser;

    private Vector3 knockbackDir;

    private bool mayMove;

    // Use this for initialization
    private void Start() {
        ToggleMayMove();
    }

    // Update is called once per frame
    void Update () {
        if (mayMove) {
            Movement();
        }
        else {
            Knockback();
        }
	}

    /// <summary>
    /// Handles the movement and rotation of the player
    /// </summary>
    private void Movement() {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (move != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(move * Time.deltaTime);
        }

        transform.Translate(move * movementSpeed * Time.deltaTime, Space.World);
    }

    // Starts the knockback sequence
    public void StartKnockback(Vector3 hitPosition) {
        // Calculate the knockBackDir
        knockbackDir = hitPosition - transform.position;
        knockbackDir = -knockbackDir.normalized;
        // Controls the players "hop" in the air
        knockbackDir.y = 0f;

        // Disallow movement
        ToggleMayMove();
        StartCoroutine(KnockbackCountdown());
    }    

    // Is called when mayMove is false and translates the PC into it's appropraite direction. Until mayMove is true again
    private void Knockback() {
        transform.Translate(knockbackDir * knockbackSpeed * knockbackMultiplier() * Time.deltaTime, Space.World);
        KnockbackSmoother();
    }

    // Smooths out the knockback for a bit
    private void KnockbackSmoother() {
        if (Time.time > knockbackDecreaser) {
            knockbackSpeed--;
            knockbackDecreaser = Time.time + (knockbackTime / smoothingRate);
        }
    }   

    // Countdown for knockback. When it's done it resets the appropriate variables and call ToggleMayMove()
    private IEnumerator KnockbackCountdown() {
        yield return new WaitForSeconds(knockbackTime);
        knockbackSpeed = 10;
        ToggleMayMove();
    }

    // Increases the knockbackSpeed depending on how much stamina the PC has left.
    private float knockbackMultiplier() {
        if (stamina > 75) {
            return 1f;
        }
        else if (stamina > 50) {
            return 1.5f;
        }
        else if (stamina > 25) {
            return 2f;
        }
        else if (stamina > 10) {
            return 3f;
        }
        else {
            return 4f;
        }
    }

    // Toggles the mayMove variable
    private void ToggleMayMove() {
        mayMove = !mayMove;
    }

    // Is the only Function that call StartKnockback() and should be removed/changed once the shields are being implemented
    public void OnCollisionEnter(Collision col) {
        if (col.transform.name == "Bouncer") {
            StartKnockback(col.transform.position);
        }
    }
}
