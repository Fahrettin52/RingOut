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
    public Code_Arena arena; // The arena in the scene

    [Header("Ingame Menu")]
    public Code_InGameMenuManager ingameMng; // The Code_IngameManager in the scene

    [Header("PlayerSelect")]
    public GameObject playerSelectMenu; // Menu for the player selection screen

    [Header("Sound")]
    public Code_SoundManager soundMng; // The sound manager

    [Header("Countdown")]
    public float countdownSeconds; // How long the countdown will take
    private float countdownSecondsSaved; // Saves the countdownSeconds so it can be reset
    public GameObject countdownBanner; // Banner for the countdown UI
    public Text countdownText; // The text that'll hold the countdown

    [Header("Victory Banner")]
    public GameObject victoryBanner; // The sprite that appears once there's a winner
    public Text victoryMessageText; // Message holder in the VictoryBanner GameObject
    public string victoryMessage; // Message that precedes the victors name

    void Start() {
        TogglePause();
    }

    // Centralization of turning pause on and off
    private void TogglePause() {
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

        // Selects which spawnpoint position should be used
        FillSpawnPositions();

        // Determines how many players are being activated
        FillActivePlayerList();

        // Changes the background music
        soundMng.TurnOffSelectedSound(0);
        soundMng.PlayAmbientMusic();

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

        // Toggle pause
        TogglePause();

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
            yield return new WaitForSeconds(1f);
        }

        // Resets the necessary variables for later reuse
        countdownBanner.SetActive(false);
        countdownSeconds = countdownSecondsSaved;

        // Activates the IngameMenuManager
        ingameMng.allowStart = true;

        // Allows all players to finally start playing
        AllowMovement();

        // Activate the arena crumble
        arena.StartCrumble();
    }

    // Let the players move
    private void AllowMovement() {
        foreach (GameObject player in activePlayers) {
            player.GetComponent<Code_Player>().NormalizeMoveState();
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
            StartCoroutine(RestartFromVictory());
        }
    }

    // Stops the gameplay and announces the winner through the VictoryBanner GameObject
    private void AnnounceWinner(Code_Player winner) {
        //Time.timeScale = 0; // TODO Check for a better location for this snipet of code
        victoryBanner.SetActive(true);
        victoryMessageText.text = victoryMessage + winner.playerNumber;
    }

    // Automatically restarts the game after a victory has been declared
    private IEnumerator RestartFromVictory() {
        yield return new WaitForSeconds(3f); // TODO replace the 3f with a public variable        
        victoryBanner.SetActive(false);
        RestartWithSameNumbers();
        TogglePause();
    }
    
    // Restarts when the game with the same amount of players
    public void RestartWithSameNumbers() {
        // Set all players to non-active
        FillActivePlayerList();

        //// Turns of all the players
        //foreach (GameObject player in activePlayers) {
        //    player.SetActive(false);
        //}

        // Reset the arenaParts
        arena.ResetArena();

        // Call SpawnPlayers
        SpawnPlayers();
    }    
}
