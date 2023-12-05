using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    GameManager gameManagerScript;

    public Image onePlayerMode;
    public Image twoPlayerMode;
    int playMode = 1;

    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            playMode = 1;
            twoPlayerMode.gameObject.SetActive(false);
            onePlayerMode.gameObject.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            playMode = 2;
            onePlayerMode.gameObject.SetActive(false);
            twoPlayerMode.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (playMode == 1)
            {
                gameManagerScript.StartGame();
            }
            else
            {

            }
        }
    }
}
