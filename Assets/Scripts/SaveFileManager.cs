using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager instance { get; private set; }
    private SaveData saveData = new SaveData();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        return;
#pragma warning disable CS0162 // Unreachable code detected
        saveData.characterData = new CharacterData();
#pragma warning restore CS0162 // Unreachable code detected
        saveData.characterData.characterLayers.AddRange(
            Enum.GetValues(typeof(CharacterLayer)) as CharacterLayer[]
        );
        foreach (CharacterLayer layer in saveData.characterData.characterLayers)
        {
            saveData.characterData.itemIndices.Add(UnityEngine.Random.Range(0, 3));
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/characterData.json", json);
    }

    public void SaveCharacter(SaveData characterData)
    {
        string json = JsonUtility.ToJson(characterData);
        File.WriteAllText(Application.persistentDataPath + "/characterData.json", json);
        Debug.Log("Character saved!");
    }
}

[Serializable]
public class CharacterData
{
    public List<CharacterLayer> characterLayers = new List<CharacterLayer>();
    public List<int> itemIndices = new List<int>();

    public CharacterData()
    {
        characterLayers = new List<CharacterLayer>();
        itemIndices = new List<int>();
    }
}

[Serializable]
public class SaveData
{
    public CharacterData characterData = new CharacterData();

    public SaveData()
    {
        characterData = new CharacterData();
    }
}
