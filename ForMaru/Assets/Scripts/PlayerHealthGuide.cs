using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usamos el modo sincronizacion de Fishnet
using FishNet.Object.Synchronizing;
using FishNet.Object;
using TMPro;


public class PlayerHealthGuide : NetworkBehaviour
{
    [SyncVar] public int health = 10;
    private TextMeshProUGUI healthText;

    private void Start()
    {
        // Asignamos nuestra variable (o clase) local a un gameobject con el tag (HealthText)
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!base.IsOwner)
            return; //Decimos que si no somos el propietario no queremos hacer nada
        //Normalmente solemos desactivar el script pero en este caso queremos que el script este habilitado para todos

        healthText.text = health.ToString();
    }
}
