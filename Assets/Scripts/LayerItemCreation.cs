using System;
using System.Collections.Generic;
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

    private float itemOffsetPositionY = 100f;
    private float itemOffsetPositionX = 100f;
    private int itemsPerRow = 3;
    private int currentItemCount = 0;
    private Vector2 itemPosition;

    private ItemCategory currentCategory = ItemCategory.None;
    private List<GameObject> groupedItem = new List<GameObject>();

    private ButtonActions buttonActions;

    private void Start()
    {
        buttonActions = FindAnyObjectByType<ButtonActions>();
        if (buttonActions != null)
        {
            buttonActions.OnFinishedSetup += SetUp;
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

    private void OrganizeItemsByLayer()
    {
        if (itemsData == null || itemsData.Length == 0)
        {
            Debug.LogWarning("No items data provided");
            return;
        }

        for (int i = 0; i < (int)CharacterLayer.Accessories + 1; i++)
        {
            itemsByLayer[i] = new List<ItemsScriptableObject>();
        }

        foreach (ItemsScriptableObject item in itemsData)
        {
            int layerIndex = (int)item.characterLayer - 1; // Enum starts with None at 0
            if (itemsByLayer.ContainsKey(layerIndex))
            {
                itemsByLayer[layerIndex].Add(item);
            }
        }
    }

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

        for (int i = 0; i < layerItemsParent.Count; i++)
        {
            if (itemsByLayer.ContainsKey(i))
            {
                foreach (ItemsScriptableObject item in itemsByLayer[i])
                {
                    GameObject newItem = Instantiate(itemPrefab, layerItemsParent[i].transform);
                    newItem.GetComponent<RectTransform>().anchoredPosition = itemPosition;
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
            }
        }
    }

    public void SetGroupedItem(ItemCategory category)
    {
        foreach (GameObject item in groupedItem)
        {
            Destroy(item);
        }
        groupedItem.Clear();
        itemPosition = itemStartPosition;
        currentItemCount = 0;
        currentCategory = category;
        if (buttonActions == null)
        {
            Debug.LogError("ButtonActions not found in scene");
            return;
        }

        List<GameObject> layerItemsParent = buttonActions.layerItems;

        for (int i = 0; i < layerItemsParent.Count; i++)
        {
            if (itemsByLayer.ContainsKey(i))
            {
                foreach (ItemsScriptableObject item in itemsByLayer[i])
                {
                    if (item.itemCategory != category)
                    {
                        continue;
                    }

                    GameObject newItem = Instantiate(itemPrefab, layerItemsParent[i].transform);
                    newItem.GetComponent<RectTransform>().anchoredPosition = itemPosition;
                    groupedItem.Add(newItem);
                    //Debug.LogWarning(
                    //    "Setting item: " + item.itemName + " in category: " + category
                    //);
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
            }
        }

        if (EventSystem.current?.currentSelectedGameObject != null)
        {
            Button clickedButton =
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (clickedButton != null)
            {
                clickedButton.onClick.RemoveAllListeners();
                clickedButton.onClick.AddListener(() => ClearGroupedItem());
            }
        }
    }

    public void ClearGroupedItem()
    {
        foreach (GameObject item in groupedItem)
        {
            Destroy(item);
        }
        groupedItem.Clear();
        itemPosition = itemStartPosition;
        currentItemCount = 0;

        if (EventSystem.current?.currentSelectedGameObject != null)
        {
            Button clickedButton =
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (clickedButton != null)
            {
                clickedButton.onClick.RemoveAllListeners();
                clickedButton.onClick.AddListener(() => SetGroupedItem(currentCategory));
            }
        }
    }
}
