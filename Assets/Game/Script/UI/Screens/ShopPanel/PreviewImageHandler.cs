using Game.Script.Controllers.Elements.Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewImageHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerStorePreviewer itemPreviewer;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemPreviewer != null)
        {
            itemPreviewer.StartDragging(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemPreviewer != null)
        {
            itemPreviewer.StopDragging();
        }
    }
}