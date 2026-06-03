using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private string parentLayerName = "ParentLayer";

    [SerializeField]
    private string partenButtonName = "ParentButton";

    [SerializeField]
    private GameObject lowLayer;

    [SerializeField]
    private GameObject highLayer;

    private List<GameObject> layers = new List<GameObject>();
    private List<Button> buttons = new List<Button>();
    private List<GameObject> layerItems = new List<GameObject>();

    private void Start()
    {
        GetAllButtons();
        GetAllLayers();

        layers[0].transform.SetParent(highLayer.transform);
        buttons[0].interactable = false;
    }

    private void GetAllButtons()
    {
        Transform buttonsParent = canvas.transform.Find(partenButtonName);
        if (buttonsParent != null)
        {
            foreach (Transform button in buttonsParent)
            {
                Button btn = button.GetComponent<Button>();
                if (btn != null)
                {
                    buttons.Add(btn);
                }
            }
        }
    }

    private void GetAllLayers()
    {
        Transform layersParent = canvas.transform.Find(parentLayerName);
        if (layersParent != null)
        {
            foreach (Transform ly in layersParent)
            {
                layers.Add(ly.gameObject);
                layerItems.Add(ly.GetChild(0).gameObject);
            }

            SetAllLowLayer();

            if (layersParent.childCount > 0)
            {
                Debug.Log("layerParent has " + layersParent.childCount + " children.");
                return;
            }

            Destroy(layersParent.gameObject);
        }
    }

    private void SetAllLowLayer()
    {
        foreach (GameObject layer in layers)
        {
            layer.transform.SetParent(lowLayer.transform);
        }
    }

    private void SetAllUnInteractable()
    {
        foreach (Button btn in buttons)
        {
            btn.interactable = true;
        }
    }

    public void ActivateLayer(int layerIndex)
    {
        SetAllLowLayer();
        SetAllUnInteractable();
        if (layerIndex >= 0 && layerIndex < layers.Count)
        {
            layers[layerIndex].transform.SetParent(highLayer.transform);
            buttons[layerIndex].interactable = false;
            Debug.Log("Activated Layer: " + layerIndex);
        }
        else
        {
            Debug.LogError("Falscher Layer Index: " + layerIndex);
        }
    }
}
