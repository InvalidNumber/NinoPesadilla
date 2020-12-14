using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void Salir()
    {
        FindObjectOfType<GameManager>().QuitGame();
    }

    public void Restart()
    {
        FindObjectOfType<GameManager>().LoadStart();
    }

    public void NextLevel()
    {
        FindObjectOfType<GameManager>().LoadNextLevel();
    }
}
