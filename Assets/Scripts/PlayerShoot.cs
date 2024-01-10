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

    [Client]
    private void Shoot()
    {
        Debug.Log("Tir effectué");

        RaycastHit hit;

        if( Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, currentWeapon.range, mask))
        {  
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
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
