using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim info")]
    [Range(0.5f, 1f)]
    [SerializeField] private float minCamaraDistance = 1.5f;
    [Range(1f, 3f)]
    [SerializeField] private float maxCamaraDistance = 4f;

    [Range(3f, 5f)]
    [SerializeField] private float aimSensetivity = 5f;


    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = Vector3.Lerp(aim.position, DesieredAimPosition(), Time.deltaTime * aimSensetivity);
    }

    //理想的準心位置
    private Vector3 DesieredAimPosition()
    {

        //透過actualMaxCamaraDistance防止玩家往下跑時玩家超出攝影機
        float actualMaxCamaraDistance = player.movement.moveInput.y < -0.5f ? minCamaraDistance : maxCamaraDistance;


        //計算攝影機準心位置
        Vector3 desiredAimPosition = GetMosuePosition();
        Vector3 aimDirection = (desiredAimPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredAimPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCamaraDistance, actualMaxCamaraDistance);

        desiredAimPosition = transform.position + aimDirection * clampedDistance;

        desiredAimPosition.y = transform.position.y + 1;

        return desiredAimPosition;
    }

    public Vector3 GetMosuePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Charcater.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Charcater.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
