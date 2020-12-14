using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static int score;

    // Variables para el cambio automatico de nivel
    public const int CAMBIO_NIVEL = 200;
    public int controlCambioNivel;

    Text textoFinalScore;
    Text textPuntuacion;

    GameObject panelVictoria;

    private void Awake()
    {
        // LOS OBJECTOS QUE LLEVAN ESTE ATRIBUTO NO PUEDEN SER HIJOS
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            score = 0;
        }

        Debug.Log("ScoreManager::Start");
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    

    void Update()
    {
        //Debug.Log("Score: " + score);
        /*
        Debug.Log("ScoreManager::Update::texto " + texto.text);
        Debug.Log("ScoreManager::Update::texto " + score);
        */

        textPuntuacion.text = score.ToString();
        
        
        if(score >= controlCambioNivel)
        {
            // Cambio de nivel automatico
            //PasarNivel();
            
            panelVictoria.SetActive(true);
            controlCambioNivel += CAMBIO_NIVEL;
            SetFinalScore();
        }
        
    }

    /*
    void PasarNivel()
    {
        controlCambioNivel += CAMBIO_NIVEL;
        FindObjectOfType<GameManager>().LoadNextLevel();
    }
    */
    public void SetFinalScore()
    {
        textoFinalScore.text = score.ToString();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ScoreManager::OnSceneLoaded");
        if(GameObject.FindGameObjectWithTag("Score") != null)
        {
           
            GameObject.FindGameObjectWithTag("Score").gameObject.GetComponent<Text>().text = score.ToString();
            textPuntuacion = GameObject.FindGameObjectWithTag("Score").gameObject.GetComponent<Text>();
            
        }
        else
        {
            //Si es nulo no existe el game object con tag score
            //dos opciones, lo creo o doy un aviso de error
        }

        if (GameObject.FindGameObjectWithTag("Victoria") != null)
        {
            panelVictoria = GameObject.FindGameObjectWithTag("Victoria");
            GameObject.FindGameObjectWithTag("PuntuacionFinal").gameObject.GetComponent<Text>().text = textPuntuacion.ToString();
            textoFinalScore = GameObject.FindGameObjectWithTag("PuntuacionFinal").gameObject.GetComponent<Text>();
            panelVictoria.SetActive(false);
        }

    }
}
