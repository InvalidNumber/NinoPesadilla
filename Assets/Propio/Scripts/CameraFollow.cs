using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;//Posición del elemento que la camara tiene que seguir
    Vector3 offset;//Vector que une el target con la camara
    short smoothing = 2;// velocidad con la que la camara sigue al target
    public bool interpolacion;//Booleano para determinar cuando utilizar la interpolacion en el calculo de la nueva posisicion de la camara
    

    private void Start()
    {
        //Vector distancia inicial entre target y camara
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        //Actualizamos la posicion de la camara = posicion actual del target + offset calculado inicialmente
        Vector3 campos = target.position + offset;

        //con este if solo se tiene en cuenta estas lineas en el editor
        /*
#if UNITY_EDITOR
        Debug.Log("CameraFllow::Start::offset" + offset);
#endif
        */
        if (interpolacion)
        {
            //Se asigna la nueva posicion de la camara atraves de una interpolacion lineal
            transform.position = Vector3.Lerp(transform.position, campos, smoothing * Time.deltaTime);
        }

        //Se asigna la nueva posicion de la camara de forma directa
        transform.position = campos;
        
    }


}
