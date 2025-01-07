using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    public bool occupied; //occupied翻譯為佔領的意思

    public void SetOccupied(bool occupied)
    {
        this.occupied = occupied;
    }
}
