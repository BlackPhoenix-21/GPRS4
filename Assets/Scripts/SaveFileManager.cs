using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager Instance { get; private set; }
    private SaveData saveData = new SaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCharacter(SaveData characterData)
    {
        string json = JsonUtility.ToJson(characterData);
        File.WriteAllText(Application.persistentDataPath + "/characterData.json", json);
    }
}

[Serializable]
public class CharacterData
{
    public List<CharacterLayer> characterLayers = new List<CharacterLayer>();
    public List<int> itemIndices = new List<int>();
    public List<string> colorIndices = new List<string>();

    public CharacterData()
    {
        characterLayers = new List<CharacterLayer>();
        itemIndices = new List<int>();
        colorIndices = new List<string>();
    }
}

[Serializable]
public class SaveData
{
    // Extra subklasse um die Erweiterung von neuen Daten zur Speicherung zu ermöglichen, ohne die bestehende Struktur zu ändern.
    public CharacterData characterData = new CharacterData();

    public SaveData()
    {
        characterData = new CharacterData();
    }
}
