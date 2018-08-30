using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MySelectable : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{

    public static HashSet<MySelectable> allMySelectables = new HashSet<MySelectable>();
    public static HashSet<MySelectable> currentlySelected = new HashSet<MySelectable>();

    Renderer myRenderer;

    [SerializeField]
    Material unselectedMaterial;
    [SerializeField]
    Material selectedMaterial;

    void Awake()
    {
        allMySelectables.Add(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelect(eventData);
    }
    public void OnSelect(BaseEventData eventData)
    {
        DeselectAll(eventData);
        currentlySelected.Add(this);
        myRenderer.material = selectedMaterial;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        myRenderer.material = unselectedMaterial;
    }

    public static void DeselectAll(BaseEventData eventData)
    {
        foreach (MySelectable selectable in currentlySelected)
        {
            selectable.OnDeselect(eventData);
        }

        currentlySelected.Clear();
    }

}

