using UnityEngine;
using System.Collections;

public class Code_Player : MonoBehaviour {

    public bool notControlled; // Test variable to make sure only one is controlled by player.

    private enum MoveState {
        Normal,
        Knockedback,
        Attacking
    }
    private MoveState moveState;

    public GameObject shield; // The PCs connectec shield
    private Code_Shield shieldCode; // the Code_Shield component on the shield child
    public int movementSpeed;
    public int rotationSpeed;
    public int playerNumber;
    private string playerNumberString;
    public float groundRayCastLength;
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

    private Rigidbody rigidbod;
    private Vector3 knockbackDir; // Direction the knockback will move in

    private Animator playerAnim; // From the player child object

    // Use this for initialization
    private void Start() {
        SetStartVariables();      
    }

    // Update is called once per frame
    void Update() {
        if (!TouchingGround()) {
            return;
        }

        switch (moveState) {
            case MoveState.Normal:
                if (notControlled) { 
                    Movement();
                    Attack();
                }
                break;
            case MoveState.Knockedback:
                Knockback();
                break;
            case MoveState.Attacking:
                // TODO determine what the player can do whilst he's attacking or whether he shouldn't be able to do anything else
                break;            
        }     

        if (stamina < startStamina) {
            StaminaRegen();
        }
    }

    // Uses a downward Raycast to check if there's still ground underneath the player
    private bool TouchingGround() {
        // Replace the Raycast with a Physics.CheckCapsule if the Arena will be filled with uneven ground or have many "tiny" holes in them
        return Physics.Raycast(transform.position, -transform.up, groundRayCastLength);
    }

    /// <summary>
    /// Handles the movement and rotation of the player
    /// </summary>
    private void Movement() {        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal" + playerNumberString), 0f, Input.GetAxis("Vertical" + playerNumberString));
        Vector3 moveTest = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); // TODO Remove afterwards
        if (move != Vector3.zero || moveTest != Vector3.zero) { // TODO Remove latter part afterwards
            transform.rotation = Quaternion.LookRotation(move * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(moveTest * Time.deltaTime);
        }

        rigidbod.velocity = move * movementSpeed;
        rigidbod.velocity = moveTest * movementSpeed; // TODO Remove afterwards
    }

    // Called from the Attack/Action buttons
    public void Attack() {
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("AButton" + playerNumberString)) {
            // Making sure that stamina never gets below 0
            if (stamina - attackCost >= 0) {
                SwitchMoveState(MoveState.Attacking);
                shieldCode.Attack();
                stamina -= attackCost;
            }
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
        SwitchMoveState(MoveState.Knockedback);
        // Play Knockedback animation
        playerAnim.SetTrigger("Knockedback");
        StartCoroutine(KnockbackCountdown());
    }

    // Is called when mayMove is false and translates the PC into it's appropraite direction. Until mayMove is true again
    private void Knockback() {
        // If there's a updraft when coming into contact with other players, swap transform.Translate with rigidbod.velocity
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
        playerAnim.SetTrigger("KnockbackHalted");
        //SwitchMoveState(MoveState.Normal); // TODO replace this through an event key in the KnockbackHalt animation
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

    // Centralisation of the changing of this PCs moveState
    private void SwitchMoveState(MoveState newMoveState) {
        moveState = newMoveState;
    }

    // When an animation sequence (like being knocked back or attacking) has ended and the moveState requires to be Normal again
    public void AnimationEnded() {
        SwitchMoveState(MoveState.Normal);
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
        playerNumberString = playerNumber.ToString();
        startStamina = stamina;
        startKnockbackSpeed = knockbackSpeed;
        playerAnim = GetComponentInChildren<Animator>();
        shieldCode = shield.GetComponent<Code_Shield>();
        rigidbod = GetComponent<Rigidbody>();
    }

    // Is the only Function that call StartKnockback() and should be removed/changed once the shields are being implemented
    public void OnCollisionEnter(Collision col) {
        if (col.transform.name == "Bouncer") {
            StartKnockback(col.transform.position);
        }
    }
}
