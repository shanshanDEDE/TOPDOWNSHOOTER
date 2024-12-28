using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//透過這個腳本會自動添加Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{

    private void Start()
    {
        //將物件的層級改為Enemy
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

}
