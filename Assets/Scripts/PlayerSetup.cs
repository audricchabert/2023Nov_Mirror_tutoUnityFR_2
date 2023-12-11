using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;

    private void Start()
    {
        RenamePlayer();

        if (!isLocalPlayer)
        {
            RemotePlayerSetup();
        }
        else // if we are the local player
        {
            //deactivate the main (the Scene Camera). Using the Camera.main object shortcut
            //also deactivate the main camera's audio listener?
            sceneCamera = Camera.main;
            //sceneCamera should never be null but just to be sure
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

        }

    }

    /**
     * renames the player object using its net Id
     */
    private void RenamePlayer()
    {
        string playerName = "Player" + GetComponent<NetworkIdentity>().netId;
        this.transform.name = playerName;

    }

    private void RemotePlayerSetup()
    {
        RemotePlayerDisableComponents();
        AssignRemoteLayer();
    }

    private void RemotePlayerDisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

    }
}
