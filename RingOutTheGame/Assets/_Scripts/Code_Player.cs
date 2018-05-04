using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Code_Player : MonoBehaviour {

    public bool keyboardControlled; // Test variable to make sure only one is controlled by player.

    public enum MoveState {
        Death,
        Normal,
        Knockedback,
        Attacking,
        Victory,
        Paused
    }
    public MoveState moveState;
    private MoveState previousMoveState;

    public GameObject shield; // The PCs connectec shield
    private Code_Shield shieldCode; // the Code_Shield component on the shield child
    public int movementSpeed;
    public int maxSpeed; // Determines the maximum amount the player is allowed to move
    public int rotationSpeed;
    public int playerNumber;
    private string playerNumberString;
    public float groundRayCastLength;
    [Header("Stamina Related")]
    public Image staminaBar; // Stamina bar image of the player
    public float stamina;
    public int attackCost; // Determines how much stamina is consumed after attacking
    private float startStamina;
    public float staminaRegenAmount; // With how much the stamina regenerates per rate
    public float staminaRegenRate; // The rate per tick that stamina regenerates
    public Sprite teleportNotUsed; // This holds the image that shows the plater that the teleport is not used
    public Sprite teleportUsed; // This holds the image that shows the player that the teleport is used
    public Image teleportImage; // The teleport image 

    [Header("Knockback")]
    public int knockbackSpeed; // How fast (by proxy how far) the PC will move when knockedback
    private int startKnockbackSpeed;

    public float knockbackTime; // How long the knockback effect lasts
    private float staminaRegen;

    [Header("Teleport related")]
    public bool didTP; //Checks if player teleported
    public Transform[] tpList; // Array of tps, can be changed to list if so required

    [Header("Raycasts")]
    public LayerMask groundLayer;
    public Transform[] rcPos; // Positions of the raycasts

    [Header("Based on players remaining stamina")]
    public int[] knockbackDangerLevels; // The dangerlevels of having low stamina. A lower amount means a higher knockbackMultiplier
    [Header("Based on the array above (Has to be same size)")]
    public float[] knockbackMultiplierList; // Connected to knockbackDangerLevels. Determine when to use which multiplier

    private Rigidbody rigidbod;
    private Vector3 knockbackDir; // Direction the knockback will move in

    private Animator playerAnim; // From the player child object

    [Header("PickUp Related")]
    public float deathShieldTimer;
    public ParticleSystem deathShieldPS;

    public delegate bool DeathDel();
    public DeathDel death;
    private Coroutine deathShieldCoroutine;

    // Use this for initialization
    private void Start() {
        SetStartVariables();
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale != 0) {
            CheckMoveState();
            CheckStamina();
        }
    }

    // Checks the movestate of the player
    private void CheckMoveState() {
        switch (moveState) {
            case MoveState.Normal:
                if (!keyboardControlled) {
                    Attack();
                }
                else {
                    AttackKeyboard(); // TODO remove this after testing phase
                }
                break;
            case MoveState.Knockedback:
                // Find something non-Physics related todo for the code (?)
                break;
            case MoveState.Attacking:
                // TODO determine what the player can do whilst he's attacking or whether he shouldn't be able to do anything else
                break;
            case MoveState.Death:
                // TODO Find something meaningful?
                break;
            case MoveState.Victory:
                // TODO play victory animation here, or something like that
                break;
        }
    }

    // Checks the players stamina incase it has to regenerate
    private void CheckStamina() {
        if (stamina < startStamina) {
            StaminaRegen();
        }
    }

    // TODO Split the current player movement into two parts, player Input in the Üpdate and Physics calculations in the FixedUpdate
    void FixedUpdate() {        
        switch (moveState) {
            case MoveState.Normal:
                CheckTeleport();
                if (GroundChecks()) {
                    if (!keyboardControlled) {
                        Movement();
                    }
                    else {
                        MovementKeyboard(); // TODO remove this after testing phase
                    }
                }
                break;
            case MoveState.Knockedback:
                Knockback();
                break;
        }
    }

    // Handles the Teleportation
    public void CheckTeleport() {
        if (!didTP && Input.GetButton("YButton" + playerNumberString)) {
            didTP = true;
            Teleport();
            teleportImage.sprite = teleportUsed;
        }
    }

    // Performs a teleportation
    private void Teleport() {
        int random = Random.Range(0, 4);
        transform.position = tpList[random].position;
    }

    // Checks if the player is touching ground
    private bool GroundChecks() {
        Vector3 downward = transform.TransformDirection(Vector3.down);
        bool isGrounded = false;
        for (int i = 0;  i < rcPos.Length; i++) {
            isGrounded = Physics.Raycast(rcPos[i].position, downward, 10f, groundLayer);
            if (isGrounded) {
                break;             
            }
        }
        return isGrounded;
    }

    //Replace this with the content in KeyboardMovement()
    private void Movement() {
        // Save the inputted axis in a Vector3
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal" + playerNumberString) * movementSpeed,
            0,
            Input.GetAxis("Vertical" + playerNumberString) * movementSpeed
        );

        // Rotates the player according to the inputted axis
        if (!Input.GetButton("Strafe" + playerNumberString)) {
            if (move.x != 0 || move.z != 0) {
                rigidbod.rotation = Quaternion.LookRotation(move * Time.deltaTime);
            }
        }

        // Moves the player according to the inputted axis
        rigidbod.AddForce(move * movementSpeed);
        rigidbod.velocity = Vector3.ClampMagnitude(rigidbod.velocity, maxSpeed);
    }

    // Called from the Attack/Action buttons
    public void Attack() {
        if (Input.GetButtonDown("AButton" + playerNumberString)) {
            // Making sure that stamina never gets below 0
            if (stamina - attackCost >= 0) {                
                SwitchMoveState(MoveState.Attacking);
                shieldCode.Attack();
                stamina -= attackCost;
                UpdateStaminaBar();
            }
        }
    }

    //// TODO remove this function after testing
    private void MovementKeyboard() {
        // Save the inputted axis in a Vector3
        Vector3 move = new Vector3(
            Input.GetAxis("HorizontalKeyboard" + playerNumber) * movementSpeed,
            0,
            Input.GetAxis("VerticalKeyboard" + playerNumber) * movementSpeed
        );

        // Rotates the player according to the inputted axis
        if (move != Vector3.zero) {
            rigidbod.rotation = Quaternion.LookRotation(move * Time.deltaTime);
        }

        // Moves the player according to the inputted axis
        rigidbod.AddForce(move * movementSpeed);
        rigidbod.velocity = Vector3.ClampMagnitude(rigidbod.velocity, maxSpeed);
    }

    // TODO remove this function after testing
    public void AttackKeyboard() {
        if (Input.GetButtonDown("Jump" + playerNumber)) {
            // Making sure that stamina never gets below 0
            if (stamina - attackCost >= 0) {
                SwitchMoveState(MoveState.Attacking);
                shieldCode.Attack();
                stamina -= attackCost;
            }
        }
    }

    //// Starts the knockback sequence
    public void StartKnockback(Vector3 hitPosition) {
        // Calculate the knockBackDir
        knockbackDir = hitPosition - transform.position;
        knockbackDir.y = 0;
        knockbackDir.Normalize();
        
        // Disallow movement
        SwitchMoveState(MoveState.Knockedback);

        // Play Knockedback animation
        playerAnim.SetTrigger("Knockedback");

        // Start countdown Coroutine
        StartCoroutine(KnockbackCountdown());
    }

    // Is called when mayMove is false and translates the PC into it's appropraite direction. Until mayMove is true again
    private void Knockback() {
        // Moves the player as if he got knocked backwards
        rigidbod.AddForce(-knockbackDir * movementSpeed * knockbackSpeed * KnockbackMultiplier());
        rigidbod.velocity = Vector3.ClampMagnitude(rigidbod.velocity, maxSpeed);
    }

    // Countdown for knockback. When it's done it resets the appropriate variables and call ToggleMayMove()
    private IEnumerator KnockbackCountdown() { 
        yield return new WaitForSeconds(knockbackTime);
        knockbackSpeed = startKnockbackSpeed;
        playerAnim.SetTrigger("KnockbackHalted");
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

    // Called from the GameMng GameObject in the scene. Also is called when an animation sequence (like being knocked back or attacking) has ended.
    // Allows the players to move after the countdown
    public void NormalizeMoveState() {
        SwitchMoveState(MoveState.Normal);
    }

    public void ResetMoveState() {
        SwitchMoveState(previousMoveState);
    }

    // Makes sure that the players can't move
    public void PauseMoveState() {
        previousMoveState = moveState;
        SwitchMoveState(MoveState.Paused);
    }

    // Is called from the Code_GameManager in the scene whenever this object is declared the victor
    public void VictoryDance() {
        SwitchMoveState(MoveState.Victory);
    }

    // Regenerates the PCs stamina when it's necesary
    private void StaminaRegen() {
        if (Time.time > staminaRegen) {
            stamina += staminaRegenAmount;
            staminaRegen = Time.time + staminaRegenRate;
            UpdateStaminaBar();
        }
    }

    // Fully regenerates the stamina
    public void ResetStamina() {
        stamina = startStamina;
        UpdateStaminaBar();
    }

    // Updates the staminabar UI
    public void UpdateStaminaBar() {
        staminaBar.fillAmount = stamina / 100;
    }

    // Sets any variable that needs to be set during Start()
    private void SetStartVariables (){
        ResetDelegates();
        playerNumberString = playerNumber.ToString();
        startStamina = stamina;
        startKnockbackSpeed = knockbackSpeed;
        playerAnim = GetComponentInChildren<Animator>();
        shieldCode = shield.GetComponent<Code_Shield>();
        rigidbod = GetComponent<Rigidbody>();
    }

    // When the player falls off the arena
    private bool Die() { 
        // TODO fill this function with more functionality regarding dieing
        SwitchMoveState(MoveState.Death);
        rigidbod.velocity = new Vector3(0f, rigidbod.velocity.y, 0f);
        // Returns true because the player "died"
        return true;
    }

    // Sets the death delegate to Die()
    private void SetDie() {
        death = Die;
        if (deathShieldPS.isEmitting) {
            deathShieldPS.Stop();
        }
    }   

    // A shield that protects the player
    private bool DeathShield() {
        SetDie();
        Teleport();
        // Returns false because the player didn't "die"
        return false;
    }

    // Turns the death into DeathShield
    public void SetDeathShield() {
        if (death != DeathShield) {
            death = DeathShield;
            deathShieldPS.Play();
            deathShieldCoroutine = StartCoroutine(DeathShieldCountdown());            
        }
    }

    // Sends back whether the players death is DeathShield or not
    public bool GetDeathShield() {        
        return death == DeathShield;
    }

    // Counts down and at the end of it turns of the deathshield
    private IEnumerator DeathShieldCountdown() {
        yield return new WaitForSeconds(deathShieldTimer);
        if (death == DeathShield) {
            SetDie();
        }        
    }

    // Resets the players when the players choose to continue the match
    public void ResetPlayer() {
        didTP = false;
        ResetDelegates();
        ResetStamina();
        teleportImage.sprite = teleportNotUsed;
    }

    // Resets all the delegates upon resetting the game
    private void ResetDelegates() {
        if (death != Die) {
            SetDie();
            if (deathShieldCoroutine != null) {
                StopCoroutine(deathShieldCoroutine);
            }            
        }
    }

    // Is the only Function that call StartKnockback() and should be removed/changed once the shields are being implemented
    public void OnCollisionEnter(Collision col) {
        if (col.transform.name == "Bouncer") {
            StartKnockback(col.transform.position);
        }
    }
}
