using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected PlayerWeaponController weaponController;
    protected MeshRenderer mesh;

    [SerializeField] private Material highlightMaterial;
    protected Material defaultMaterial;

    private void Start()
    {
        if (mesh == null)
        {
            mesh = GetComponentInChildren<MeshRenderer>();
        }

        defaultMaterial = mesh.sharedMaterial;
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = mesh.sharedMaterial;
    }

    //互動
    public virtual void Interaction()
    {
        Debug.Log("Interact with" + gameObject.name);
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


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (weaponController == null)
        {
            weaponController = other.GetComponent<PlayerWeaponController>();
        }

        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosetInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction == null)
        {
            return;
        }
        HighlightActive(false);
        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosetInteractable();
    }

}
