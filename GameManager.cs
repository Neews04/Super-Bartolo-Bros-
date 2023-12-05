using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    Player playerScript;
    AudioManager audioManagerScript;
    GameObject bartolo;

    public bool isGameActive = true;
    public bool isEndOfLevel = false;
    public bool isLevelStarting = false;
    public bool isFirstLevel = true;
    public bool isTimeUp = false;
    public bool updateLimits = false;
    public string sceneToLoad = "1-1";
    public float leftLimit;
    public float rightLimit;
    public float lowerLimit;
    public float upperLimit;
    public Vector3 bartoloSpawn;
    public Vector3 bartoloSpawnOffset;
    public Vector3 bartoloSpawnUnderground;
    public Vector3 bartoloSpawnAboveground;
    public Vector3 bartoloSpawnAbovegroundOffset;
    public Vector3 bartoloSpawnEarth;
    public Vector3 bartoloSpawnEarthOffset;
    public Vector3 cameraUndergroundCoordenates;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI worldText;
    public TextMeshProUGUI startingGameWorldText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI topScoreText;
    public Image startingLevelScreen;
    public Image gameOverScreen;
    public Image timeUpScreen;
    public GameObject menuScreen;
    public AudioClip currentTheme;
    float score;
    float coins;
    public int time = 400;
    public int levelType;
    public int groundLevel;
    float topScore = 0;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerScript = GameObject.Find("Bartolo").GetComponent<Player>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        bartolo = GameObject.Find("Bartolo");
    }

    void UpdateInformation()
    {
        playerScript.numberOfLifes = 3;
        score = 0;
        coins = 0;
        scoreText.text = "000000";
        coinsText.text = "00";
        lifesText.text = "" + playerScript.numberOfLifes;
        isFirstLevel = true;
    }

    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;

        if (score < 10)
        {
            scoreText.text = "00000" + score;
        }
        else if (score < 100)
        {
            scoreText.text = "0000" + score;
        }
        else if (score < 1000)
        {
            scoreText.text = "000" + score;
        }
        else if (score < 10000)
        {
            scoreText.text = "00" + score;
        }
        else if (score < 100000)
        {
            scoreText.text = "0" + score;
        }
        else
        {
            scoreText.text = "" + score;
        }
    }

    public void UpdateTopScore()
    {
        if (score > topScore)
        {
            topScore = score;
        }

        if (topScore < 10)
        {
            topScoreText.text = "00000" + topScore;
        }
        else if (topScore < 100)
        {
            topScoreText.text = "0000" + topScore;
        }
        else if (topScore < 1000)
        {
            topScoreText.text = "000" + topScore;
        }
        else if (topScore < 10000)
        {
            topScoreText.text = "00" + topScore;
        }
        else if (topScore < 100000)
        {
            topScoreText.text = "0" + topScore;
        }
        else
        {
            topScoreText.text = "" + topScore;
        }
    }

    public void UpdateCoins(float coinsToAdd)
    {
        coins += coinsToAdd;

        if (coins == 100)
        {
            coins = 0;
            playerScript.numberOfLifes++;
            audioManagerScript.PlaySoundEffect(audioManagerScript.extraLifeSound);
        }

        if (coins < 10)
        {
            coinsText.text = "0" + coins;
        }
        else
        {
            coinsText.text = "" + coins;
        }
    }

    public void UpdateWorldNumber(string worldNumber)
    {
        worldText.text = worldNumber;
        startingGameWorldText.text = worldNumber;
    }

    void ResetTime()
    {
        timeText.text = "" + 400;
    }

    public void UpdateLifes(int numberOfLifes)
    {
        lifesText.text = "" + numberOfLifes;
    }

    public void PlayMusic(int levelType)
    {
        audioManagerScript.isLevelTheme = true;
        if (levelType == 0)
        {
            audioManagerScript.PlayMusic(audioManagerScript.groundTheme);
        }
        else if (levelType == 1)
        {
            audioManagerScript.PlayMusic(audioManagerScript.undergroundTheme);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        bartolo.transform.position = bartoloSpawn;
        StartCoroutine(StartingLevel());
    }

    public void ReloadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (levelType == 1)
        {
            bartolo.transform.position = bartoloSpawnOffset;
        }
        else if (levelType == 0)
        {
            bartolo.transform.position = bartoloSpawn;
        }
        StartCoroutine(ResettingLevel());
        StartCoroutine(playerScript.ChangeSprite());
    }

    public void StartGame()
    {
        menuScreen.gameObject.SetActive(false);
        UpdateInformation();
        SceneManager.LoadScene(sceneToLoad);
        levelType = 0;
        groundLevel = 0;
        currentTheme = audioManagerScript.groundTheme;
        StartCoroutine(StartingLevel());
        leftLimit = 0f; rightLimit = 202f; lowerLimit = 0.5f; upperLimit = 0.5f;
        bartoloSpawn = new Vector3(-1.5f, -4.55f, 0.0f);
        bartoloSpawnUnderground = new Vector3(28.0f, -19.1f, 0.0f);
        bartoloSpawnAboveground = new Vector3(110.0f, -1.55f, 0.0f);
        bartoloSpawnAbovegroundOffset = new Vector3(110.0f, -4.55f, 0.0f);
        cameraUndergroundCoordenates = new Vector3(34, -24.5f, -10.0f);
        bartolo.transform.position = bartoloSpawn;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Menu");
        sceneToLoad = "1-1";
        menuScreen.gameObject.SetActive(true);
    }
    
    public IEnumerator Timer()
    {
        while (isGameActive)
        {
            if (time < 0)
            {
                break;
            }

            if (time == 100)
            {
                isGameActive = false;
                if (groundLevel == 0)
                {
                    audioManagerScript.PlayMusic(audioManagerScript.groundAceleratedTheme);
                }
                else if (groundLevel == 1 || groundLevel == 2)
                {
                    audioManagerScript.PlayMusic(audioManagerScript.undergroundAceleratedTheme);                
                }

                yield return new WaitForSeconds(3.0f);
                isGameActive = true;
            }

            yield return new WaitForSeconds(0.5f);
            time--;
            timeText.text = "" + time;
        }
    }

    public IEnumerator LevelEnd()
    {
        isEndOfLevel = true;
        while (time >= 0)
        {
            if (time == 0)
            {
                StartCoroutine(LevelEndAnimation());
                break;
            }

            yield return new WaitForSeconds(0.01f);
            time--;
            score += 5;
            if (score < 10)
            {
                scoreText.text = "00000" + score;
            }
            else if (score < 100)
            {
                scoreText.text = "0000" + score;
            }
            else if (score < 1000)
            {
                scoreText.text = "000" + score;
            }
            else if (score < 10000)
            {
                scoreText.text = "00" + score;
            }
            else if (score < 100000)
            {
                scoreText.text = "0" + score;
            }
            else
            {
                scoreText.text = "" + score;
            }
            timeText.text = "" + time;
        }
    }

    IEnumerator StartingLevel() 
    {
        isLevelStarting = true;
        timeText.gameObject.SetActive(false);
        if (isFirstLevel)
        {
            isFirstLevel = false;
        }
        else
        {
            string[] worldNumberParts = sceneToLoad.Split('-');
            if (worldNumberParts.Length == 2)
            {
                string worldNumberWithSpaces = worldNumberParts[0].Trim() + " - " + worldNumberParts[1].Trim();
                UpdateWorldNumber(worldNumberWithSpaces);
            }

        }
        startingLevelScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        startingLevelScreen.gameObject.SetActive(false);
        time = 400;
        ResetTime();
        timeText.gameObject.SetActive(true);
        isLevelStarting = false;
        isGameActive = true;
        PlayMusic(levelType);
        StartCoroutine(Timer());
    }

    IEnumerator ResettingLevel()
    {
        isLevelStarting = true;
        timeText.gameObject.SetActive(false);
        if (isTimeUp)
        {
            timeUpScreen.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            timeUpScreen.gameObject.SetActive(false);
            isTimeUp = false;
        }
        startingLevelScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        startingLevelScreen.gameObject.SetActive(false);
        time = 400;
        ResetTime();
        timeText.gameObject.SetActive(true);
        isLevelStarting = false;
        isGameActive = true;
        PlayMusic(levelType);
        StartCoroutine(Timer());
    }

    IEnumerator LevelEndAnimation()
    {
        yield return new WaitForSeconds(3.0f);
        isEndOfLevel = false;
        LoadScene(sceneToLoad);
    }

    public IEnumerator DeathTime()
    {
        audioManagerScript.StopMusic();
        audioManagerScript.PlaySoundEffect(audioManagerScript.marioDiesSound);
        yield return new WaitForSeconds(4.0f);
        ReloadScene(sceneToLoad);
    }

    public IEnumerator GameOver()
    {
        audioManagerScript.StopMusic();
        audioManagerScript.PlaySoundEffect(audioManagerScript.marioDiesSound);
        yield return new WaitForSeconds(4.0f);
        timeText.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(true);
        StartCoroutine(playerScript.ChangeSprite());
        audioManagerScript.PlaySoundEffect(audioManagerScript.gameOverSound);
        yield return new WaitForSeconds(4.5f);
        gameOverScreen.gameObject.SetActive(false);
        bartolo.transform.position = new Vector3(-5.0f, -4.55f, 0.0f);
        playerScript.playerState = 1;
        UpdateTopScore();
        RestartGame();
    }
}
