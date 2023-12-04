using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera sceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
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

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

    }
}
