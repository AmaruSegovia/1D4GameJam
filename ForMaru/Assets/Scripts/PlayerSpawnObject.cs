using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public class PlayerSpawnObject : NetworkBehaviour
{
    //Referencia del objeto que vamos a crear
    public GameObject objToSpawn;
    //Objeto de juego publico, podemos HideInInspector para que no nos salga en el inspector porque no hace faltas
    [HideInInspector] public GameObject spawnedObject;

    // Cuando el servidor hace esto:
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<PlayerSpawnObject>().enabled = false;
    }

    
    private void Update()
    {
        //Si este objeto es nulo (Es decir no generamos el objeto aun) y presionamos 1 spawmnea el objeto
        if (spawnedObject == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnObject(objToSpawn, transform, this);
        }
        //Si el objeto ya ha sido spawneado (osea no es nulo) al presionar 2 se elimina
        if (spawnedObject != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            DespawnObject(spawnedObject);
        }
    }

    //Generamos el objeto a traves el modo servidor RPC
    [ServerRpc]
    //Mandamos al servidor, el objeto, la ubicacion del jugador y para poder configurar esto a traves de la red tambien le mandamos el script
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        // Hacemos una instancia del objeto, para instanciar un objeto usamos
        // (un Game Object, la posicion del tipo Vector3, la rotacion del tipo Quaternion)
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity); //Lo generamos localmente
        //Aqui decimos que vamos a spawnear este objeto que acabamos de instanciar
        ServerManager.Spawn(spawned); //Lo generamos en el servidor para todos
        SetSpawnedObject(spawned, script); //aca le mandamos el objeto que generamos en el servidor y hacemos referencia este script
        //lo estamos configurando en el script que generamos esto
    }

    // Aca usamos el modo de Servidores activos, es decir, enviamos esta funcion a cada uno de los observadores y la ejecutan al mismo tiempo
    [ObserversRpc]

    //Hacemos esta funcion para tener una referencia del objeto que acabamos de crear para poder eliminarlo/configurarlo mas tarde
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
    {
        //creo que asignamos el objeto creado por el script al objeto, como diciendole este objeto es de este script.
        script.spawnedObject = spawned;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }

}
