using UnityEngine;
using System.Collections;

public class Code_Player : MonoBehaviour {

    public int movementSpeed;
    public int rotationSpeed;
    [Header("Stamina Related")]
    public int stamina;
    public int attackCost; // Determines how much stamina is consumed after attacking
    private int startStamina;
    public int staminaRegenAmount; // With how much the stamina regenerates per rate
    public float staminaRegenRate; // The rate per tick that stamina regenerates

    [Header("Knockback")]
    public int knockbackSpeed; // How fast (by proxy how far) the PC will move when knockedback
    private int startKnockbackSpeed;

    public float knockbackTime; // How long the knockback effect lasts
    public float smoothingRate; // Determines how smoothly the knockbackSpeed decreases in value. Advised to be 10f
    private float knockbackDecreaser;
    private float staminaRegen;

    [Header("Based on players remaining stamina")]
    public int[] knockbackDangerLevels; // The dangerlevels of having low stamina. A lower amount means a higher knockbackMultiplier
    [Header("Based on the array above (Has to be same size)")]
    public float[] knockbackMultiplierList; // Connected to knockbackDangerLevels. Determine when to use which multiplier

    private Vector3 knockbackDir; // Direction the knockback will move in

    private bool mayMove; // Determines whether or not the player has control over the PCs movement. True = yes, False = no

    // Use this for initialization
    private void Start() {
        SetStartVariables();        
        ToggleMayMove();
    }

    // Update is called once per frame
    void Update() {
        if (mayMove) {
            Movement();
        }
        else {
            Knockback();
        }

        if (Input.GetButtonDown("Jump")) {
            Attack();
        }

        if (stamina < startStamina) {
            StaminaRegen();
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

    // Called from the Attack/Action buttons
    public void Attack() {
        // Making sure that stamina never gets below 0
        if (stamina - attackCost >= 0) {
            // TODO activate the shield here

            stamina -= attackCost;
        }
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
        transform.Translate(knockbackDir * knockbackSpeed * KnockbackMultiplier() * Time.deltaTime, Space.World);
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
        knockbackSpeed = startKnockbackSpeed;
        ToggleMayMove();
    }

    // Increases the knockbackSpeed depending on how much stamina the PC has left.
    private float KnockbackMultiplier() {
        for (int i = 0; i < knockbackDangerLevels.Length; i++) {
            // Checks if the current stamina is lower than the current danger level
            if (stamina < knockbackDangerLevels[i]) {
                return knockbackMultiplierList[i];
            }
        }
        return 1f;
    }

    // Toggles the mayMove variable
    private void ToggleMayMove() {
        mayMove = !mayMove;
    }

    // Regenerates the PCs stamina when it's necesary
    private void StaminaRegen() {
        if (Time.time > staminaRegen) {
            stamina += staminaRegenAmount;
            staminaRegen = Time.time + staminaRegenRate;
        }
    }

    // Sets any variable that needs to be set during Start()
    private void SetStartVariables (){
        startStamina = stamina;
        startKnockbackSpeed = knockbackSpeed;
    }

    // Is the only Function that call StartKnockback() and should be removed/changed once the shields are being implemented
    public void OnCollisionEnter(Collision col) {
        if (col.transform.name == "Bouncer") {
            StartKnockback(col.transform.position);
        }
    }
}
