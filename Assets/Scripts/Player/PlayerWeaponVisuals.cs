using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Animator anim;
    private Player player;

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

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
        //注意  GetComponents 要有s
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        //平滑回復rig Wigth
        UpdateRigWigth();

        //平滑回復左手IK權重
        UpdateLeftHandIKWeight();
    }

    public void PlayFireAnimation() => anim.SetTrigger("Fire");

    public void PlayerReloadAnimation()
    {
        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        //由於左手受到IK影響,所以要將左手IK的權重設為0
        ReduceRigWeight();
    }

    public void PlayerWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipType", (float)equipType);
        anim.SetFloat("EquipSpeed", equipmentSpeed);
    }

    //切換武器
    public void SwitchOnCurrentWeaponModel()
    {
        //取得現在的武器的拿槍方式
        int animationIndex = ((int)CurrentWeaponModel().holdType);

        //關閉所有武器
        SwitchOffWeaponModels();

        //關閉所有背後武器
        SwitchOffBackupWeaponModels();

        if (player.weapon.HasOnlyOneWeapon() == false)
        {
            //切換顯示的背後武器
            SwitchOnBackupWeaponModel();
        }

        //切換為該拿槍方式的動畫
        SwitchAnimationLayer(animationIndex);
        //透過取的整個model的方式就不用寫每個子物件了的相關資訊(如發射位置等等..)
        CurrentWeaponModel().gameObject.SetActive(true);
        AttachLeftHand();
    }

    //關閉所有武器
    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    //關閉所有背後武器
    public void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.gameObject.SetActive(false);
        }
    }

    //切換顯示的背後武器
    public void SwitchOnBackupWeaponModel()
    {
        //取的玩家前輩用武器的類型
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            if (backupWeaponModel.weaponType == weaponType)
            {
                backupWeaponModel.gameObject.SetActive(true);
            }
        }

    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
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

    #region  動畫 Riggging 方法

    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

        //localPosition:物件相對於其 父物件 的位置,position:物件在 世界座標系 中的絕對位置
        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
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

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    public void MaximizeRigWeight()
    {
        shouldIncrease_RigWeight = true;
    }

    public void MaximizeLeftHandWeight()
    {
        ShouldIncrease_leftHandIKWeight = true;
    }

    #endregion

}
