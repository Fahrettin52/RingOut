using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Code_GameManager : MonoBehaviour {
    public GameObject[] players; // Array of players in the game    
    private List<GameObject> activePlayers = new List<GameObject>(); // List of players that are playing the game
    public GameObject[] spawnLocations; // Array of array of spawn locations, based on the chosen amount of players
    private List<Vector3> spawnPosition = new List<Vector3>(); // Private list of Vector3 children objects in the items of spawnLocations
    private int playersSelected; // The amount of players selected to play the game
    private int oldPlayersSelected; // Value that saves the playerSelected, so it can compare it later on    
    private bool firstTimeStarting; // Differentiates the code when it's the first time calling StartGameCountdown Function

    [Header("Arena")]
    public Code_Arena arena; // The arena in the scene
    public GameObject arenaPrefab; // Prefab for the arena
    public string arenaName; // To name the newly instantiated arena
    private GameObject newArena; // To save the instantiated arena

    [Header("Bouncer")]
    public BouncerControl bouncer; // The bouncer object present in the scene

    [Header("Camera")]
    public Code_CameraControl camCon;

    [Header("Ingame Menu")]
    public Code_InGameMenuManager ingameMng; // The Code_IngameManager in the scene

    [Header("PlayerSelect")]
    public GameObject playerSelectMenu; // Menu for the player selection screen
    public GameObject playAgainMenu; // Menu for the replay game

    [Header("Sound")]
    public Code_SoundManager soundMng; // The sound manager

    [Header("Countdown")]
    public float countdownSeconds; // How long the countdown will take
    private float countdownSecondsSaved; // Saves the countdownSeconds so it can be reset
    public GameObject countdownBanner; // Banner for the countdown UI
    public Text countdownText; // The text that'll hold the countdown

    [Header("PickUp Manager")]
    public Code_PickUpManager pickUpMng; // The Code_PickUpManager currently in the scene

    [Header("Victory Banner")]
    public GameObject victoryBanner; // The sprite that appears once there's a winner
    public Text victoryMessageText; // Message holder in the VictoryBanner GameObject
    public string victoryMessage; // Message that precedes the victors name

    void Start() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
        }
    }

    // Centralization of turning pause on and off
    public void TogglePause() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

    // Player Selected a number of participants through the PlayerSelectMenu
    public void SelectedNumberOfPlayers(int numberOfPlayers) {
        playersSelected = numberOfPlayers;
        countdownSecondsSaved = countdownSeconds;

        // Change the first two curBonus indexes based on the playersSelected
        camCon.ChangeBonusValues(playersSelected);

        // Selects which spawnpoint position should be used
        FillSpawnPositions();

        // Determines how many players are being activated
        FillActivePlayerList();

        // Turn off the Main Menu Music sound
        soundMng.musicAudioSource[0].mute = true;

        // Play the Game Music sound
        soundMng.PlayGameMusic();

        // Spawns the players to their respective spawnPoint
        SpawnPlayers();
    }

    // Fills the spawnVector3 list with new values based on the number of players playing
    private void FillSpawnPositions() {
        if (oldPlayersSelected != playersSelected) {
            spawnPosition.Clear();
            foreach (Transform child in spawnLocations[playersSelected-2].transform) {
                spawnPosition.Add(child.position);
            }

            oldPlayersSelected = playersSelected;
        }
    }

    // Fills the private activePlayers List with players that are going to play the game
    public void FillActivePlayerList() {
        activePlayers.Clear();
        for (int i = 0; i < playersSelected; i++) {
            activePlayers.Add(players[i]);
        }
    }

    // Activates the necessary amount of players
    public void SpawnPlayers() {
        // Turn of player select menu
        playerSelectMenu.SetActive(false);

        // Places the players on their respective spawn location
        for (int i = 0; i < activePlayers.Count; i++) {
            activePlayers[i].transform.position = spawnPosition[i];
        }

        // Activates the players so they are visible
        foreach (GameObject player in activePlayers) {
            player.SetActive(true);
        }

        CallStartGameCountdown();
    }

    public void CallStartGameCountdown() {
        // Starts the countdown of the game
        StartCoroutine(StartGameCountdown());
    }

    // Countdown to the players being able to start playing
    private IEnumerator StartGameCountdown() {
        // Sets the adequate variables to desired values;
        countdownBanner.SetActive(true);
        
        // A countdown system using the while loop
        while (countdownSeconds != 0) {
            countdownText.text = countdownSeconds.ToString();
            countdownSeconds--;
            float countdownEnd = Time.realtimeSinceStartup + 1f; // The 1f represents one second, MUST be hardcoded
            while (Time.realtimeSinceStartup < countdownEnd) {
                yield return null;
            }            
        }

        // Resets the necessary variables for later reuse
        countdownBanner.SetActive(false);
        countdownSeconds = countdownSecondsSaved;

        // Toggle pause
        TogglePause();
        

        // Check if this is the first time that this function's called
        if (!firstTimeStarting) {
            // Allows all players to finally start playing
            AllowMovement();

            // Activate the arena crumble
            arena.ActivateFallProcess();
            firstTimeStarting = true;
        }
        else {            
            ResetMovement();
        }

        // Activates the IngameMenuManager
        ingameMng.allowStart = true;
    }

    // Let the players move
    private void AllowMovement() {
        foreach (GameObject player in activePlayers) {
            player.GetComponent<Code_Player>().NormalizeMoveState();
        }
    }

    // Resets the movestate back to its previous state
    private void ResetMovement() {
        foreach (GameObject player in activePlayers) {
            player.GetComponent<Code_Player>().NormalizeMoveState();
        }
    }

    // Players are not allowed to move
    public void DisableMovement() {
        foreach (GameObject player in activePlayers) {
            player.GetComponent<Code_Player>().PauseMoveState();
        }
    }

    // Is called from the Arena GameObject each time a player falls past it's boundary
    public void CheckForVictory(GameObject deadPlayer) {
        activePlayers.Remove(deadPlayer);

        // TODO If there's UI element that represent the active players, Affect that element that belongs to the fallen player
        if (activePlayers.Count == 1) {
            ingameMng.allowStart = false;
            activePlayers[0].GetComponent<Code_Player>().VictoryDance();
            AnnounceWinner(activePlayers[0].GetComponent<Code_Player>());
            bouncer.StopCurrentCoroutine();            
            StartCoroutine(RestartFromVictory());
        }
    }

    // Stops the gameplay and announces the winner through the VictoryBanner GameObject
    private void AnnounceWinner(Code_Player winner) {
        victoryMessageText.text = victoryMessage + GetVictorsColor(winner.playerNumber);
        victoryBanner.SetActive(true);        
    }

    // Gives back a string based on the number of the winning player
    private string GetVictorsColor(int winningNumber) {
        switch (winningNumber) {
            case 1:
                victoryMessageText.color = Color.blue;
                return "blue wins";
            case 2:
                victoryMessageText.color = Color.green;
                return "green wins";
            case 3:
                victoryMessageText.color = Color.red;
                return "red wins";
            case 4:
                victoryMessageText.color = Color.yellow;
                return "yellow wins";
            default:
                return "Winners don't use drugs";
        }
    }

    // Automatically restarts the game after a victory has been declared
    private IEnumerator RestartFromVictory() {
        yield return new WaitForSeconds(3f); // TODO replace the 3f with a public variable        
        victoryBanner.SetActive(false);
        TogglePause();
        ingameMng.ToggleMenus(6);
    }
    
    // Restarts when the game with the same amount of players
    public void RestartWithSameNumbers() {
        // turn off the playagain menu
        ingameMng.playAgain.SetActive(!ingameMng.playAgain.activeSelf);

        // Set all players to non-active
        FillActivePlayerList();

        // Destroy the arena
        Destroy(arena.gameObject);

        // Instantiate the new one
        newArena = Instantiate(arenaPrefab, Vector3.zero, Quaternion.identity);
        newArena.name = arenaName;
        arena = newArena.GetComponent<Code_Arena>();

        // Assign a new bouncer to the arena
        arena.bouncer = bouncer;
        // Reset the bouncers position
        bouncer.ResetPosition();

        // Reset the pickUpMng
        pickUpMng.ResetPickUps();

        // Reset firstTimeStarting
        firstTimeStarting = false;

        // Reset player stamina if necesarry
        foreach (GameObject player in activePlayers) {
            player.GetComponent<Code_Player>().ResetPlayer();
        }

        // Reset the Camera bonus
        camCon.UpdateBonuses(-1); // The UpdateBonusses itself increments the int value with 1

        // Call SpawnPlayers
        SpawnPlayers();
    }    
}
