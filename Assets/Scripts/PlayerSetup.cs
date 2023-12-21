using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private string dontDrawLayerName = "DontDraw";

    [SerializeField]
    private GameObject playerGraphics;

    Camera sceneCamera;

    private void Start()
    {
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

            //Désactiver la partie graphique du joueur local
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

        }
        GetComponent<Player>().Setup();
    }

    //OnStartClient comes from the NetworkBehavior. Is run automatically when the client starts
    public override void OnStartClient()
    {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
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

    // This method is run automatically when the component is disabled (ie : when the unity instance disconnects from the server? quits the scene?)
    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }


        GameManager.UnregisterPlayer(transform.name);
    }

    private void SetLayerRecursively(GameObject gameObject, int newLayer)
    {
        gameObject.layer = newLayer;
        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
