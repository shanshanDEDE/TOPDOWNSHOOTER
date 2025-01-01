using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //玩家目前所有範圍內可互動的物件清單
    public List<Interactable> interactables;

    private Interactable closetInteractable;

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
}
