using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameManager gameManagerScript;
    protected AudioManager audioManagerScript;
    Player playerScript;

    public float leftLimit;
    public float rightLimit;
    public bool useLimits;

    public bool isEnemyActive = false;
    float moveSpeed = 2.5f;
    int movementDirection = -1;
    protected int enemyScoreValue;

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        playerScript = GameObject.Find("Bartolo").GetComponent<Player>();
    }

    void Update()
    {
        if (gameManagerScript.isGameActive && isEnemyActive && gameManagerScript.groundLevel != 2)
        {
            Vector3 movement = new Vector3(movementDirection * moveSpeed * Time.deltaTime, 0.0f, 0.0f);
            transform.position += movement;

            if (transform.position.x < leftLimit && useLimits)
            {
                movementDirection *= -1;
            }
            else if (transform.position.x > rightLimit && useLimits)
            {
                movementDirection *= -1;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pipes") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Object") 
            || collision.gameObject.CompareTag("Stairs"))
        {
            movementDirection = -movementDirection;
        }
        if (collision.gameObject.CompareTag("Fireball"))
        {
            audioManagerScript.PlaySoundEffect(audioManagerScript.crushEnemySound);
            gameManagerScript.UpdateScore(enemyScoreValue);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}