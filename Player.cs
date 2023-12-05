using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    static Player _instance;

    public static Player Instance
    {
        get { return _instance; }
    }

    Rigidbody2D playerRb;
    GameManager gameManagerScript;
    SpriteRenderer spriteRenderer;
    public Sprite smallBartolo;
    public Sprite peyoteBartolo;
    public Sprite chileBartolo;
    public Sprite solBartolo;
    public Sprite playerDied;

    public AudioManager audioManagerScript;
    public Animator animator;

    public GameObject playerHead;
    public GameObject playerFeet;
    public GameObject fireballPrefab;
    public GameObject pipelineToDesactive;

    public bool isOnGround = true;
    public bool canInteract = true;
    public bool canReceiveDamage = true;
    public bool canSpitFireballs = false;
    public bool hasSuperPower = false;
    public bool canUseShortcut = false;
    public bool canFollowPlayer = false;
    public int playerState = 1;
    public int numberOfLifes;
    float speed = 5.5f;
    float jumpVelocity = 19.0f;
    float fallMultiplier = 2.5f;
    bool isShortcutPipeCoroutineRunning = false;
    bool isExitPipe = false;
    public bool isCanInteractCoroutineRunning = false;
    int scoreToAdd;

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
        playerRb = GetComponent<Rigidbody2D>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (gameManagerScript.isGameActive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            animator.SetFloat("Horizontal", Mathf.Abs(horizontalInput));
            Vector3 movement = new Vector3(horizontalInput * speed * Time.deltaTime, 0.0f, 0.0f);
            transform.position += movement;

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                Jump();

                if (playerState == 1)
                {
                    audioManagerScript.PlaySoundEffect(audioManagerScript.smallJumpSound);
                }
                else if (playerState == 2 || playerState == 3)
                {
                    audioManagerScript.PlaySoundEffect(audioManagerScript.bigJumpSound);
                }
            }
            if (Input.GetKeyDown(KeyCode.F) && canSpitFireballs && playerState == 3)
            {
                Fireball();
            }

            if (playerRb.velocity.y < 0)
            {
                playerRb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }

            if (canUseShortcut && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
            {
                canUseShortcut = false;
                pipelineToDesactive.gameObject.SetActive(false);
                StartCoroutine(UseShortcutPipe());
            }

            if (gameManagerScript.time == 0)
            {
                if (numberOfLifes > 1)
                {
                    gameManagerScript.isGameActive = false;
                    numberOfLifes--;
                    gameManagerScript.UpdateLifes(numberOfLifes);
                    gameManagerScript.isTimeUp = true;
                    StartCoroutine(gameManagerScript.DeathTime());
                }
                else
                {
                    gameManagerScript.isGameActive = false;
                    numberOfLifes--;
                    StartCoroutine(gameManagerScript.GameOver());
                }
            }
        }
    }

    void Jump()
    {
        playerRb.velocity = new Vector2(playerRb.velocity.y, jumpVelocity);
    }

    void Fireball()
    {
        Vector3 spawnFireballOffset = new Vector3(0.6f, 0.15f, 0.0f);
        Instantiate(fireballPrefab, (transform.position + spawnFireballOffset), fireballPrefab.transform.rotation);
        audioManagerScript.PlaySoundEffect(audioManagerScript.fireballSound);
        canSpitFireballs = false;
        StartCoroutine(SplitFireball());
    }

    void ChangePlayerColliders(Collider2D collider)
    {
        if (playerState == 1)
        {
            Vector3 playerColliders = new Vector3(0.9f, 2.15f, 1.0f);
            collider.transform.localScale = playerColliders;
        }
        else if (playerState != 1)
        {
            Vector3 playerColliders = new Vector3(0.9f, 1.25f, 1.0f);
            collider.transform.localScale = playerColliders;
        }
    }

    void ChangeHeadColliders(Collider2D collider)
    {
        if (playerState == 1)
        {
            collider.offset = new Vector3(0.0f, 1.1f, 1.0f);
        }
        else if (playerState != 1)
        {
            collider.offset = new Vector3(0.0f, 0.65f, 1.0f);
        }
    }

    void ChangeFeetColliders(Collider2D collider)
    {
        if (playerState == 1)
        {
            collider.offset = new Vector3(0.0f, -1.1f, 1.0f);
        }
        else if (playerState != 1)
        {
            collider.offset = new Vector3(0.0f, -0.65f, 1.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            audioManagerScript.PlaySoundEffect(audioManagerScript.getCoinSound);
            Destroy(collision.gameObject);
            scoreToAdd = 100;
            gameManagerScript.UpdateScore(scoreToAdd);
            gameManagerScript.UpdateCoins(0.5f);
        }

        if (collision.gameObject.CompareTag("ExitPipe") && !isShortcutPipeCoroutineRunning)
        {
            gameManagerScript.isGameActive = false;
            isExitPipe = true;
            pipelineToDesactive = collision.transform.parent.parent.gameObject;
            pipelineToDesactive.SetActive(false);
            StartCoroutine(UseExitPipe());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (playerState == 1 && !hasSuperPower && canReceiveDamage)
            {
                if (numberOfLifes > 1)
                {
                    gameManagerScript.isGameActive = false;
                    canSpitFireballs = false;
                    spriteRenderer.sprite = playerDied;
                    numberOfLifes--;
                    gameManagerScript.UpdateLifes(numberOfLifes);
                    StartCoroutine(gameManagerScript.DeathTime());
                }
                else
                {
                    gameManagerScript.isGameActive = false;
                    canSpitFireballs = false;
                    spriteRenderer.sprite = playerDied;
                    numberOfLifes--;
                    StartCoroutine(gameManagerScript.GameOver());
                }
            }
            else if ((playerState == 2 || playerState == 3) && !hasSuperPower && canReceiveDamage)
            {
                playerState = 1;
                StartCoroutine(IsTakingDamage());
            }
            else if (hasSuperPower)
            {
                Destroy(collision.gameObject);
                gameManagerScript.UpdateScore(50);
                audioManagerScript.PlaySoundEffect(audioManagerScript.kickEnemySound);
            }
        }

        if (collision.gameObject.CompareTag("Peyote"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(Powerup());
            spriteRenderer.sprite = peyoteBartolo;
            playerState = 2;
            scoreToAdd = 1000;
            gameManagerScript.UpdateScore(scoreToAdd);
        }

        if (collision.gameObject.CompareTag("Chile"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(Powerup());
            if (playerState == 2)
            {
                spriteRenderer.sprite = chileBartolo;
                playerState = 3;
            }
            else
            {
                spriteRenderer.sprite = peyoteBartolo;
                playerState = 2;
            }
            scoreToAdd = 1000;
            gameManagerScript.UpdateScore(scoreToAdd);
            canSpitFireballs = true;
        }

        if (collision.gameObject.CompareTag("Sun"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(SuperPower());
            spriteRenderer.sprite = solBartolo;
            scoreToAdd = 1000;
            gameManagerScript.UpdateScore(scoreToAdd);
        }
        if (collision.gameObject.CompareTag("Sombrero"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(ExtraLife());
            numberOfLifes++;
            gameManagerScript.UpdateLifes(1);
        }
    }

    IEnumerator IsTakingDamage()
    {
        canReceiveDamage = false;
        audioManagerScript.PlaySoundEffect(audioManagerScript.enterPipeSound);
        yield return new WaitForSeconds(2.0f);
        spriteRenderer.sprite = smallBartolo;
        canReceiveDamage = true;
    }

    IEnumerator SplitFireball()
    {
        yield return new WaitForSeconds(0.3f);
        canSpitFireballs = true;
    }

    IEnumerator Powerup()
    {
        audioManagerScript.PlaySoundEffect(audioManagerScript.powerupSound);
        yield return null;
    }

    IEnumerator ExtraLife()
    {
        audioManagerScript.PlaySoundEffect(audioManagerScript.extraLifeSound);
        yield return null;
    }

    IEnumerator SuperPower()
    {
        spriteRenderer.sprite = solBartolo;
        hasSuperPower = true;
        audioManagerScript.PlayMusic(audioManagerScript.superPowerTheme);
        yield return new WaitForSeconds(10.0f);
        audioManagerScript.PlayMusic(gameManagerScript.currentTheme);
        hasSuperPower = false;
        if (playerState == 1)
        {
            spriteRenderer.sprite = smallBartolo;
        }
        else if (playerState == 2)
        {
            spriteRenderer.sprite = peyoteBartolo;
        }
        else if (playerState == 3)
        {
            spriteRenderer.sprite = chileBartolo;
        }
    }

    IEnumerator UseShortcutPipe()
    {
        isShortcutPipeCoroutineRunning = true;
        if (gameManagerScript.isGameActive)
        {
            gameManagerScript.isGameActive = false;
        }
        if (gameManagerScript.groundLevel != 2)
        {
            playerRb.gravityScale = 0;
            canInteract = false;
            yield return StartCoroutine(EnterPipe());
            audioManagerScript.StopMusic();
            yield return new WaitForSeconds(0.2f);
            transform.position = gameManagerScript.bartoloSpawnUnderground;
            gameManagerScript.groundLevel = 2;
            gameManagerScript.currentTheme = audioManagerScript.currentTheme;
            audioManagerScript.PlayMusic(audioManagerScript.undergroundTheme);
            pipelineToDesactive.gameObject.SetActive(true);
            gameManagerScript.isGameActive = true;
            StartCoroutine(gameManagerScript.Timer());
        }

        isShortcutPipeCoroutineRunning = false;
    }

    IEnumerator UseExitPipe()
    {
        if (gameManagerScript.groundLevel == 2)
        {
            playerRb.gravityScale = 0;
            yield return StartCoroutine(EnterPipe());
            audioManagerScript.StopMusic();
            yield return new WaitForSeconds(0.2f);
            transform.position = gameManagerScript.bartoloSpawnAboveground;
            if (gameManagerScript.levelType == 0)
            {
                gameManagerScript.groundLevel = 0;
                audioManagerScript.PlayMusic(audioManagerScript.groundTheme);
            }
            else if (gameManagerScript.levelType == 1)
            {
                gameManagerScript.groundLevel = 1;
                audioManagerScript.PlayMusic(audioManagerScript.undergroundTheme);
            }
            pipelineToDesactive.gameObject.SetActive(false);
            transform.position = gameManagerScript.bartoloSpawnAbovegroundOffset;
            canFollowPlayer = true;
            yield return StartCoroutine(ExitPipe());
            pipelineToDesactive.gameObject.SetActive(true);
            playerRb.gravityScale = 4.4f;
            gameManagerScript.isGameActive = true;
            StartCoroutine(gameManagerScript.Timer());
        }
        else if (gameManagerScript.groundLevel == 1)
        {
            playerRb.gravityScale = 0;
            yield return StartCoroutine(EnterPipe());
            audioManagerScript.StopMusic();
            yield return new WaitForSeconds(0.2f);
            transform.position = gameManagerScript.bartoloSpawnEarth;
            gameManagerScript.groundLevel = 0;
            audioManagerScript.PlayMusic(audioManagerScript.groundTheme);
            pipelineToDesactive.gameObject.SetActive(false);
            transform.position = gameManagerScript.bartoloSpawnEarthOffset;
            gameManagerScript.updateLimits = true;
            canFollowPlayer = true;
            yield return StartCoroutine(ExitPipe());
            pipelineToDesactive.gameObject.SetActive(true);
            playerRb.gravityScale = 4.4f;
            gameManagerScript.isGameActive = true;
            StartCoroutine(gameManagerScript.Timer());
        }
        yield return null;
    }

    IEnumerator EnterPipe()
    {
        audioManagerScript.PlaySoundEffect(audioManagerScript.enterPipeSound);
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - 3f, startPosition.z);
        if (gameManagerScript.groundLevel != 0 && isExitPipe)
        {
            endPosition = new Vector3(startPosition.x + 2f, startPosition.y + 0.3f, startPosition.z);
        }
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (gameManagerScript.groundLevel != 2)
        {
            playerRb.gravityScale = 4.4f;
        }
        canInteract = true;
    }

    IEnumerator ExitPipe()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 3f, startPosition.z);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isExitPipe = false;
        playerRb.gravityScale = 4.4f;
    }

    public IEnumerator DeathByFall()
    {
        gameManagerScript.isGameActive = false;
        playerState = 1;
        numberOfLifes--;
        gameManagerScript.UpdateLifes(numberOfLifes);
        StartCoroutine(gameManagerScript.DeathTime());
        yield return null;
    }

    public IEnumerator ChangeSprite()
    {
        spriteRenderer.sprite = smallBartolo;
        yield return null;
    }
}
