using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    private List<GameObject> layer = new List<GameObject>();
    private List<Button> buttons = new List<Button>();

    [SerializeField]
    private GameObject lowLayer;

    [SerializeField]
    private GameObject highLayer;

    private void Start() { }

    private void GetAllButtons()
    {
        Transform buttonsParent = canvas.transform.Find("Buttons");
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
        Transform layersParent = canvas.transform.Find("Layers");
        if (layersParent != null)
        {
            foreach (Transform layer in layersParent)
            {
                this.layer.Add(layer.gameObject);
            }
        }
    }

    public void ActivateLayer(int layerIndex)
    {
        DeactivateLayer();
        ActivateButtons();
        if (layerIndex >= 0 && layerIndex < layer.Count)
        {
            buttons[layerIndex].interactable = false;
            layer[layerIndex].SetActive(true);
            SetLowLayer();
            SetHighLayer(parentLayer[layerIndex]);
        }
    }
}
