using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    GameObject player;
    Player playerScript;
    GameManager gameManagerScript;
    Vector3 cameraDistance = new Vector3(1.5f, 4.5f, -10.0f);
    float leftLimit = 0.0f, rightLimit = 202.0f;
    float upperLimit = 0.5f, lowerLimit = 0.5f;

    void Start()
    {
        player = GameObject.Find("Bartolo");
        playerScript = GameObject.Find("Bartolo").GetComponent<Player>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void LateUpdate()
    {
        if (gameManagerScript.updateLimits)
        {
            leftLimit = gameManagerScript.leftLimit;
            rightLimit = gameManagerScript.rightLimit;
            lowerLimit = gameManagerScript.lowerLimit;
            upperLimit = gameManagerScript.upperLimit;
            gameManagerScript.updateLimits = false;
        }

        if (gameManagerScript.groundLevel == 2)
        {
            transform.position = gameManagerScript.cameraUndergroundCoordenates;
        }

        if (gameManagerScript.isGameActive && gameManagerScript.groundLevel != 2)
        {
            Vector3 newPosition = player.transform.position + cameraDistance;
            if( newPosition.x >= transform.position.x)
            {
                leftLimit = newPosition.x;
            }
            newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);
            newPosition.y = Mathf.Clamp(newPosition.y, lowerLimit, upperLimit);
            transform.position = newPosition;
        }
        // PARA ESTABLECER LA CAMARA EN POSICION TRAS SALIR DE UNA TUBERIA
        if (playerScript.canFollowPlayer)
        {
            Vector3 newPosition = player.transform.position + cameraDistance;
            if (newPosition.x >= transform.position.x)
            {
                leftLimit = newPosition.x;
            }
            newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);
            newPosition.y = Mathf.Clamp(newPosition.y, lowerLimit, upperLimit);
            transform.position = newPosition;
            playerScript.canFollowPlayer = false;
        }
    }
}