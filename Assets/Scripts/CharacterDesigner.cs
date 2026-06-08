using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDesigner : MonoBehaviour
{
    public static CharacterDesigner Instance { get; private set; }

    [SerializeField]
    private GameObject player;

    private Dictionary<CharacterLayer, GameObject> characterLayers =
        new Dictionary<CharacterLayer, GameObject>();
    private Dictionary<CharacterLayer, List<GameObject>> layerItems =
        new Dictionary<CharacterLayer, List<GameObject>>();

    private SaveData currentCharacterData;

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

    private void Start()
    {
        // Load CharacterData?
        currentCharacterData = new SaveData();
        foreach (CharacterLayer layer in Enum.GetValues(typeof(CharacterLayer)))
        {
            currentCharacterData.characterData.characterLayers.Add(layer);
            currentCharacterData.characterData.itemIndices.Add(0); // Default to first item
        }

        InitializeCharacterLayers();
        Debuger();
    }

    private void Debuger()
    {
        foreach (var layer in characterLayers)
        {
            Debug.Log($"Layer: {layer.Key}, GameObject: {layer.Value.name}");
            if (layerItems.ContainsKey(layer.Key))
            {
                foreach (var item in layerItems[layer.Key])
                {
                    Debug.Log($"  Item: {item.name}");
                }
            }
        }
    }

    private void InitializeCharacterLayers()
    {
        int layerIndex = 1; // Start from 1 to skip None
        for (int i = 0; i < player.transform.childCount; i++)
        {
            GameObject layer = player.transform.GetChild(i).gameObject;

            if (layer.name == "Body") // Skip body layer for now
            {
                continue;
            }

            characterLayers.Add((CharacterLayer)layerIndex, layer);
            foreach (GameObject item in layer.transform)
            {
                if (!layerItems.ContainsKey((CharacterLayer)layerIndex))
                {
                    layerItems[(CharacterLayer)layerIndex] = new List<GameObject>();
                }
                layerItems[(CharacterLayer)layerIndex].Add(item);
            }
            layerIndex++;
        }
    }

    private void ActivateItem(CharacterLayer layer, int itemIndex)
    {
        if (layerItems.ContainsKey(layer) && itemIndex < layerItems[layer].Count)
        {
            for (int i = 0; i < layerItems[layer].Count; i++)
            {
                layerItems[layer][i].SetActive(i == itemIndex);
            }
        }
        currentCharacterData.characterData.itemIndices[
            currentCharacterData.characterData.characterLayers.IndexOf(layer)
        ] = itemIndex;
    }
}

public enum CharacterLayer
{
    None,
    Clothes,
    Hair,
    Face,
    Body,
    Accessories,
}

public enum ItemCategory
{
    None,
    Shirt,
    Pants,
    Shoes,
    Hat,
    Glasses,
    Necklace,
    Earrings,
}
