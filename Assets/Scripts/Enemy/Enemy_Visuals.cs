using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy_MeleeWeaponType { OnHand, Throw }

public class Enemy_Visuals : MonoBehaviour
{
    [Header("Weapon model")]
    [SerializeField] private Enemy_WeaponModel[] weaponModels;  //武器模型
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    public GameObject currentWeaponModel { get; private set; }

    [Header("Corruption visuls(腐敗水晶相關視覺)")]
    [SerializeField] private GameObject[] corruptionCrystals;   //所有腐敗水晶
    [SerializeField] private int corruptionAmount;              //腐敗水晶數量

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {

        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);    //取得所有武器模型 true是代表要包含子物件

        //取得所有腐敗水晶
        CollectCorruptionCrystals();
    }


    //設定武器類型
    public void SetupWeaponType(Enemy_MeleeWeaponType type)
    {
        weaponType = type;
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

    //設定隨機武器
    private void SetupRandomWeapon()
    {
        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

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
        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        //顯示該武器
        currentWeaponModel.SetActive(true);
    }

    //設定隨機顏色
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);
        newMat.mainTexture = colorTextures[randomIndex];
        skinnedMeshRenderer.material = newMat;
    }

    //取得所有腐敗水晶
    private void CollectCorruptionCrystals()
    {
        //取得所有腐敗水晶
        Ennemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Ennemy_CorruptionCrystal>(true);
        corruptionCrystals = new GameObject[crystalComponents.Length];      //依照crystalComponents.Length長度設置陣列大小

        //將所有腐敗水晶加入陣列
        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }
    }
}
