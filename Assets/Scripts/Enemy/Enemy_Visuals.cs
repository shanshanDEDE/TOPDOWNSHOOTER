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

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);    //取得所有武器模型 true是代表要包含子物件

        //用nameof想法直接抓方法名稱比較不會出現錯字的錯誤
        InvokeRepeating(nameof(SetupLook), 0f, 1.5f);
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
}
