using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class EnemyAttack : MonoBehaviour
{
    // Variables de control de cantidad de daño, tiempo de ataque y saber si estamos siendo atacados
    public int attackDamage = 5;
    public float timeAttack = 1.0f;
    public bool playerHurt;

    GameObject player;
    Animator anim;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Referencias a Componentes/Game Objects
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }


    void OnTriggerEnter(Collider other)
    {
        if(player == other.gameObject)
        {
            playerHurt = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (player == other.gameObject)
        {
            playerHurt = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;
        if(playerHurt && timer >= timeAttack)
        {
            Profiler.BeginSample("EnemyAttack::Update::if>0");
            timer = 0.0f;
            if(player.GetComponent<PlayerHealth>().currentHealth > 0)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }
            Profiler.EndSample();
        }

        Profiler.BeginSample("EnemyAttack::Update::if<0");
        if(player.GetComponent<PlayerHealth>().currentHealth < 0)
        {
            anim.SetBool("Movimiento", true);
        }
        Profiler.EndSample();
    }
}
