using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
    public GameObject player;

    public void LoadPlayerData(SaveData data)
    {
        CharacterDesigner cd = player.AddComponent<CharacterDesigner>();
        cd.player = player;
        bool finished = cd.LoadCharacterData(data.characterData);

        if (finished)
        {
            Destroy(cd);
            Debug.Log("Player loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load player data.");
        }
    }
}
