using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon playerWeapon;

    [SerializeField]
    private GameObject weaponGFX;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if(playerCamera == null)
        {
            Debug.LogError("Pas de caméra renseignée sur PlayerShoot");
            this.enabled = false;
        }

        //since this whole script is only processed on the local player, the weapon layer is only changed on its own weapon. This is used in the double camera display do avoid clipping. If others weapons also had the layer name changed, then they would also be visible through the walls
        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit hit;

        if( Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerWeapon.range, mask))
        {  
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, playerWeapon.damage);
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
