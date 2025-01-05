using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Visuals : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        //用nameof想法直接抓方法名稱比較不會出現錯字的錯誤
        InvokeRepeating(nameof(SetupLook), 0f, 1.5f);
    }

    public void SetupLook()
    {
        SetupRandomColor();
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
