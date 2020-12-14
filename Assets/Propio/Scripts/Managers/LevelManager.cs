using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject pantallaLoading;

    public GameObject environment;
    public GameObject player;
    public GameObject enemyManager;
    public GameObject ambient;

    // Hemos convertido el start directamente en una corrutina
    IEnumerator Start()
    {
        pantallaLoading.SetActive(true);
        yield return new WaitForSeconds(3);
        pantallaLoading.SetActive(false);
        environment.SetActive(true);
        player.SetActive(true);
        enemyManager.SetActive(true);
        ambient.SetActive(true);
    }
}
