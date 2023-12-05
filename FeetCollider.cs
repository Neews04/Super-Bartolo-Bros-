using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FeetCollider : MonoBehaviour
{
    public GameObject bartolo;

    GameManager gameManagerScript;
    Player playerScript;
    Rigidbody2D playerRigidbody;

    public float yVelocityThreshold = 0.01f;

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerScript = transform.parent.GetComponent<Player>();
        playerRigidbody = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
            // Verificar si la velocidad en el eje Y está cambiando
            if (Mathf.Abs(playerRigidbody.velocity.y) > yVelocityThreshold)
            {
                // El jugador está cayendo o subiendo
                playerScript.isOnGround = false;
            }
            else
            {
                // El jugador no está cayendo ni subiendo significativamente, asumimos que está en el suelo
                playerScript.isOnGround = true;
            }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ShortcutPipe") && gameManagerScript.groundLevel != 2)
        {
            playerScript.pipelineToDesactive = collision.transform.parent.parent.gameObject;
            playerScript.canUseShortcut = true;
        }

        if (collision.gameObject.CompareTag("ShortcutPipeOutlet"))
        {
            playerScript.pipelineToDesactive = collision.transform.parent.gameObject;
        }

        if (collision.gameObject.CompareTag("DeathByFallActivator"))
        {
            if (playerScript.numberOfLifes > 1)
            {
                StartCoroutine(playerScript.DeathByFall());
            }
            else
            {
                gameManagerScript.isGameActive = false;
                playerScript.numberOfLifes--;
                playerScript.playerState = 1;
                StartCoroutine(playerScript.ChangeSprite());
                StartCoroutine(gameManagerScript.GameOver());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ShortcutPipe"))
        {
            playerScript.canUseShortcut = false;
        }
    }
}
