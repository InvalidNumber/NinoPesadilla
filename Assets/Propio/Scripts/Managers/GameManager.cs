using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    string nextLevel;

    // Variable para guardar el nombre de la primera escena
    //public string startScene;

    // Array de string donde guardar los nombres de las escenas
    public string[] scenes;

    // Variable para saber en qué escena/nivel me encuentro
    static int escenaActual;

    // Referencia al panel del canvas utilizado para hacer el fade
    Image fadeImagePanel;
    GameObject fadeGameObjectPanel;

    // Booleano para activa el fade de transparente a negro
    bool toBlack;

    // Bool para controlar el fadeIn de entrada de escena
    bool primeraVez;

    // Referencias a GameObjects/Componentes cuyas opciones/párámetros puedes ser configurados por el usuario
    // p.ej: Activación/Desactivación de la musica.
    public AudioMixerGroup ambient;
    public AudioMixerGroup[] sfxs; // Esto es un array que nos permite desactivar todo lo que tengamos dentro al pulsar el boton sfx
    public AudioMixer audioMixer;

    // Variable para tener acceso al componente toggle de la musica y efectos
    Toggle toggleMusic;
    Toggle toggleSFX;
    #endregion

    #region Functions

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /* Int para controlar la musica ---------------------------
    int musicaN;
    bool musicaB;
    */

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<SaveLoadManager>().LoadRanking();

        fadeImagePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();
        fadeGameObjectPanel = GameObject.FindGameObjectWithTag("FadePanel");

        escenaActual = SceneManager.GetActiveScene().buildIndex;
        primeraVez = true;

        // Conseguir los componentes Toggle de musica y SFX del canvas
        toggleMusic = GameObject.FindGameObjectWithTag("Toggle-Musica").GetComponent<Toggle>();
        toggleSFX = GameObject.FindGameObjectWithTag("Toggle-SFX").GetComponent<Toggle>();

        // Añadir listener a cada toggle para que nos avise cuando haya un cambio d evalor
        toggleMusic.onValueChanged.AddListener(delegate { ToggleMusicChanged(toggleMusic); }); // .RemoveListener(); sirve para quitarlos
        toggleSFX.onValueChanged.AddListener(delegate { ToggleSFXChanged(toggleSFX); });

        LoadPrefs();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //* Codigo necesario para que la primera vez que entre en una escena se haga el fade
        // Refencias a las variables relacionadas con el panel fade
        fadeImagePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();
        fadeGameObjectPanel = GameObject.FindGameObjectWithTag("FadePanel");
        // Avtivar el fade
        StartCoroutine(FadeInOut(false, 3));
    }

    // Update is called once per frame
    void Update()
    {

        if (primeraVez)
        {
            StartCoroutine(FadeInOut(false, 3));
            primeraVez = false;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }

        // boton de pausa
        if(Input.GetKey(KeyCode.P))
        {

        }

        // Guardar partida
        if(Input.GetKey(KeyCode.G))
        {
            GameObject.FindObjectOfType<SaveLoadManager>().SaveGame();
        }

        if (toBlack)
        {

            StartCoroutine(FadeInOut(true, 3));
            toBlack = false;
        }

    }

    /// <summary>
    /// QuitGame: función para salir del juego
    /// </summary>
    public void QuitGame()
    {
        SavePrefs();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadNextLevel()
    {
        toBlack = true;
        StartCoroutine(esperaFadeLoadNext());

    }

    public void LoadStart()
    {
        toBlack = true;
        StartCoroutine(esperaFadeStart());

    }

    public void ReloadScene()
    {

        SceneManager.LoadScene(scenes[escenaActual]);
    }

    IEnumerator esperaFadeLoadNext()
    {
        yield return new WaitForSeconds(4);
        escenaActual += 1;
        if (escenaActual >= scenes.Length)
        {
            escenaActual = 0;
        }
        SceneManager.LoadScene(scenes[escenaActual]);
    }

    IEnumerator esperaFadeStart()
    {
        Debug.Log("esperaFadeStart: ");
        yield return new WaitForSeconds(4);
        Debug.Log("esperaFadeStart::LoadScene ");
        SceneManager.LoadScene(scenes[0]);
        escenaActual = 0;
    }


    public IEnumerator FadeInOut(bool fadeToBlack, int fadeSpeed)
    {
        yield return new WaitForSeconds(1f);
        Color imageColor = fadeImagePanel.color;

        float cantidadAlpha;

        if (fadeToBlack)
        {
            fadeGameObjectPanel.SetActive(true);
            while (fadeImagePanel.color.a < 1)
            {
                cantidadAlpha = imageColor.a + (fadeSpeed * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, cantidadAlpha);
                fadeImagePanel.color = new Color(imageColor.r, imageColor.g, imageColor.b, cantidadAlpha);
                yield return null;
            }

        }
        else
        {
            while (fadeImagePanel.color.a > 0)
            {

                cantidadAlpha = imageColor.a - (fadeSpeed * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, cantidadAlpha);
                fadeImagePanel.color = imageColor;
                yield return null;
            }
            fadeGameObjectPanel.SetActive(false);



        }
    }

    ///<summary>
    ///Guardar preferencias de usuario
    ///</summary>
    public void SavePrefs()
    {
        
        // Almacenar funcion musica
                                                                                        // PlayerPrefs.GetInt("musica");
        GameObject toggleMusica = GameObject.FindGameObjectWithTag("Toggle-Musica");
        bool activo = toggleMusica.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("Toggle-Musica", activo ? 1 : 0);

        GameObject toggleSFX = GameObject.FindGameObjectWithTag("Toggle-SFX");
        bool activoSFX = toggleSFX.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("Toggle-SFX", activoSFX ? 1 : 0);
        //Debug.Log("GameManager::SavePrefs::Toggle-IsOn: " + activo);

        // Guardar los player prefs
        PlayerPrefs.Save();
    }

    void LoadPrefs()
    {
        
        // Cargar valor preferencia Musica
        if(PlayerPrefs.HasKey("Toggle-Musica"))
        {
            int MusicaValue = PlayerPrefs.GetInt("Toggle-Musica");
            bool activo = MusicaValue == 1 ? true : false; // le decimos que si musica value es 1 sea verdadero y sino falso
            // Acceder a la propiedad isOn del componente Toggle del GameObject con tag "Toggle-Musica"
            GameObject.FindGameObjectWithTag("Toggle-Musica").GetComponent<Toggle>().isOn = activo;

            // Cuando cargo el valor de isOn del componente Toggle del GameObject "Toggle-Musica", llamo a la función que pone los 
            // valores de atenuación (volumen) del grupo (canal) Ambient en el AudioMixer.
            // Si se guardó la preferencia de Música On, se pone el volumen a un valor que suene.
            // Si se guardó la preferencia de Música Off, se pone el volumen al valor mínimo posible para que no suene.
            ToggleMusicChanged(toggleMusic);
            //Debug.Log("GameManager::LoadPrefs::Toggle-Musica" + activo);
        }
        else
        {
            Debug.Log("GameManager::LoadPrefs::Toggle-Musica No se encuentra en los player prefs");
        }

        // Cargar valor preferencia SFX
        if (PlayerPrefs.HasKey("Toggle-SFX"))
        {
            int SFXValue = PlayerPrefs.GetInt("Toggle-SFX");
            bool activoSFX = SFXValue == 1 ? true : false; // le decimos que si musica value es 1 sea verdadero y sino falso
            // Acceder a la propiedad isOn del componente Toggle del GameObject con tag "Toggle-Musica"
            GameObject.FindGameObjectWithTag("Toggle-SFX").GetComponent<Toggle>().isOn = activoSFX;

            // Cuando cargo el valor de isOn del componente Toggle del GameObject "Toggle-SFX", llamo a la función que pone los 
            // valores de atenuación (volumen) del grupo (canal) Player,Enemie y SFX en el AudioMixer.
            // Si se guardó la preferencia de SFX On, se pone el volumen a un valor que suene.
            // Si se guardó la preferencia de SFX Off, se pone el volumen al valor mínimo posible para que no suene.
            ToggleSFXChanged(toggleSFX);
            //Debug.Log("GameManager::LoadPrefs::Toggle-SFX" + activoSFX);
        }
        else
        {
            Debug.Log("GameManager::LoadPrefs::Toggle-SFX No se encuentra en los player prefs");
        }
    }

    // Funcion que actua como delegado del callback OnValueChange del Toggle-Musica
    //Permite ejecutar el codigo que hay dentro del ToggleMusicChanged cuando se produce un cambio en el music Toggle
    void ToggleMusicChanged(Toggle change)
    {
        Debug.Log("GameManager::ToggleMusicChanged:: change.isOn " + change.isOn); //-----
        if(change.isOn)
        {
            ambient.audioMixer.SetFloat("VolumeAmbient", -11.0f);
        }
        else
        {
            ambient.audioMixer.SetFloat("VolumeAmbient", -80.0f);
        }
    }

    // Funcion que actua como delegado del callback OnValueChange del Toggle-SFX
    //Permite ejecutar el codigo que hay dentro del ToggleMusicChanged cuando se produce un cambio en el SFX Toggle
    void ToggleSFXChanged(Toggle change)
    {
        Debug.Log("GameManager::ToggleSFXChanged::change.isOn: " + change.isOn);
        if (change.isOn)
        {
            audioMixer.SetFloat("VolumePlayer", -5.0f);
            audioMixer.SetFloat("VolumeSFX", -0.0f);
            
        }
        else
        {
            for (int i = 0; i < sfxs.Length; i++)
            {
                if (sfxs[i].name == "Player")
                {
                    sfxs[i].audioMixer.SetFloat("VolumePlayer", -80.0f);
                }
                else if (sfxs[i].name == "SFX")
                {
                    sfxs[i].audioMixer.SetFloat("VolumeSFX", -80.0f);
                }
            }
        }
    }

    #endregion

    /* version antiguoa sin for
    void ToggleSFXChanged(Toggle change)
    {
        if (change.isOn)
        {
            sfxs[0].audioMixer.SetFloat("VolumePlayer", -5.0f);
            sfxs[1].audioMixer.SetFloat("VolumeSFX", -5.0f);
        }
        else
        {
            sfxs[0].audioMixer.SetFloat("VolumePlayer", -80.0f);
            sfxs[1].audioMixer.SetFloat("VolumeSFX", -80.0f);
        }
    }
    */

    /* ---------------------
    bool ActivarMusica(int musica, bool activar)
    {
        if (musica != 0)
            return activar = true;
        else
            return activar = false;
    }
    */
}
