using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        //Instantiate the weapon at the weaponHolder position and rotation
        GameObject weaponIns = Instantiate(currentWeapon.graphics, weaponHolder.position, weaponHolder.rotation);
        //also need to make it a child of the weaponHolder to make it move with it
        weaponIns.transform.SetParent(weaponHolder);

        //Get the WeaponGraphics scripts attached to the weaponIns object (the prefab)
        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
        if(currentGraphics == null)
        {
            Debug.LogError("pas de script WeaponGraphics sur la prefab de l'arme" + weaponIns.name);
        }

        if (isLocalPlayer)
        {
            //since this whole script is only processed on the local player, the weapon layer is only changed on its own weapon. This is used in the double camera display do avoid clipping. If others weapons also had the layer name changed, then they would also be visible through the walls
            Utils.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

}
