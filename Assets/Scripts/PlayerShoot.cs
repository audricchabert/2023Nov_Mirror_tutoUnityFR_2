using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon playerWeapon;

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
        player.TakeDamage(shotDamage);
    }
}
