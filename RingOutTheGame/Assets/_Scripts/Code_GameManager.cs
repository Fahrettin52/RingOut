using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Code_GameManager : MonoBehaviour {
    public GameObject[] players; // Array of players in the game    
    private List<GameObject> activePlayers = new List<GameObject>(); // List of players that are playing the game
    public GameObject[] spawnLocations; // Array of array of spawn locations, based on the chosen amount of players
    private List<Vector3> spawnPosition = new List<Vector3>(); // Private list of Vector3 children objects in the items of spawnLocations
    public int playersSelected; // The amount of players selected to play the game
    private int oldPlayersSelected; // Value that saves the playerSelected, so it can compare it later on

    [Header("PlayerSelect")]
    public GameObject playerSelectMenu; // Menu for the player selection screen

    [Header("Countdown")]
    public float countdownSeconds; // How long the countdown will take
    private float countdownSecondsSaved; // Saves the countdownSeconds so it can be reset
    public GameObject countdownBanner; // Banner for the countdown UI
    public Text countdownText; // The text that'll hold the countdown

    [Header("Victory Banner")]
    public GameObject victoryBanner; // The sprite that appears once there's a winner
    public Text victoryMessageText; // Message holder in the VictoryBanner GameObject
    public string victoryMessage; // Message that precedes the victors name

    void Start () {
        countdownSecondsSaved = countdownSeconds;
        OpenSelectPlayersMenu();
    }

    // Activates the menu where the player can select the amount of players
    private void OpenSelectPlayersMenu() {
        playerSelectMenu.SetActive(true);
    }

    // Player Selected a number of participants through the PlayerSelectMenu
    public void SelectedNumberOfPlayers(int numberOfPlayers) {
        playersSelected = numberOfPlayers;

        // Selects which spawnpoint position should be used
        FillSpawnPositions();

        // Determines how many players are being activated
        FillActivePlayerList();

        // Spawns the players to their respective spawnPoint
        SpawnPlayers();
    }

    // Fills the spawnVector3 list with new values based on the number of players playing
    private void FillSpawnPositions() {
        if (oldPlayersSelected != playersSelected) {
            spawnPosition.Clear();
            foreach (Transform child in spawnLocations[playersSelected].transform) {
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
        // Places the players on their respective spawn location
        for (int i = 0; i < activePlayers.Count; i++) {
            activePlayers[i].transform.position = spawnPosition[i];
        }

        // Activates the players so they are visible
        foreach (GameObject player in activePlayers) {
            player.SetActive(true);
        }

        // Starts the countdown of the game
        StartCoroutine(StartGameCountdown());
    }

    // Countdown to the players being able to start playing
    private IEnumerator StartGameCountdown() {
        // Sets the adequate variables to desired values;
        int curSeconds = 0;
        countdownBanner.SetActive(true);
        
        // A countdown system using the while loop
        while (curSeconds < countdownSeconds) {
            countdownText.text = countdownSeconds.ToString();            
            countdownSeconds--;
            curSeconds++;
            yield return new WaitForSeconds(1f);
        }

        // Resets the necessary variables for later reuse
        countdownBanner.SetActive(false);
        countdownSeconds = countdownSecondsSaved;

        // Allows all players to finally start playing
        AllowMovement();
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
            AnnounceWinner(activePlayers[0].GetComponent<Code_Player>());
        }
    }

    

    // Stops the gameplay and announces the winner through the VictoryBanner GameObject
    private void AnnounceWinner(Code_Player winner) {
        Time.timeScale = 0; // TODO Check for a better location for this snipet of code
        victoryBanner.SetActive(true);
        victoryMessageText.text = victoryMessage + winner.playerNumber;
    }
}
