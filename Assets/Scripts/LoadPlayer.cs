using System.IO;
using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
    public GameObject player;
    private CharacterDesigner cd;

    private void Start()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/characterData.json");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        cd = player.AddComponent<CharacterDesigner>();
        cd.player = player;
        cd.currentCharacterData = data;
        cd.LoadingCharacterData += LoadPlayerData;
        cd.InitializeCharacterLayers();
    }

    public void LoadPlayerData()
    {
        bool finished = cd.LoadCharacterData();

        if (finished)
        {
            Destroy(cd);
            //Debug.Log("Player loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load player data.");
        }
    }
}
