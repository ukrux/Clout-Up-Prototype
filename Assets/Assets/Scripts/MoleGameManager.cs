using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MoleGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 30f;

    [Header("Spawn Randomness")]
    [SerializeField] private float minSpawnDelay = 0.2f;
    [SerializeField] private float maxSpawnDelay = 2f;

    [Header("Mole Randomness")]
    [SerializeField] private float minMoleTime = 0.5f;
    [SerializeField] private float maxMoleTime = 2.5f;

    [SerializeField] private int minMolesPerSpawn = 1;
    [SerializeField] private int maxMolesPerSpawn = 3;

    [Header("Rewards")]
    [SerializeField] private int xpPerHit = 10;
    [SerializeField] private int coinsPerHit = 1;

    [Header("Mole Spawn Audio")]
    [SerializeField] private AudioClip moleSpawnSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Gameplay UI")]
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text timerText;

    [Header("Scoreboard")]
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private TMP_Text finalXPText;
    [SerializeField] private TMP_Text finalCoinsText;

    private GameObject[] moles;

    private List<GameObject> activeMoles = new List<GameObject>();

    private int totalXP = 0;
    private int totalCoins = 0;

    private float timer;
    private bool gameRunning = false;

    private Coroutine spawnRoutine;


    // =========================
    // START
    // =========================

    private void Start()
    {
        moles = GameObject.FindGameObjectsWithTag("Mole");

        // Hide all moles and connect them to the manager
        foreach (GameObject mole in moles)
        {
            mole.SetActive(false);

            Mole moleScript = mole.GetComponent<Mole>();

            if (moleScript != null)
            {
                moleScript.gameManager = this;
            }
        }

        // Reset score
        totalXP = 0;
        totalCoins = 0;

        // Start timer
        timer = gameDuration;
        gameRunning = true;

        // Hide scoreboard
        scoreboard.SetActive(false);

        // Update UI
        UpdateGameplayUI();

        // Start random spawning
        spawnRoutine = StartCoroutine(SpawnLoop());
    }


    // =========================
    // UPDATE
    // =========================

    private void Update()
    {
        if (!gameRunning)
            return;

        timer -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timer).ToString() + "s";
        }

        if (timer <= 0)
        {
            EndGame();
        }
    }


    // =========================
    // RANDOM SPAWN LOOP
    // =========================

    private IEnumerator SpawnLoop()
    {
        while (gameRunning)
        {
            // Completely random delay
            float randomDelay = Random.Range(
                minSpawnDelay,
                maxSpawnDelay
            );

            yield return new WaitForSeconds(randomDelay);

            if (!gameRunning)
                yield break;

            SpawnRandomMoles();
        }
    }


    // =========================
    // SPAWN 1-3 MOLES
    // =========================

    private void SpawnRandomMoles()
    {
        // Find currently inactive moles
        List<GameObject> availableMoles = new List<GameObject>();

        foreach (GameObject mole in moles)
        {
            if (!mole.activeSelf)
            {
                availableMoles.Add(mole);
            }
        }

        if (availableMoles.Count == 0)
            return;

        // Random number of moles
        int amountToSpawn = Random.Range(
            minMolesPerSpawn,
            maxMolesPerSpawn + 1
        );

        // Don't exceed available holes
        amountToSpawn = Mathf.Min(
            amountToSpawn,
            availableMoles.Count
        );


        // Spawn random moles
        for (int i = 0; i < amountToSpawn; i++)
        {
            int randomIndex = Random.Range(
                0,
                availableMoles.Count
            );

            GameObject mole = availableMoles[randomIndex];

            availableMoles.RemoveAt(randomIndex);

            // Activate mole
            mole.SetActive(true);

            // Track active mole
            activeMoles.Add(mole);

            // Give this mole its own random lifetime
            float randomLifetime = Random.Range(
                minMoleTime,
                maxMoleTime
            );

            StartCoroutine(
                HideMoleAfterTime(
                    mole,
                    randomLifetime
                )
            );
        }


        // =========================
        // ONE SFX PER SPAWN EVENT
        // =========================

        if (moleSpawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(moleSpawnSound);
        }
    }


    // =========================
    // RANDOM MOLE DISAPPEAR
    // =========================

    private IEnumerator HideMoleAfterTime(
        GameObject mole,
        float lifetime
    )
    {
        yield return new WaitForSeconds(lifetime);

        if (mole != null && mole.activeSelf)
        {
            mole.SetActive(false);

            activeMoles.Remove(mole);
        }
    }


    // =========================
    // MOLE CLICKED
    // =========================

    public void MoleClicked(GameObject mole)
    {
        if (!gameRunning)
            return;

        // Make sure this mole is currently active
        if (!activeMoles.Contains(mole))
            return;

        // Give XP
        totalXP += xpPerHit;

        // Give coins
        totalCoins += coinsPerHit;

        // Update UI
        UpdateGameplayUI();

        // Hide mole
        mole.SetActive(false);

        // Remove from active list
        activeMoles.Remove(mole);
    }


    // =========================
    // UPDATE GAMEPLAY UI
    // =========================

    private void UpdateGameplayUI()
    {
        if (xpText != null)
        {
            xpText.text = totalXP.ToString() + " XP";
        }

        if (coinText != null)
        {
            coinText.text = totalCoins.ToString();
        }

        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timer).ToString() + "s";
        }
    }


    // =========================
    // END GAME
    // =========================

    private void EndGame()
    {
        gameRunning = false;

        // Stop spawning
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        // Hide all moles
        foreach (GameObject mole in moles)
        {
            mole.SetActive(false);
        }

        activeMoles.Clear();

        // Final XP
        if (finalXPText != null)
        {
            finalXPText.text = totalXP.ToString() + " XP";
        }

        // Final Coins
        if (finalCoinsText != null)
        {
            finalCoinsText.text = totalCoins.ToString();
        }

        // Show scoreboard
        scoreboard.SetActive(true);

        // Timer
        if (timerText != null)
        {
            timerText.text = "0s";
        }
    }
}