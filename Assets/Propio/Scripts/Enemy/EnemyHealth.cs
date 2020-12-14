using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{

    // Variable para controlar la vida
    public int startingHealth = 100;
    public int currentHealth;
    bool isDead;

    // Variable publica puntuacion al matar al enemigo
    public int puntuacion;

    // Variables para recoger el clip de audio cuando el enemigo muere
    public AudioClip deathClip;
    AudioSource audioSource;

    // Variables para referencias del componente del gameobject
    Animator anim;

    // Referecia al nasvegador para cambiar su velocidad despues
    NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = this.GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            
            // Tambien StartCoroutine("DestroyEnemy");
            StartCoroutine(DestroyEnemy());
        }
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int cantidad)
    {
        if(!isDead)
        {
            audioSource.Play();
            currentHealth = currentHealth - cantidad;

            if (currentHealth <= 0)
            {
                audioSource.clip = deathClip;
                audioSource.Play();

                nav.speed = 0;
                anim.SetTrigger("Muerto");

                isDead = true;

                ScoreManager.score += puntuacion;

                // Le desactivamos la esfera para que no pueda hacernos daño
                GetComponent<SphereCollider>().enabled = false;
            }
        }
        
    }
}
