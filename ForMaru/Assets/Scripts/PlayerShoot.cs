using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerShoot : NetworkBehaviour
{
    public int damage; //danio del disparo
    public float timeBetweenFire; //Tiempo entre disparo
    float fireTimer;
    private void Update()
    {
        if (!base.IsOwner)
            return; //Decimos que si no somos el propietario no queremos hacer nada
        //Normalmente solemos desactivar el script pero en este caso queremos que el script este habilitado para todos
        if (Input.GetButton("Fire1"))
        {
            if (fireTimer <= 0)
            {
                ShootServer(damage, Camera.main.transform.position, Camera.main.transform.forward);
                fireTimer = timeBetweenFire;
            }

        }
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    [ServerRpc (RequireOwnership = false)]

    private void ShootServer(int damageToGive, Vector3 position, Vector3 direction)
    {
        if(Physics.Raycast(position, direction, out RaycastHit hit) && hit.transform.TryGetComponent(out PlayerHealthGuide enemyHealth))
        {
            enemyHealth.health -= damageToGive; 
        }
    }

}
