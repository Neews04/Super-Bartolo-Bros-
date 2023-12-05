using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Almeja : Enemy
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFeet"))
        {
            enemyScoreValue = 10;
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
