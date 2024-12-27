using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;

    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private bool rigShouldBeIncreased;

    [Header("左手IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    [SerializeField] private float leftHandIK_IncreaseStep;
    private bool ShouldIncreasedleftHandIKWeight;

    private Rig rig;

    private bool busyGrabbingWeapon;

    private void Start()
    {
        SwitchOn(pistol);

        anim = GetComponentInChildren<Animator>();

        rig = GetComponentInChildren<Rig>();

        Debug.Log((float)GrabType.SideGrab);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Reload");
            //由於左手受到IK影響,所以要將左手IK的權重設為0
            PauseRig();
        }

        //平滑回復rig Wigth
        UpdateRigWigth();

        //平滑回復左手IK權重
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (ShouldIncreasedleftHandIKWeight)
        {
            leftHandIK.weight += leftHandIK_IncreaseStep * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                ShouldIncreasedleftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWigth()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;

            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    private void PlayerWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        PauseRig();
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        busyGrabbingWeapon = busy;
        anim.SetBool("BushGrabbingWeapon", busyGrabbingWeapon);
    }

    public void ReturnRigWeighthToOne()
    {
        rigShouldBeIncreased = true;
    }

    public void ReturnWeightToLeftHandIK()
    {
        ShouldIncreasedleftHandIKWeight = true;
    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        //localPosition:物件相對於其 父物件 的位置,position:物件在 世界座標系 中的絕對位置
        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwitchAnimationLayer(3);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }
    }

}

public enum GrabType { SideGrab, BackGrab };
