using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> layer = new List<GameObject>();

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        DeactivateLayer();
        ActivateButtons();
        buttons[0].interactable = false;
        layer[0].SetActive(true);
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
        }
    }
}
