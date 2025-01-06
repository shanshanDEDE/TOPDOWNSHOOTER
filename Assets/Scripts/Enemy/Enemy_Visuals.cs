using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy_MeleeWeaponType { OnHand, Throw, Unarmed }

public enum Enemy_RangeWeaponType { Pistol, Revolver, Shotgun, AutoRifle, Rifle }

public class Enemy_Visuals : MonoBehaviour
{
    public GameObject currentWeaponModel { get; private set; }

    [Header("Corruption visuls(腐敗水晶相關視覺)")]
    [SerializeField] private GameObject[] corruptionCrystals;   //所有腐敗水晶
    [SerializeField] private int corruptionAmount;              //腐敗水晶數量

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    //啟用或關閉武器Trail
    public void EnableWeaponTrail(bool enable)
    {
        //取得現在武器的模組
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        //去啟用或關閉Trail
        currentWeaponScript.EnableTrailEffect(enable);
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }

    //設置隨機腐敗水晶
    private void SetupRandomCorruption()
    {
        List<int> avalibleIndesx = new List<int>();
        corruptionCrystals = CollectCorruptionCrystals();   //取得所有腐敗水晶

        //將所有腐敗水晶加入avalibleIndesx列表並且全部關閉
        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            avalibleIndesx.Add(i);
            corruptionCrystals[i].SetActive(false);
        }

        //for迴圈依照corruptionAmount來決定要隨機開啟的數量
        for (int i = 0; i < corruptionAmount; i++)
        {
            if (avalibleIndesx.Count == 0) break;

            int randomIndex = Random.Range(0, avalibleIndesx.Count);            //一列表還有的長度隨機抽一個數
            int objectIndex = avalibleIndesx[randomIndex];                      //把抽到的數給objectIndex


            corruptionCrystals[objectIndex].SetActive(true);                    //開啟抽到的腐敗水晶
            avalibleIndesx.RemoveAt(randomIndex);                               //將該腐敗水晶從avalibleIndesx列表中移除
        }
    }

    //設定隨機顏色
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);
        newMat.mainTexture = colorTextures[randomIndex];
        skinnedMeshRenderer.material = newMat;
    }

    //設定隨機武器
    private void SetupRandomWeapon()
    {
        bool thisEnemyIsMelee = GetComponent<Enemy_Melee>() != null;
        bool thisEnemyIsRange = GetComponent<Enemy_Range>() != null;

        if (thisEnemyIsRange)
        {
            currentWeaponModel = FindRangeWeaponModel();
        }

        if (thisEnemyIsMelee)
        {
            currentWeaponModel = FindMeleeWeaponModel();
        }

        //顯示該武器
        currentWeaponModel.SetActive(true);

        OverrideAnimatorControllerIfCan();

    }

    //找出符合條件的武器(遠程)
    private GameObject FindRangeWeaponModel()
    {
        Enemy_RangeWeaponModel[] weaponModels = GetComponentsInChildren<Enemy_RangeWeaponModel>(true);    //取得所有武器模型 true是代表要包含子物件
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;    //取得武器類型

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                //依照武器模組上的type決定要切換的動畫layer
                SwitchAnimationLayer((int)weaponModel.weaponHoldType);
                return weaponModel.gameObject;
            }
        }

        Debug.LogWarning("沒有找到range武器模組");

        return null;
    }

    //取得所有腐敗水晶
    private GameObject[] CollectCorruptionCrystals()
    {
        //取得所有腐敗水晶
        Ennemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Ennemy_CorruptionCrystal>(true);
        GameObject[] corruptionCrystals = new GameObject[crystalComponents.Length];      //依照crystalComponents.Length長度設置陣列大小

        //將所有腐敗水晶加入陣列
        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }

        return corruptionCrystals;
    }

    //找出符合條件的武器(近戰)(有random邏輯去隨機取一個)
    private GameObject FindMeleeWeaponModel()
    {
        Enemy_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);    //取得所有武器模型 true是代表要包含子物件
        Enemy_MeleeWeaponType weaponType = GetComponent<Enemy_Melee>().weaponType;    //取得武器類型

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        //找出武器模組中與武器類型相同的武器
        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponModel);
            }
        }

        //符合條件的內隨機取一個
        int randomIndex = Random.Range(0, filteredWeaponModels.Count);
        return filteredWeaponModels[randomIndex].gameObject;
    }

    private void OverrideAnimatorControllerIfCan()
    {
        //如果有設定AnimatorOverrideController(不為null)
        AnimatorOverrideController overrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.overrideController;

        //則切換原先的動畫到新的動畫
        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        Animator anim = GetComponentInChildren<Animator>();

        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }

}
