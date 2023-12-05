using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour
{
    GameManager gameManagerScript;

    GameObject blockToActive;
    Rigidbody2D rb2D;
    Collider2D objectCollider2D;

    float moveSpeed = 3.5f;
    int movementDirection = 1;
    bool objectRiseIsFinished = false;
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();
        objectCollider2D = GetComponent<Collider2D>();
        StartCoroutine(ObjectRising());
    }

    void Update()
    {
        if (objectRiseIsFinished && gameManagerScript.isGameActive && gameObject.CompareTag("Sun") || gameObject.CompareTag("Peyote"))
        {
            Vector3 movement = new Vector3(movementDirection * moveSpeed * Time.deltaTime, 0.0f, 0.0f);
            transform.position += movement;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pipes") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Object"))
        {
            movementDirection = -movementDirection;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Random Block"))
        {
            blockToActive = other.gameObject;
        }
    }

    IEnumerator ObjectRising()
    {
        rb2D.isKinematic = true;
        objectCollider2D.enabled = false;
        float duration = 1.0f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 1f, startPosition.z);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectCollider2D.enabled = true;
        rb2D.isKinematic = false;
        objectRiseIsFinished = true;
    }
}
