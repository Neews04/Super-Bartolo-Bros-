using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactoomba : Enemy
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFeet"))
        {
            enemyScoreValue = 50;
            gameManagerScript.UpdateScore(enemyScoreValue);
            audioManagerScript.PlaySoundEffect(audioManagerScript.crushEnemySound);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("EnemyActivator"))
        {
            isEnemyActive = true;
        }
    }
}
