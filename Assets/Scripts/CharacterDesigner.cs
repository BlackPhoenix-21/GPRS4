using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDesigner : MonoBehaviour
{
    public static CharacterDesigner Instance { get; private set; }

    public GameObject player;

    private Dictionary<CharacterLayer, GameObject> characterLayers =
        new Dictionary<CharacterLayer, GameObject>();
    private Dictionary<CharacterLayer, List<GameObject>> layerItems =
        new Dictionary<CharacterLayer, List<GameObject>>();

    public CharacterLayer currentLayer = CharacterLayer.None;

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
        foreach (var item in layerItems)
        {
            item.Value.ForEach(i => i.SetActive(false)); // Deactivate all items by default
            item.Value[0].SetActive(true); // Activate the first item in each layer by default
        }
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
                    Debug.Log($"Item: {item.name}");
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
            Debug.Log($"Processing layer: {layer.name}");
            characterLayers.Add((CharacterLayer)layerIndex, layer);

            for (int j = 0; j < layer.transform.childCount; j++)
            {
                GameObject item = layer.transform.GetChild(j).gameObject;
                Debug.Log($"Processing item: {item.name}");
                if (!layerItems.ContainsKey((CharacterLayer)layerIndex))
                {
                    layerItems[(CharacterLayer)layerIndex] = new List<GameObject>();
                }
                layerItems[(CharacterLayer)layerIndex].Add(item);
            }
            layerIndex++;
            Debug.Log("\n");
        }
    }

    public void ActivateItem(CharacterLayer layer, int itemIndex)
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

    public void ChangeMaterial(Material newMaterial)
    {
        if (characterLayers.ContainsKey(currentLayer))
        {
            MeshRenderer[] renderers = characterLayers[currentLayer]
                .GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = newMaterial;
            }
        }
    }

    public bool LoadCharacterData(CharacterData characterData)
    {
        currentCharacterData.characterData = characterData;
        for (int i = 0; i < characterData.characterLayers.Count; i++)
        {
            ActivateItem(characterData.characterLayers[i], characterData.itemIndices[i]);
        }
        foreach (List<GameObject> items in layerItems.Values)
        {
            foreach (GameObject item in items)
            {
                if (!item.activeSelf)
                {
                    Destroy(item);
                }
            }
        }
        return true;
    }

    public Dictionary<CharacterLayer, List<GameObject>> GetLayerItems()
    {
        return layerItems;
    }
}

public enum CharacterLayer
{
    None,
    Hair,
    Shirt,
    Pants,
    Shoes,
    Accessories,
    Face,
}
