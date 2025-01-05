using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("彈道雷射")]
    [SerializeField] private LineRenderer aimLaser; //這個組件放在weapon holder上(player的子物件)

    [Header("準心控制")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;     //是否鎖定y的準瞄準狀態
    [SerializeField] private bool isLockingToTarget;    //是否會鎖定目標

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

    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        //是否開啟
        if (Input.GetKeyDown(KeyCode.P))
        {
            isAimingPrecisly = !isAimingPrecisly;
        }

        //是否開啟
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLockingToTarget = !isLockingToTarget;
        }

        updateAimVisuals();   //更新彈道預測線

        UpdateAimPosition();
        UpdateCameraPosition();
    }

    //更新彈道預測線
    private void updateAimVisuals()
    {
        aimLaser.enabled = player.weapon.WeaponReady();

        if (aimLaser.enabled == false)
            return;


        WeaponModel weaponModel = player.weaponVisuals.CurrentWeaponModel();

        //將槍口及子彈生成時的瞄準方向瞄準到aim上
        weaponModel.transform.LookAt(aim);
        weaponModel.gunPoint.LookAt(aim);                    //移動到更好的地方ㄌ

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLenght = 0.5f;
        float gunDistance = player.weapon.CurrentWeapon().gunDistance;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        //如果彈道預測線上有東西則讓彈道長度停止伸長
        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLenght = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);

        //這是要讓彈道預測線看來碰觸到敵人前就會進入第三段立即隱形來達到看起來沒有碰到敵人
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLenght);
    }

    private void UpdateAimPosition()
    {
        Transform target = Target();
        //鼠標有點到目標則做判斷,達成條件則瞄準目標transform
        if (target != null && isLockingToTarget)
        {
            //取得目標的中心(因為有些資產或模型pivot不在中心因此這邊去抓中心)
            if (target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }

            return;
        }

        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
        {
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }
    }

    //取得射線射到的目標是否為Target
    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        if (target == null)
        {
            return null;
        }

        return target;
    }

    public Transform Aim() => aim;

    public bool CanAimPrecisly() => isAimingPrecisly;

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            //紀錄位置
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        //如果未偵測到射線撞到東西,則回傳上一次偵測到的位置
        return lastKnownMouseHit;
    }

    #region 相機相關區域

    private void UpdateCameraPosition()
    {
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

    #endregion

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Charcater.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Charcater.Aim.canceled += context => mouseInput = Vector2.zero;
    }
}
