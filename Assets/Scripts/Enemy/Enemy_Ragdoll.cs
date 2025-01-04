using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ragdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollParent;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    private void Awake()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        RagdollActive(false);                           //啟用isKinematic(不啟用運動學的意思)
    }

    //啟用或關閉運動學
    public void RagdollActive(bool active)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !active;
        }
    }

    //啟用或關閉碰撞
    public void ColliderActive(bool active)
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = active;
        }
    }
}
