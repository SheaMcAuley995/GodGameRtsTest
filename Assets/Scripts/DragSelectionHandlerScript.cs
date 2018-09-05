using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DragSelectionHandlerScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [SerializeField]
    Image selectionBoxImage;

    Vector2 startPos;
    Rect selectionRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            MySelectable.DeselectAll(new BaseEventData(EventSystem.current));
        }
       
        selectionBoxImage.gameObject.SetActive(true);
        startPos = eventData.position;
        selectionRect = new Rect();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.position.x < startPos.x)
        {
            selectionRect.xMin = eventData.position.x;
            selectionRect.xMax = startPos.x;
        }
        else
        {
            selectionRect.xMin = startPos.x;
            selectionRect.xMax = eventData.position.x;
        }

        if (eventData.position.y < startPos.y)
        {
            selectionRect.yMin = eventData.position.y;
            selectionRect.yMax = startPos.y;
        }
        else
        {
            selectionRect.yMin = startPos.y;
            selectionRect.yMax = eventData.position.y;
        }

        selectionBoxImage.rectTransform.offsetMin = selectionRect.min;
        selectionBoxImage.rectTransform.offsetMax = selectionRect.max;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBoxImage.gameObject.SetActive(false);
        foreach(MySelectable selectable in MySelectable.allMySelectables)
        {
            if(selectionRect.Contains(Camera.main.WorldToScreenPoint(selectable.transform.position)))
            {
                selectable.OnDeselect(eventData);
            }
        }
    }

}
