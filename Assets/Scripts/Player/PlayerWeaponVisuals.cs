using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator anim;
    private bool isGrabbingWeapon;
    private Player player;

    [SerializeField] private WeaponModel[] weaponModels;

    [Header("Rig")]
    [SerializeField] private float rightWeightIncreaseRate;
    private bool shouldIncrease_RigWeight;
    private Rig rig;

    [Header("左手IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    private bool ShouldIncrease_leftHandIKWeight;


    private void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        //平滑回復rig Wigth
        UpdateRigWigth();

        //平滑回復左手IK權重
        UpdateLeftHandIKWeight();
    }

    //取得現在的weaponModel
    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }
        }

        return weaponModel;
    }


    public void PlayerReloadAnimation()
    {
        if (isGrabbingWeapon) return;

        anim.SetTrigger("Reload");
        //由於左手受到IK影響,所以要將左手IK的權重設為0
        reduceRigWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (ShouldIncrease_leftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                ShouldIncrease_leftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWigth()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rightWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
            {
                shouldIncrease_RigWeight = false;
            }
        }
    }

    private void reduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    private void PlayerWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        reduceRigWeight();
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        isGrabbingWeapon = busy;
        anim.SetBool("BushGrabbingWeapon", isGrabbingWeapon);
    }

    public void MaximizeRigWeight()
    {
        shouldIncrease_RigWeight = true;
    }

    public void MaximizeLeftHandWeight()
    {
        ShouldIncrease_leftHandIKWeight = true;
    }

    private void SwitchOn()
    {
        SwitchOffWeaponModels();

        //透過取的整個model的方式就不用寫每個子物件了的相關資訊(如發射位置等等..)
        CurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    private void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

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
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn();
            SwitchAnimationLayer(1);
            PlayerWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn();
            SwitchAnimationLayer(2);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn();
            SwitchAnimationLayer(3);
            PlayerWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}
