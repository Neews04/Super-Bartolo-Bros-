using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    GameManager gameManagerScript;

    float elevatorSpeed = 2.5f;
    int movementDirection = 1;
    public int elevatorType;
    public float leftLimit;
    public float rightLimit;

    private void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManagerScript.isGameActive)
        {
            if (elevatorType == 1)
            {
                Vector3 movement = new Vector3(0.0f, movementDirection * elevatorSpeed * Time.deltaTime, 0.0f);
                transform.position += movement;

                if (transform.position.y > 8.5f)
                {
                    transform.position = new Vector3(transform.position.x, -7.5f, transform.position.z);
                }
            }
            else if (elevatorType == 2)
            {
                Vector3 movement = new Vector3(0.0f, -movementDirection * elevatorSpeed * Time.deltaTime, 0.0f);
                transform.position += movement;

                if (transform.position.y < -7.5f)
                {
                    transform.position = new Vector3(transform.position.x, 8.5f, transform.position.z);
                }
            }
            else if (elevatorType == 3)
            {
                Vector3 movement = new Vector3(movementDirection * elevatorSpeed * Time.deltaTime, 0.0f, 0.0f);
                transform.position += movement;

                if (transform.position.x < leftLimit)
                {
                    transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
                    movementDirection *= -1;
                }

                if (transform.position.x > rightLimit)
                {
                    transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
                    movementDirection *= -1;
                }
            }
            else if (elevatorType == 4)
            {
                Vector3 movement = new Vector3(-movementDirection * elevatorSpeed * Time.deltaTime, 0.0f, 0.0f);
                transform.position += movement;

                if (transform.position.x < leftLimit)
                {
                    transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
                    movementDirection *= -1;
                }

                if (transform.position.x > rightLimit)
                {
                    transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
                    movementDirection *= -1;
                }
            }
        }
    }
}
