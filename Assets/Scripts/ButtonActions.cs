using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    private List<GameObject> parentLayer = new List<GameObject>();

    [SerializeField]
    private GameObject LowLayer;

    [SerializeField]
    private GameObject HighLayer;

    [SerializeField]
    private List<GameObject> layer = new List<GameObject>();

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        GetParentLayer();
        DeactivateLayer();
        ActivateButtons();
        SetLowLayer();
        SetHighLayer(parentLayer[0]);
        buttons[0].interactable = false;
        layer[0].SetActive(true);
    }

    private void GetParentLayer()
    {
        foreach (GameObject obj in layer)
        {
            parentLayer.Add(obj.transform.parent.gameObject);
        }
    }

    private void SetLowLayer()
    {
        foreach (GameObject obj in parentLayer)
        {
            obj.transform.SetParent(LowLayer.transform);
        }
    }

    private void SetHighLayer(GameObject obj)
    {
        obj.transform.SetParent(HighLayer.transform);
    }

    private void DeactivateLayer()
    {
        foreach (GameObject obj in layer)
        {
            obj.SetActive(false);
        }
    }

    private void ActivateButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
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
