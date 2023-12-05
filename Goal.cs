using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Player playerScript;
    GameManager gameManagerScript;
    AudioManager audioManagerScript;
    Rigidbody2D playerRb;

    public int goalValue;
    public int levelType;
    public string sceneToLoad;
    public float leftLimit;
    public float rightLimit;
    public float lowerLimit;
    public float upperLimit;
    public Vector3 spawnCoordenates;
    public Vector3 spawnOffsetCoordenates;
    public Vector3 spawnUndergroundCoordenates;
    public Vector3 spawnAbovegroundCoordenates;
    public Vector3 spawnAbovegroundOffsetCoordenates;
    public Vector3 spawnEarthCoordenates;
    public Vector3 spawnEarthOffsetCoordenates;
    public Vector3 cameraUndergroundCoordenates;

    void Start()
    {
        playerScript = GameObject.Find("Bartolo").GetComponent<Player>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        playerRb = GameObject.Find("Bartolo").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameManagerScript.isGameActive)
        {
            StartCoroutine(StageClear());
        }
    }

    IEnumerator FlagpoleEffect()
    {
        audioManagerScript.PlaySoundEffect(audioManagerScript.flagpoleSound);
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator StageClearEffect()
    {
        audioManagerScript.PlaySoundEffect(audioManagerScript.stageClearSound);
        yield return null;
    }

    IEnumerator StageClear()
    {
        gameManagerScript.isGameActive = false;
        audioManagerScript.StopMusic();
        yield return StartCoroutine(FlagpoleEffect());
        yield return StartCoroutine(StageClearEffect());
        gameManagerScript.UpdateScore(goalValue * (100 * goalValue));
        gameManagerScript.sceneToLoad = sceneToLoad;
        gameManagerScript.levelType = levelType;
        gameManagerScript.leftLimit = leftLimit;
        gameManagerScript.rightLimit = rightLimit;
        gameManagerScript.lowerLimit = lowerLimit;
        gameManagerScript.upperLimit = upperLimit;
        gameManagerScript.bartoloSpawn = spawnCoordenates;
        gameManagerScript.bartoloSpawnOffset = spawnOffsetCoordenates;
        gameManagerScript.bartoloSpawnUnderground = spawnUndergroundCoordenates;
        gameManagerScript.bartoloSpawnAboveground = spawnAbovegroundCoordenates;
        gameManagerScript.bartoloSpawnAbovegroundOffset = spawnAbovegroundOffsetCoordenates;
        gameManagerScript.bartoloSpawnEarth = spawnEarthCoordenates;
        gameManagerScript.bartoloSpawnEarthOffset = spawnEarthOffsetCoordenates;
        gameManagerScript.cameraUndergroundCoordenates = cameraUndergroundCoordenates;
        StartCoroutine(gameManagerScript.LevelEnd());
    }
}
