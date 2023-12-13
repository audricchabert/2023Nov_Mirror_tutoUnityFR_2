using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string playerIdToRegister, Player playerToRegister)
    {
        //Add the player to the dictionnary
        string playerKeyToRegister = playerIdPrefix + playerIdToRegister;
        players.Add(playerKeyToRegister, playerToRegister);

        //Rename the player object in the scene
        playerToRegister.transform.name = playerKeyToRegister;
    }

    public static void UnregisterPlayer(string playerIdToUnregister)
    {
        players.Remove(playerIdToUnregister);
    }

    //debug method that displays on the GUI informations
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerId in players.Keys)
        {
            GUILayout.Label(playerId + " - " + players[playerId].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
