using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("準心控制")]
    [SerializeField] private Transform aim;

    [Header("相機控制")]
    [SerializeField] private Transform camaraTarget;
    [Range(0.5f, 1f)]
    [SerializeField] private float minCamaraDistance = 1.5f;
    [Range(1f, 3f)]
    [SerializeField] private float maxCamaraDistance = 4f;
    [Range(3f, 5f)]
    [SerializeField] private float camaraSensetivity = 5f;

    [Space]

    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 aimInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);

        camaraTarget.position = Vector3.Lerp(camaraTarget.position, DesieredCameraPosition(), Time.deltaTime * camaraSensetivity);
    }

    //理想的準心位置
    private Vector3 DesieredCameraPosition()
    {

        //透過actualMaxCamaraDistance防止玩家往下跑時玩家超出攝影機
        float actualMaxCamaraDistance = player.movement.moveInput.y < -0.5f ? minCamaraDistance : maxCamaraDistance;


        //計算攝影機準心位置
        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCamaraDistance, actualMaxCamaraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;

        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            //紀錄位置
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        //如果未偵測到射線撞到東西,則回傳上一次偵測到的位置
        return lastKnownMouseHit;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Charcater.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Charcater.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
