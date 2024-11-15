using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using TMPro;

public class PlayerPickupGuide : NetworkBehaviour
{
    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] Transform pickupPosition;
    [SerializeField] KeyCode pickupButton = KeyCode.E;
    [SerializeField] KeyCode dropButton = KeyCode.Q;

    Camera cam;
    bool hasObjectInHand; //utilizamos un bool para saber si tenemos un objeto en la mano
    GameObject objInHand;
    Transform worldObjectHolder; //position rotacion y escala de un objeto (cual objeto? ni idea)

    public override void OnStartClient()
    {
        base.OnStartClient();
        //la misma configuracion de siempre, si no somos propietarios desactivamos el script
        if (!base.IsOwner)
            enabled = false;
        //la camara que vamos a usar en este script es la camara main, pero podria ser otra
        cam = Camera.main;
        worldObjectHolder = GameObject.FindGameObjectWithTag("WorldObjects").transform; //aca supongo que sincronizamos nuestra variable local con el gameobject que creamos en la escena
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupButton))
            Pickup();

        if (Input.GetKeyDown(dropButton))
            Drop();
    }

    void Pickup()
    {
        //en este caso mandamos un raycast que comience en la posicion de la camara y tenga la direccion de esta, generamos el raycast, escogemos la distancia y el ultimo
        //parametro hace referencia al layermask, es decir, todos los objetos que se encuentren en esa capa
        //seran afectados por este rayo, los que no no
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, raycastDistance, pickupLayer))
        {
            if (!hasObjectInHand)
            {
                //aca le pasamos el objeto que agarramos con el rayo luego la posicion y la rotacion del objeto vacio de nuestro prefab y el jugador asignado
                SetObjectInHandServer(hit.transform.gameObject, pickupPosition.position, pickupPosition.rotation, gameObject);
                //el objeto que tenemos en la mano pasa a ser el objeto que agarramos con el raycast (o eso entendi)
                objInHand = hit.transform.gameObject;
                //y pasamos esto a true porque ya tenemos algo en la manito
                hasObjectInHand = true;
            }
            else if (hasObjectInHand)
            {
                Drop();

                SetObjectInHandServer(hit.transform.gameObject, pickupPosition.position, pickupPosition.rotation, gameObject);
                objInHand = hit.transform.gameObject;
                hasObjectInHand = true;
            }
        }
    }

    // el RequireOwnerShip false significa no hace falta que seamos propietarios¿? para modificar esto
    [ServerRpc(RequireOwnership = false)]
    // Queremos saber que objeto hemos agarrado, tambien la posicion y la rotacion donde queremos colocarlo y luego saber quien esta agarrando el objeto (osea el padre del objeto)
    void SetObjectInHandServer(GameObject obj, Vector3 position, Quaternion rotation, GameObject player)
        //Esta posicion y rotacion la vamos a sacar del prefab del jugador donde hay un objeto vacio que se llama PickupPosition
    {
        SetObjectInHandObserver(obj, position, rotation, player);
    }

    [ObserversRpc]
    void SetObjectInHandObserver(GameObject obj, Vector3 position, Quaternion rotation, GameObject player)
    {
        obj.transform.position = position; //aca le decimos que la posicion del objeto ahora va a ser la nueva posicion que le pasemos
        obj.transform.rotation = rotation; //al igual que la rotacion
        obj.transform.parent = player.transform; //creo que esto hace que el objeto ahora sea un hijo del jugador

        if (obj.GetComponent<Rigidbody>() != null) 
            obj.GetComponent<Rigidbody>().isKinematic = true;
        //aca le decimos que si el objeto tiene un rigidbody sea kinematic para que no sea afectado por las colisiones o otro sistema de fisicas
    }

    void Drop()
    {
        //Si no tenemos nada en la mano simplemente retorna nada xd
        if (!hasObjectInHand)
            return;
        //aca le decimos que vamos a soltar el objeto que tenemos en la mano 
        DropObjectServer(objInHand, worldObjectHolder);
        hasObjectInHand = false;
        objInHand = null;
    }

    [ServerRpc(RequireOwnership = false)]
    void DropObjectServer(GameObject obj, Transform worldHolder)
    {
        DropObjectObserver(obj, worldHolder);
    }

    [ObserversRpc]
    void DropObjectObserver(GameObject obj, Transform worldHolder)
    {
        obj.transform.parent = worldHolder;

        if (obj.GetComponent<Rigidbody>() != null)
            obj.GetComponent<Rigidbody>().isKinematic = false;
    }
}
