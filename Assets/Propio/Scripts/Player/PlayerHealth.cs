using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    // Variables para controlar la vida del player
    public int startingHealth = 100;
    public int currentHealth;

    // Variables para controlar el feedback visual en la vida
    public Slider sliderHealth;
    public Image heart;
    public Image imageDamage;
    public AudioClip deathClip;

    // Variables para referencias de comportamiento del gameobject
    Animator anim;
    AudioSource audioSource;
    PlayerMovement playerMovement;
    bool isDead;
    bool isHurt;

    //Activar panel gameover si morimos
    public GameObject panelGameOver;

    void Start()
    {
        // Referencias las variables de componentes
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        ResetPlayer();
    }

    void Update()
    {
        if(isHurt)
        {
            imageDamage.color = new Color(1.0f, 0.0f, 0.2f);
        }
        else
        {
            imageDamage.color = Color.Lerp(imageDamage.color, Color.clear, 10.0f * Time.deltaTime);
        }

        isHurt = false;
    }
    public void ResetPlayer()
    {
        // Vida inicial
        currentHealth = startingHealth;

        // Activar el movimiento
        playerMovement.enabled = true;

        // Variable de control de muerte a false
        isDead = false;
    }

    public void TakeDamage(int cantidad)
    {
        // Daño -> varible de control a true;
        isHurt = true;

        // Restar a  vida
        currentHealth = currentHealth - cantidad;

        // Modificamos el slider
        sliderHealth.value = currentHealth;

        //Play audio daño
        audioSource.Play();

        // Cambiamos el icono de corazon cuando a vida es menor de 20
        if(currentHealth <= 20)
        {
            heart.sprite = Resources.Load<Sprite>("Heart_Negro");
        }
        else if((20 < currentHealth) && (currentHealth < 80))
        {
            heart.sprite = Resources.Load<Sprite>("Heart");
        }
        else 
        {
            heart.sprite = Resources.Load<Sprite>("Heart");
        }
          
        // Comprobar si el player muere
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        // Variable de control a true
        isDead = true;

        // No movernos
        playerMovement.enabled = false;

        audioSource.clip = deathClip;
        audioSource.Play();

        // Activar la animacion de muerte
        anim.SetTrigger("Muerto");

        // Activar el boton de reintentar y el texto de muerte
        panelGameOver.SetActive(true);

        // desactivamos el ataque del player
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // Llmar a funcion para escribir puntuacion final en pantalla
        FindObjectOfType<ScoreManager>().SetFinalScore();

        //Llamar a funcion para guardar en el ranking la partida
        string playerName = "Player_" + Random.Range(0, 10).ToString();

        FindObjectOfType<SaveLoadManager>().SaveRanking(playerName, ScoreManager.score);
    }
}
