using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    Player playerScript;
    HeadCollider headColliderScript;
    public Sprite spriteToChange;
    
    public float moveDistance = 0.5f;
    public float moveSpeed = 10f;
    public int blockType = 0;
    public int coins = 8;
    public bool isBump;
    Vector3 initialPosition;

    void Start()
    {
        playerScript = GameObject.Find("Bartolo").GetComponent<Player>();
        headColliderScript = GameObject.Find("PlayerHead").GetComponent<HeadCollider>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (coins <= 0)
        {
            SpriteRenderer objectSprite = GetComponent<SpriteRenderer>();
            objectSprite.sprite = spriteToChange;
            isBump = true;
        }
    }

    public IEnumerator MoveBlock()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.225f) 
        {
            float newY = initialPosition.y + Mathf.Sin(elapsedTime * moveSpeed) * moveDistance;
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
    }
}
