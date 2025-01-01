using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //玩家目前所有範圍內可互動的物件清單
    private List<Interactable> interactables = new List<Interactable>();

    private Interactable closetInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();

        player.controls.Charcater.Interaction.performed += context => InteractWithCloset();
    }

    //跟最近的物件互動
    public virtual void InteractWithCloset()
    {
        closetInteractable?.Interaction();
        interactables.Remove(closetInteractable);

        //更新最近的互動物件
        UpdateClosetInteractable();
    }

    //尋找最近的互動物件
    public void UpdateClosetInteractable()
    {
        closetInteractable?.HighlightActive(false);

        closetInteractable = null;

        float closetDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance < closetDistance)
            {
                closetDistance = distance;
                closetInteractable = interactable;
            }
        }

        closetInteractable?.HighlightActive(true);
    }


    public List<Interactable> GetInteractables() => interactables;

}
