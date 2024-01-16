using UnityEngine;

public class PlayerUIScript : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController playerController;

    public void SetController(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    private void Update()
    {
        SetFuelAmount(playerController.GetThrusterFuelAmount());
    }
}
