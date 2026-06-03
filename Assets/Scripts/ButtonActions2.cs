using System.Collections.Generic;
using UnityEngine;

public class ButtonActions2 : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> layers = new List<GameObject>();

    public void OpenLayer(int index)
    {
        if (index < 0 || index >= layers.Count)
        {
            Debug.LogError("Falscher Layer Index: " + index);
            return;
        }

        layers[index].transform.SetAsLastSibling();

        Debug.Log("Open Layer: " + index);
    }
}
