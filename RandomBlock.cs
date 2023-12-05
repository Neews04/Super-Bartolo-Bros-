using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlock : MonoBehaviour
{
    Player playerScript;
    HeadCollider headColliderScript;
    GameManager gameManagerScript;
    AudioManager audioManagerScript;
    public Animator animator;

    public GameObject peyotePrefab;
    public GameObject chilePrefab;
    public GameObject sunPrefab;
    public GameObject sombreroPrefab;
    public Sprite spriteToChange;
    public bool canChange = true;
    public int content = 0;
    int scoreToAdd;

    void Start()
    {
        playerScript = GameObject.Find("Bartolo").GetComponent<Player>();
        headColliderScript = GameObject.Find("PlayerHead").GetComponent<HeadCollider>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        animator.SetBool("canUse", canChange);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHead") && playerScript.canInteract)
        {
            audioManagerScript.PlaySoundEffect(audioManagerScript.bumpSound);

            playerScript.canInteract = false;
            StartCoroutine(headColliderScript.WaitForInteract());

            if (canChange)
            {
                SpriteRenderer objectSprite = GetComponent<SpriteRenderer>();
                objectSprite.sprite = spriteToChange;
                canChange = false;
                animator.SetBool("canUse", false);

                if (content == 0)
                {
                    audioManagerScript.PlaySoundEffect(audioManagerScript.getCoinSound);
                    scoreToAdd = 200;
                    gameManagerScript.UpdateScore(scoreToAdd);
                    gameManagerScript.UpdateCoins(1);
                }
                else if (content == 1)
                {
                    audioManagerScript.PlaySoundEffect(audioManagerScript.powerupAppearsSound);
                    if (playerScript.playerState == 1)
                    {
                        Vector3 spawnOffset = new Vector3(0.0f, 0.0f, 1.0f);
                        Instantiate(peyotePrefab, (transform.position + spawnOffset), peyotePrefab.transform.rotation);
                    }
                    else if (playerScript.playerState == 2)
                    {
                        Vector3 spawnOffset = new Vector3(0.0f, 0.0f, 1.0f);
                        Instantiate(chilePrefab, (transform.position + spawnOffset), chilePrefab.transform.rotation);
                    }
                }
                else if (content == 2)
                {
                    if (playerScript.playerState == 1)
                    {
                        audioManagerScript.PlaySoundEffect(audioManagerScript.powerupAppearsSound);
                        Vector3 spawnOffset = new Vector3(0.0f, 0.0f, 1.0f);
                        Instantiate(sunPrefab, (transform.position + spawnOffset), peyotePrefab.transform.rotation);
                    }
                    else if (playerScript.playerState != 1)
                    {
                        audioManagerScript.PlaySoundEffect(audioManagerScript.powerupAppearsSound);
                        Vector3 spawnOffset = new Vector3(0.0f, 0.0f, 1.0f);
                        Instantiate(sunPrefab, (transform.position + spawnOffset), sunPrefab.transform.rotation);
                    }
                }
                else if (content == 3)
                {
                    if (playerScript.playerState == 1)
                    {
                        audioManagerScript.PlaySoundEffect(audioManagerScript.powerupAppearsSound);
                        Vector3 spawnOffset = new Vector3(0.0f, 0.0f, 1.0f);
                        Instantiate(sunPrefab, (transform.position + spawnOffset), peyotePrefab.transform.rotation);
                    }
                    else if (playerScript.playerState != 1)
                    {
                        audioManagerScript.PlaySoundEffect(audioManagerScript.powerupAppearsSound);
                        Vector3 spawnOffset = new Vector3(0.0f, 0.0f, 1.0f);
                        Instantiate(sunPrefab, (transform.position + spawnOffset), sombreroPrefab.transform.rotation);
                    }
                }
            }
        }
    }
}
