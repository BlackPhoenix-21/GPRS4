using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RotatePlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform character;
    public float rotationSpeed = 150f;

    private bool isDragging = false;

    void Update()
    {
        if (!isDragging)
            return;
        if (Mouse.current == null)
            return;

        float mouseX = Mouse.current.delta.ReadValue().x;
        character.Rotate(0, -mouseX * rotationSpeed * Time.deltaTime, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
