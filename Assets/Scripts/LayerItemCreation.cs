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
    private ItemsScriptiableObject[] itemsData;

    private Dictionary<int, List<ItemsScriptiableObject>> itemsByLayer =
        new Dictionary<int, List<ItemsScriptiableObject>>();

    private float itemOffsetPositionY = 50f;
    private float itemOffsetPositionX = 50f;
    private int itemsPerRow = 3;
    private int currentItemCount = 0;
    private Vector2 itemPosition;

    private ItemCategory currentCategory = ItemCategory.None;
    private List<GameObject> groupedItem = new List<GameObject>();

    private void Start()
    {
        itemPosition = itemStartPosition;
        SetUpItems();
    }

    private void SetUpItems()
    {
        ButtonActions buttonActions = FindAnyObjectByType<ButtonActions>();
        List<GameObject> layerItemsParent = buttonActions.layerItems;
        for (int i = 0; i < layerItemsParent.Count; i++)
        {
            foreach (ItemsScriptiableObject item in itemsByLayer[i])
            {
                GameObject newItem = Instantiate(itemPrefab, layerItemsParent[i].transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = itemPosition;
                Debug.LogWarning(
                    "Setting item: " + item.itemName + " in category: " + item.itemCategory
                );
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

    public void SetGroupedItem(ItemCategory category)
    {
        foreach (GameObject item in groupedItem)
        {
            Destroy(item);
        }
        groupedItem.Clear();

        currentCategory = category;

        ButtonActions buttonActions = FindAnyObjectByType<ButtonActions>();
        List<GameObject> layerItemsParent = buttonActions.layerItems;

        for (int i = 0; i < layerItemsParent.Count; i++)
        {
            foreach (ItemsScriptiableObject item in itemsByLayer[i])
            {
                if (item.itemCategory != category)
                {
                    continue;
                }

                GameObject newItem = Instantiate(itemPrefab, layerItemsParent[i].transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = itemPosition;
                Debug.LogWarning("Setting item: " + item.itemName + " in category: " + category);
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

        // Button that was clicked to trigger this method
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (clickedButton != null)
        {
            clickedButton.onClick.RemoveAllListeners();
            clickedButton.onClick.AddListener(() => ClearGroupedItem());
        }
    }

    public void ClearGroupedItem()
    {
        foreach (GameObject item in groupedItem)
        {
            Destroy(item);
        }
        groupedItem.Clear();

        // Button that was clicked to trigger this method
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (clickedButton != null)
        {
            clickedButton.onClick.RemoveAllListeners();
            clickedButton.onClick.AddListener(() => SetGroupedItem(currentCategory));
        }
    }
}
