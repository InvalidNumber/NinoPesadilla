using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Velocidad inicial del player [1,6]")]
    [Range(1.0f, 6.0f)]
    public float speed = 2.0f;//velocidad del player

    [Header("Health Settings")]
    [Tooltip("Vida incicial del player [1,10")]
    [Range(1, 10)]
    public short health = 5;

    //[Header("Player Settings")]
    //[SerializeField]
    //private GameObject player;//**
    /*[SerializeField] nos sirve para usar una variable privada desde el IDE de unity */

    

    //variables privadas no accesibles
    Vector3 playerMovement; //vector donde guardamos la direccion de movimiento
    Rigidbody playerRigidBody; // referencia al componente rigidbody
    Animator playerAnimator; //ref animator de player
    int floorMask; // mascara para hacer raycast solo con el suelo

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        floorMask = LayerMask.GetMask("Floor");// Numero int que define las mascaras del raycast GetMask("Floor", "Water");
    }


    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        //Raw para que no tenga "aceleracion"
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Movimiento
        Move(h, v);

        //Giros
        Turn();

        //Animaciones
        Animacion(h, v);
    }

    //las regiones te permiten "minimizar" una parte del script
    #region Movement
   void Move(float h, float v)
    {
        //Vector donde guardamos la direccion de movimiento actual
        playerMovement.Set(h, 0.0f, v);

        //Calculamos el desplazamiento normalizando el vector de direccion y multiplicandolo por la velocidad
        playerMovement = playerMovement.normalized * speed * Time.deltaTime;

        //Aplicamos el desplazamiento al rigibody
        playerRigidBody.MovePosition(transform.position + playerMovement);
    }

    void Turn()
    {
        // Creamos un rayo desde el punto donde he pulsado con el cursor del raton
        //en la pantalla en la direccion del raton
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if(Physics.Raycast(cameraRay, out floorHit, 1000.0f, floorMask))
        {
            Vector3 playerToHit = floorHit.point - transform.position;

            playerToHit.y = 0.0f;

            Quaternion rotation = Quaternion.LookRotation(playerToHit);

            playerRigidBody.MoveRotation(rotation);
        }
    }

    void Animacion(float h, float v)
    {
        if(h != 0 || v != 0)
        {
            playerAnimator.SetBool("Movimiento", true);
        }
        else
        {
            playerAnimator.SetBool("Movimiento", false);
        }

        //playerAnimator.SetBool("IsWalking", h != 0 || v != 0);

    }

    #endregion
}
