using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    public GameObject bartolo;
    Player playerScript;
    GameManager gameManagerScript;
    AudioManager audioManagerScript;

    void Start() 
    {
        playerScript = transform.parent.GetComponent<Player>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    public IEnumerator WaitForInteract()
    {
        if (playerScript.canInteract == true)
        {
            playerScript.canInteract = false;
        }
        playerScript.isCanInteractCoroutineRunning = true;
        yield return new WaitForSeconds(0.3f);
        playerScript.isCanInteractCoroutineRunning = false;
        playerScript.canInteract = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block") && playerScript.canInteract && !playerScript.isCanInteractCoroutineRunning)
        {
            Block blockScript = other.gameObject.GetComponent<Block>();
            StartCoroutine(WaitForInteract());

            if ((playerScript.playerState == 2 || playerScript.playerState == 3 || playerScript.hasSuperPower) && blockScript.blockType != 1)
            {
                audioManagerScript.PlaySoundEffect(audioManagerScript.breakBlockSound);
                Destroy(other.gameObject);
            }
            else if((playerScript.playerState == 1 && playerScript.hasSuperPower) && blockScript.blockType != 1)
            {
                audioManagerScript.PlaySoundEffect(audioManagerScript.breakBlockSound);
                Destroy(other.gameObject);
            }
            else if (playerScript.playerState == 1 && blockScript.blockType == 1 && blockScript.coins != 0)
            {
                audioManagerScript.PlaySoundEffect(audioManagerScript.bumpSound);
                audioManagerScript.PlaySoundEffect(audioManagerScript.getCoinSound);
                StartCoroutine(blockScript.MoveBlock());
                gameManagerScript.UpdateCoins(1);
                gameManagerScript.UpdateScore(100);
                blockScript.coins--;
            }
            else if (playerScript.playerState == 1 && blockScript.blockType == 1 && blockScript.coins == 0)
            {
                audioManagerScript.PlaySoundEffect(audioManagerScript.bumpSound);
                StartCoroutine(blockScript.MoveBlock());
            }
            else if (playerScript.playerState == 1)
            {
                audioManagerScript.PlaySoundEffect(audioManagerScript.bumpSound);
                StartCoroutine(blockScript.MoveBlock());
            }
        }

        if (other.gameObject.CompareTag("DecorativeBlock") && playerScript.canInteract && !playerScript.isCanInteractCoroutineRunning)
        {
            StartCoroutine(WaitForInteract());
            audioManagerScript.PlaySoundEffect(audioManagerScript.bumpSound);
        }
    }
}
