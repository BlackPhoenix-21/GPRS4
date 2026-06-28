using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDesigner : MonoBehaviour
{
    public static CharacterDesigner Instance { get; private set; }
    public event Action OnFinishedSetup;

    public GameObject player;

    private Dictionary<CharacterLayer, GameObject> characterLayers =
        new Dictionary<CharacterLayer, GameObject>();
    private Dictionary<CharacterLayer, List<GameObject>> layerItems =
        new Dictionary<CharacterLayer, List<GameObject>>();

    public CharacterLayer currentLayer = CharacterLayer.None;

    public SaveData currentCharacterData { get; private set; } = new SaveData();

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
        foreach (CharacterLayer layer in Enum.GetValues(typeof(CharacterLayer)))
        {
            currentCharacterData.characterData.characterLayers.Add(layer);
            currentCharacterData.characterData.itemIndices.Add(0); // Default to first item
        }
        FindAnyObjectByType<ButtonActions>().OnFinishedSetup += SetUp;
    }

    private void SetUp()
    {
        InitializeCharacterLayers();
        foreach (var item in layerItems)
        {
            item.Value.ForEach(i => i.SetActive(false)); // Deactivate all items by default
            item.Value[0].SetActive(true); // Activate the first item in each layer by default
        }
        OnFinishedSetup?.Invoke();
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

    /// <summary>
    /// Initializes the character layers by iterating through the child GameObjects of the player GameObject.
    /// It populates the characterLayers dictionary with the corresponding CharacterLayer enum and GameObject.
    /// </summary>
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

            for (int j = 0; j < layer.transform.childCount; j++)
            {
                GameObject item = layer.transform.GetChild(j).gameObject;
                if (!layerItems.ContainsKey((CharacterLayer)layerIndex))
                {
                    layerItems[(CharacterLayer)layerIndex] = new List<GameObject>();
                }
                layerItems[(CharacterLayer)layerIndex].Add(item);
            }
            layerIndex++;
        }
        Dictionary<CharacterLayer, GameObject> temp = new Dictionary<CharacterLayer, GameObject>();
        for (int i = 1; i < characterLayers.Count + 1; i++)
        {
            temp.Add((CharacterLayer)i, characterLayers[(CharacterLayer)i]);
        }
        characterLayers = temp;
    }

    /// <summary>
    /// Activates the specified item in the given CharacterLayer and deactivates all other items in that layer.
    /// It also updates the currentCharacterData to reflect the selected item index for that layer.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="itemIndex"></param>
    public void ActivateItem(CharacterLayer layer, int itemIndex)
    {
        Debug.Log($"Activating item at index {itemIndex} in layer {layer}");
        if (!layerItems.ContainsKey(layer))
        {
            Debug.LogWarning($"Layer {layer} does not exist in layerItems.");
            return;
        }
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

    /// <summary>
    /// Changes the material of the currently active item in the specified CharacterLayer to the newMaterial provided.
    /// It iterates through all MeshRenderer components in the active item and updates their material.
    /// </summary>
    /// <param name="newMaterial"></param>
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

    /// <summary>
    /// Loads the character data from the provided CharacterData object and activates the corresponding items in each CharacterLayer.
    /// It also destroys any inactive items in the layerItems dictionary to clean up the scene.
    /// </summary>
    /// <param name="characterData"></param>
    /// <returns></returns>
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
    Accessories,
    Hair,
    Shirt,
    Pants,
    Shoes,
    Face,
}
