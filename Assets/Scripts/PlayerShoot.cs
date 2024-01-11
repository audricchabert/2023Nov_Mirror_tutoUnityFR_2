using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        if(playerCamera == null)
        {
            Debug.LogError("Pas de caméra renseignée sur PlayerShoot");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
        
    }

    //Fonction appellée sur le serveur pour gérer le tir d'un player sur toutes les instances, pour afficher les effets visuels
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //Méthode qui fait apparaître les effets de tir d'un joueur sur tous les autres clients
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    //Command to the server ; to start playing the particles for the point that is shot at, a wall (or a player)
    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    //The function to play on each client the effects for a shot that has touched something, a wall (or a player)
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }

    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer) { return; }

        CmdOnShoot();

        RaycastHit hit;

        if( Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, currentWeapon.range, mask))
        {  
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }

            //if we hit a player of a wall, we play the effect for a shot impact
            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    private void CmdPlayerShot(string playerName, float shotDamage)
    {
        Debug.Log(playerName + "has been hit");

        Player player = GameManager.GetPlayer(playerName);
        player.RpcTakeDamage(shotDamage);
    }
}
