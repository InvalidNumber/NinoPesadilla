using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//RequireComponent hace que si un objeto no tiene cierto componente cuandos e inicia se lo mete
//en este caso le metera un NavMeshAgent si no lo tiene
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public float attackRange = 1.0f;//distancia de ataque

    //ponemos una variable donde ponemos el nombre de la aniacion para hacer el script mas reutilizable
    public string animacionNombre;

    Animator enemyAnimator;
    Transform player;
    NavMeshAgent nav;


    // Start is called before the first frame update
    void Start()
    {
        //Buscamos el gameobject con el Tag Player y obtenemos su posicion
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Obtenemos el NavMeshAgent del gameobject al que el script esta asociado
        nav = this.GetComponent<NavMeshAgent>();

        //obtenemos el animator controller
        enemyAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        /*
        Debug.Log("EnemyMovement::Update:: distancia " + Vector3.Distance(transform.position, player.transform.position));
        Debug.Log("EnemyMovement::Update:: attackRange " + attackRange);
        Debug.Log("EnemyMovement::Update:: Player currentHealth " + player.GetComponent<PlayerHealth>().currentHealth);
        Debug.Log("EnemyMovement::Update:: Enemy currentHealth " + GetComponent<EnemyHealth>().currentHealth);
        */

        //si la distancia al player es menor que attackrange para de moverse
        if ((Vector3.Distance(transform.position, player.transform.position) < attackRange) && 
                (player.GetComponent<PlayerHealth>().currentHealth > 0) && (GetComponent<EnemyHealth>().currentHealth > 0))
        {
            //le ponemos de destino la posicion del jugador
           
            nav.SetDestination(player.transform.position);
            //si se mueve hace la animacion de correr
            enemyAnimator.SetBool(animacionNombre, true);
        }
        else
        {
            enemyAnimator.SetBool(animacionNombre, false);
        }
       
    }

    
}
