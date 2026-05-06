using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public List<GameObject> layer = new List<GameObject>();

    private void Start()
    {
        DeactivateLayer();
        layer[0].SetActive(true);
    }

    private void DeactivateLayer()
    {
        foreach (GameObject obj in layer)
        {
            obj.SetActive(false);
        }
    }

    public void ActivateLayer(int layerIndex)
    {
        DeactivateLayer();
        if (layerIndex >= 0 && layerIndex < layer.Count)
        {
            layer[layerIndex].SetActive(true);
        }
    }
}
