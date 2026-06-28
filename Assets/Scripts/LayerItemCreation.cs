using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LayerItemCreation : MonoBehaviour
{
    [SerializeField]
    private Vector2 itemStartPosition;

    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private ItemsScriptableObject[] itemsData;

    private Dictionary<int, List<ItemsScriptableObject>> itemsByLayer =
        new Dictionary<int, List<ItemsScriptableObject>>();

    private float itemOffsetPositionY = 250f;
    private float itemOffsetPositionX = 250f;
    private int itemsPerRow = 3;
    private int currentItemCount = 0;
    private Vector2 itemPosition;

    private ButtonActions buttonActions;

    private void Start()
    {
        buttonActions = FindAnyObjectByType<ButtonActions>();
        if (buttonActions != null)
        {
            CharacterDesigner.Instance.OnFinishedSetup += SetUp;
        }
        else
        {
            Debug.LogError("ButtonActions not found in scene");
        }
    }

    private void SetUp()
    {
        itemPosition = itemStartPosition;
        OrganizeItemsByLayer();
        SetUpItems();
    }

    /// <summary>
    /// Organizes the items by their corresponding CharacterLayer and stores them in a dictionary for easy access.
    /// It also sorts the items based on the FBX GameObjects in the scene to ensure that the items are displayed in the correct order.
    /// </summary>
    private void OrganizeItemsByLayer()
    {
        if (itemsData == null || itemsData.Length == 0)
        {
            Debug.LogWarning("No items data provided");
            return;
        }

        for (int i = 0; i < (int)CharacterLayer.Face + 1; i++)
        {
            itemsByLayer[i] = new List<ItemsScriptableObject>();
        }

        foreach (ItemsScriptableObject item in itemsData)
        {
            int layerIndex = (int)item.characterLayer - 1;
            if (itemsByLayer.ContainsKey(layerIndex))
                itemsByLayer[layerIndex].Add(item);
        }

        Dictionary<int, List<ItemsScriptableObject>> sortedItemsByLayer =
            new Dictionary<int, List<ItemsScriptableObject>>();

        Dictionary<CharacterLayer, List<GameObject>> itemsFBX =
            CharacterDesigner.Instance.GetLayerItems();

        foreach (var list in itemsFBX)
        {
            int index = (int)list.Key - 1;

            if (!sortedItemsByLayer.ContainsKey(index))
                sortedItemsByLayer[index] = new List<ItemsScriptableObject>();

            foreach (var fbx in list.Value)
            {
                ItemsScriptableObject matchingItem = null;
                foreach (var item in itemsData)
                {
                    if (item.assetname == fbx.name)
                    {
                        matchingItem = item;
                        break;
                    }
                }

                if (matchingItem == null)
                {
                    Debug.LogWarning($"No matching item found for FBX: {fbx.name}");
                    continue;
                }

                sortedItemsByLayer[index].Add(matchingItem);
            }
        }
        itemsByLayer = sortedItemsByLayer;
    }

    /// <summary>
    /// Sets up the items in the UI by instantiating the item prefabs and assigning their properties based on the corresponding ItemsScriptableObject data.
    /// It also sets up the button click events to activate the corresponding item in the CharacterDesigner when clicked.
    /// </summary>
    private void SetUpItems()
    {
        if (buttonActions == null)
        {
            Debug.LogError("ButtonActions not found in scene");
            return;
        }

        List<GameObject> layerItemsParent = buttonActions.layerItems;
        if (layerItemsParent == null || layerItemsParent.Count == 0)
        {
            Debug.LogWarning("No layer items parent found");
            return;
        }

        CharacterDesigner characterDesigner = FindAnyObjectByType<CharacterDesigner>();
        if (characterDesigner == null)
        {
            Debug.LogError("CharacterDesigner not found in scene");
            return;
        }

        for (int i = 0; i < layerItemsParent.Count; i++)
        {
            if (itemsByLayer.ContainsKey(i))
            {
                foreach (ItemsScriptableObject item in itemsByLayer[i])
                {
                    GameObject newItem = Instantiate(itemPrefab, layerItemsParent[i].transform);
                    newItem.GetComponent<RectTransform>().anchoredPosition = itemPosition;
                    Button btn = newItem.GetComponentInChildren<Button>();
                    Image[] img = newItem.GetComponentsInChildren<Image>();
                    img[1].sprite = item.itemImage;

                    int itemIndex = itemsByLayer[i].IndexOf(item);
                    btn.onClick.AddListener(() =>
                    {
                        characterDesigner.ActivateItem(item.characterLayer, itemIndex);
                        Debug.Log(
                            $"Button clicked for item: {item.itemName} in layer: {item.characterLayer}"
                        );
                    });

                    newItem.GetComponentInChildren<TMP_Text>().text = item.itemName;

                    //Debug.LogWarning("Setting item: " + item.itemName + " in category: " + item.itemCategory);
                    currentItemCount++;

                    if (currentItemCount % itemsPerRow == 0)
                    {
                        itemPosition.x = itemStartPosition.x;
                        itemPosition.y -= itemOffsetPositionY;
                    }
                    else
                    {
                        itemPosition.x += itemOffsetPositionX;
                    }
                }
                currentItemCount = 0;
                itemPosition = itemStartPosition;
            }
        }
    }
}
