using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Esto es nuevo
using FishNet.Broadcast;
using FishNet;
using FishNet.Connection;
public class CubePositionBroadcast : MonoBehaviour //En este caso no convertimos el script a NetworkBehaviour ya que
    //en realidad no estamos interactuando con a traves de la red
{
    public List<Transform> cubePositions = new List<Transform>(); //hacemos una lista de las posiciones
    public int transformIndex; //y creamos un indice (manito arriba)

    private void OnEnable()
    {
        //Registramos nuestra funcion para que ahora todos los clientes la reciban (Le mandamos la esctructura y la funcion donde asignamos el dato de la esctructura con nuestro dato)
        InstanceFinder.ClientManager.RegisterBroadcast<PositionIndex>(OnPositionBroadcast);
        //Registramos nuestra funcion para que ahora el servidor reciba esta funcion (o eso entiendo yo D:) 
        InstanceFinder.ServerManager.RegisterBroadcast<PositionIndex>(OnClientPositionBroadcast);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<PositionIndex>(OnPositionBroadcast);
        InstanceFinder.ServerManager.UnregisterBroadcast<PositionIndex>(OnClientPositionBroadcast);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            int nextIndex = transformIndex + 1;
            if (nextIndex >= cubePositions.Count)
                nextIndex = 0;
            //Si nosotros somos el servidor ejectuamos un tipo de transmision
            if (InstanceFinder.IsServer)
            {
                //Aca instanciamos el broadcast y le decimos que el tIndex de nuestra esctructura (PositionIndex) va a ser igual al valor que tenga tIndex
                //En este caso solo lo puede hacer el host (o servidor)
                InstanceFinder.ServerManager.Broadcast(new PositionIndex() { tIndex = nextIndex });
            }
            //Si somos el cliente iniciamos otro tipo de transmision
            else if (InstanceFinder.IsClient)
            {
                //Aca instanciamos el broadcast y le decimos que el tIndex de nuestra esctructura (PositionIndex) va a ser igual al valor que tenga tIndex
                //Solo que ahora tambien lo pueden hacer los clientes
                InstanceFinder.ClientManager.Broadcast(new PositionIndex() { tIndex = nextIndex });
            }
        }

        //esto quiere decir que cada fotograma ahora es la posicion del index (esto no es optimo pero se puede mejorar)
        transform.position = cubePositions[transformIndex].position;
    }

    //Aqui lo unico que hacemos es traer la esctructura de datos con el nombre "indexStruct"
    //y hacemos que nuestro indice anterior sea igual al tIndex de nuestra esctructura usando "indexStruct.tIndex"
    private void OnPositionBroadcast(PositionIndex indexStruct)
    {
        transformIndex = indexStruct.tIndex;
    }

    //en esta funcion solo usamos el indexStruct, el networkconecction es para saber que cliente te envio esto
    private void OnClientPositionBroadcast(NetworkConnection networkConnection, PositionIndex indexStruct)
    {
        InstanceFinder.ServerManager.Broadcast(indexStruct);
    }

    //Creamos una esctura de datos (me saltee esa clase en la secundaria ctmrr)
    public struct PositionIndex : IBroadcast
    {
        public int tIndex;
    }   
}

