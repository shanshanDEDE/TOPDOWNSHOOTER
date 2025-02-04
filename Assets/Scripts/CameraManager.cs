using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera virtualCamara;
    private CinemachineFramingTransposer transposer;

    [Header("相機距離")]
    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] private float distanceChangeRate;
    private float targetCamaraDistance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        virtualCamara = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = virtualCamara.GetCinemachineComponent<CinemachineFramingTransposer>();

    }

    private void Update()
    {
        UpdateCameraDistance();
    }

    //更新相機距離
    private void UpdateCameraDistance()
    {
        if (canChangeCameraDistance == false) return;

        float currentDistance = transposer.m_CameraDistance;

        //防止lerp一直去更動
        if (Mathf.Abs(targetCamaraDistance - currentDistance) < 0.1f)
        {
            return;
        }
        transposer.m_CameraDistance =
            Mathf.Lerp(transposer.m_CameraDistance, targetCamaraDistance, distanceChangeRate * Time.deltaTime);
    }

    public void ChangeCameraDistance(float distance) => targetCamaraDistance = distance;


}
