using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
//Esto es nuevo
using FishNet.Object.Synchronizing;

public class PlayerHealth : NetworkBehaviour //Otra vez pasamos esto a NetworkBehaviour
{
    //Le agregamos SyncVar a nuestra variable, esto hace que cuando esta variable cambie
    //tambien se va a actualizar en el servidor
    [SyncVar] public int health = 10;

    public override void OnStartClient()
    {
        base.OnStartClient();
        //La misma configuracion de siempre, si no somos propietarios el script se desactiva
        if (!base.IsOwner)
            GetComponent<PlayerSpawnObject>().enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            //Aca le decimos que la vida del jugador va a bajar en 1 cada que presionemos R
            UpdateHealth(this,-1);//
        }
    }

    //Hacemos la funcion para el servidor
    [ServerRpc]
    //Hacemos referencia a este script para que cambie la vida de este script segun un numero cualquiera
    public void UpdateHealth(PlayerHealth script, int amountToChange)
    {
        script.health += amountToChange;
    }
}
