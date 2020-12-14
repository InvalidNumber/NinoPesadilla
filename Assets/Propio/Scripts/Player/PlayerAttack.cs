using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Variables para control del daño
    public int disparoDano = 20;// Daño del disparo
    public float tiempoEntreDisparos = 0.1f;
    public float rango = 25.0f;
    public float vidaDisparo = 0.1f;

    float timer;
    // Variables para el dsparo
    Ray disparoRay = new Ray();
    RaycastHit disparoHit;
    int disparoMask;
    public ParticleSystem gunParticles;
    public AudioSource audioSourceDisparo;
    public Transform puntoDesdeDondeDisparamos;
    public LineRenderer disparo;


    // Start is called before the first frame update
    void Start()
    {
        disparoMask = LayerMask.GetMask("Shootable");

        // Timer para controlar el tiempo entre disparos
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizamos el valor del timer
        timer = timer + Time.deltaTime;

        if(timer > tiempoEntreDisparos)
        {
            if(Input.GetButton("Fire1"))
            {
                Disparar();
            }
        }

        if (timer > tiempoEntreDisparos *vidaDisparo)
        {
            disparo.enabled = false;
        }
    }

    void Disparar()
    {
        // Inicializar el timer que controla el tiempo entre disparos
        timer = 0.0f;

        // Audio de disparo
        audioSourceDisparo.Play();

        // Activar particulas de efecto disparo
        gunParticles.Stop();
        gunParticles.Play();

        // Inicializar disparo (lineRenderer)
        disparo.enabled = true;
        disparo.SetPosition(0, disparo.transform.position);

        // Inicializar valores del Ray
        disparoRay.origin = puntoDesdeDondeDisparamos.position;
        disparoRay.direction = puntoDesdeDondeDisparamos.forward;

        if(Physics.Raycast(disparoRay, out disparoHit, rango, disparoMask))
        {
            if(disparoHit.collider.GetComponent<EnemyHealth>())
            {
                disparoHit.collider.GetComponent<EnemyHealth>().TakeDamage(disparoDano);
            }

            disparo.SetPosition(1, disparoHit.point);
        }
        else
        {
            disparo.SetPosition(1, disparoRay.origin + disparoRay.direction * rango);
        }
    }
}
