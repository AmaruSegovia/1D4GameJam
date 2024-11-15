using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class PlayerColorNetwork : NetworkBehaviour
{
    public GameObject body;
    public Color endColor;

    public override void OnStartClient()
    {
        base.OnStartClient();
        //Si somos los propietarios no haremos nada
        if (base.IsOwner)
        {

        }
        //Si no somos propietarios desactivamos el script para no controlar algo que no nos corresponde
        else
        {
            GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    private void Update()
    {
        //Si presionamos F cambia el color
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeColorServer(gameObject, endColor);
        }
    }

    //Hacemos que cargue esta funcion al servidor y le envie todos estos datos los que esten el servidor
    [ServerRpc]
    public void ChangeColorServer(GameObject player, Color color)
    {
        ChangeColor(player, color);
    }

    //Le enviamos a traves del objeto jugador, el cambio de color
    [ObserversRpc]
    public void ChangeColor(GameObject player, Color color)
    {
        //Primero hacemos referencia al mismo Script, luego referenciamos al body y traemos el renderizado,
        // luego escogemos el material y el color y le decimos que va a ser igual al nuevo color
        player.GetComponent<PlayerColorNetwork>().body.GetComponent<Renderer>().material.color = color;
    }
}