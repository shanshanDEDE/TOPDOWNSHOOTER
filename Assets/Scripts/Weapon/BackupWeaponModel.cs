using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HangType { LowBackHang, BackHang, SideHang }

public class BackupWeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    [SerializeField] private HangType hangType;

    public void Activate(bool activated) => gameObject.SetActive(activated);

    //判斷模型的type為哪個
    public bool HangTypeIs(HangType hangType) => this.hangType == hangType;
}
