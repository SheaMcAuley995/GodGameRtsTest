using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InteractType { Collision, Interact, Both }
public enum ResourceType { Wood, Stone}

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    bool isFocus = false;
    Transform player;

    public InteractType interactType;
    public ResourceType resource;

    bool hasInteracted = false;
    public virtual void Interact()
    {
        if(resource == ResourceType.Stone)
        {
            ResourceManager.instance.amount_Rock++;
            Destroy(this.gameObject);
        }
        if(resource == ResourceType.Wood)
        {
            ResourceManager.instance.amount_Wood++;
            Destroy(this.gameObject);
        }
        ResourceManager.instance.StartCoroutine("changeText");
        Debug.Log("Interacting with " + transform.name);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                Debug.Log("INTERACT");
                hasInteracted = true;
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmos()
    {
        if (interactionTransform == null)
            interactionTransform = transform;


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}

