using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    GameManager gameManagerScript;

    int movementDirection = 1;
    float moveSpeed = 6.0f;

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (gameManagerScript.isGameActive)
        {
            Vector3 movement = new Vector3(movementDirection * moveSpeed * Time.deltaTime, 0.0f, 0.0f);
            transform.position += movement;
        }
    }
}
