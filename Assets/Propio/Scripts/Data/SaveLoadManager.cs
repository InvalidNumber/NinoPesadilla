using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    bool loadGame = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // Si pulsamos load game la variable load game estara a true
        if(loadGame)
        {
            loadGame = false;
            // Cargar datos guardados
            PlayerData playerData = SaveSystem.LoadPlayer();
            if(playerData != null)
            {
                // Obtenemos la referencia del player
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // Cogemos el valor de vida guardado y lo escribimos en las variables relacionadas con la vida
                player.GetComponent<PlayerHealth>().currentHealth = playerData.health;
                player.GetComponent<PlayerHealth>().startingHealth = playerData.health;
                GameObject.FindGameObjectWithTag("SliderHealth").GetComponent<Slider>().value = playerData.health;

                // Coger el valor de la posicion y la rotacion y se lo ponemos al Player
                player.gameObject.transform.position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
                player.gameObject.transform.rotation = Quaternion.Euler(playerData.rotation[0], playerData.rotation[1], playerData.rotation[2]);

                // Cargar valor de puntuacion
                ScoreManager.score = playerData.score;
                GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = playerData.score.ToString();

                // Cargar los valores de la camara
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

                camera.gameObject.transform.position = new Vector3(playerData.positionCamera[0],
                    playerData.positionCamera[1], playerData.positionCamera[2]);

                camera.gameObject.transform.rotation.Set(playerData.rotationCamera[0],
                    playerData.rotationCamera[1], playerData.rotationCamera[2], playerData.rotationCamera[3]);
            }
            else
            {
                Debug.Log("SaveLoadManager;;OnSceneLoaded::playerData NULL");
            }

            // Cargar 
            EnemiesData enemiesData = SaveSystem.LoadEnemies();

            if(enemiesData != null)
            {
                int numeroEnemigos = enemiesData.tipoEnemigo.Length;

                for(int i = 0; i < numeroEnemigos; i++)
                {
                    // Obtener una referencia a GameObject que quier instanciar, se encuentra en enemy manager
                    GameObject[] enemiesFromEnemyManager = GameObject.FindObjectOfType<EnemyManager>().enemies;
                    
                    for(int j = 0; j < enemiesFromEnemyManager.Length; j++)
                    {
                        if (enemiesData.tipoEnemigo[i].Contains(enemiesFromEnemyManager[j].name))
                        {
                            Instantiate(
                                enemiesFromEnemyManager[j],
                                new Vector3(enemiesData.position[i][0], enemiesData.position[i][1], enemiesData.position[i][2]),
                                Quaternion.Euler(enemiesData.rotation[i][0],
                                enemiesData.rotation[i][1],
                                enemiesData.rotation[i][2])
                            );
                        }
                    }
                }
            }
            else
            {
                Debug.Log("SaveLoadManager::OnSceneLoaded::enemiesData Null", this.gameObject);
            }
            
        }
    }

    public void SaveGame()
    {
        // Guardar Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");


        // Guardar Camara
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        // Guardar la puntuacion
        int score = ScoreManager.score;

        SaveSystem.SavePlayer(player, score, camera);

        // Guardar la informacion de los enemigos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        SaveSystem.SaveEnemies(enemies);
    }

    public void LoadGame()
    {
        loadGame = true;
        GameObject.FindObjectOfType<GameManager>().LoadNextLevel();

    }

    public void LoadRanking()
    {
        RankingData rankingData = SaveSystem.LoadRanking();

        if(rankingData != null)
        {
            for (int i = 0; i < rankingData.sizeRankingData; i++)
            {
                Debug.Log("Name: " + rankingData.nameRankingData[i] +
                    " -Score: " + rankingData.scoreRankingData[i]
                     + " -Date: " + rankingData.dateRankingData[i]);
            }
        }
        else
        {
            Debug.LogError("SaveLoadManager::LoadRanking::NULL ");
        }
    }

    public void SaveRanking(string namePlayer, int scoreGame)
    {
        // Recupero el ultimo ranking guardado
        RankingData rankingData = SaveSystem.LoadRanking();

        // Fecha actual Unix Epoch
        int date = System.DateTime.Now.Millisecond;
        if (rankingData != null)
        {
            // Creo unas estructuras auxiliares para guardar el ranking
            string[] nameAux = new string[rankingData.sizeRankingData + 1];
            int[] scoreAux = new int[rankingData.sizeRankingData + 1];
            int[] dateAux = new int[rankingData.sizeRankingData + 1];

            // Guardo el ranking actual en las estructuras auxiliares
            for (int i = 0; i < rankingData.sizeRankingData; i++)
            {
                nameAux[i] = rankingData.nameRankingData[i];
                scoreAux[i] = rankingData.scoreRankingData[i];
                dateAux[i] = rankingData.dateRankingData[i];
            }

            // Añado mi nueva puntuacion
            nameAux[rankingData.sizeRankingData] = namePlayer;
            scoreAux[rankingData.sizeRankingData] = scoreGame;
            dateAux[rankingData.sizeRankingData] = date;

            // Guardar el ranking ampliado
            SaveSystem.SaveRanking(nameAux, scoreAux, dateAux, rankingData.sizeRankingData + 1);
        }
        else
        {
            // Creo unas estructuras auxiliares para guardar el ranking
            string[] nameAux = new string[1];
            int[] scoreAux = new int[1];
            int[] dateAux = new int[1];

            nameAux[0] = namePlayer;
            scoreAux[0] = scoreGame;
            dateAux[0] = date;

            SaveSystem.SaveRanking(nameAux, scoreAux, dateAux, 1);
        }
        

        
    }
}
