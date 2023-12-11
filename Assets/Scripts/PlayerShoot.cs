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

    private void Shoot()
    {
        RaycastHit hit;

        if( Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerWeapon.range, mask))
        {
            Debug.Log("object hit : " + hit.collider.name);
        }
    }
}
