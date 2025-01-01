using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material highlightMaterial;
    private Material defaultMaterial;

    private void Start()
    {
        if (mesh == null)
        {
            mesh = GetComponentInChildren<MeshRenderer>();
        }

        defaultMaterial = mesh.material;
    }

    //是否在互動狀態,如果是則切換材質,反責復原
    public void HighlightActive(bool active)
    {
        if (active)
        {
            mesh.material = highlightMaterial;
        }
        else
        {
            mesh.material = defaultMaterial;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        playerInteraction.interactables.Add(this);
        playerInteraction.UpdateClosetInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        HighlightActive(false);
        playerInteraction.interactables.Remove(this);
        playerInteraction.UpdateClosetInteractable();
    }

}
