using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [Header("Cover point")]
    [SerializeField] private GameObject coverPointPrefab;
    [SerializeField] private List<CoverPoint> coverPoints = new List<CoverPoint>();
    [SerializeField] private float xoffset = 1;
    [SerializeField] private float yoffset = 0.2f;
    [SerializeField] private float zoffset = 1;

    private void Start()
    {
        GenerateCoverPoints();
    }

    private void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints = {
            new Vector3(0, yoffset, zoffset),       //前
            new Vector3(0, yoffset, -zoffset),      //後
            new Vector3(xoffset, yoffset, 0),       //右
            new Vector3(-xoffset, yoffset, 0)       //左
        };


        foreach (Vector3 localPoint in localCoverPoints)
        {
            //從局部座標轉換為世界座標
            Vector3 worldPoint = transform.TransformPoint(localPoint);
            CoverPoint coverPoint = Instantiate(coverPointPrefab, worldPoint, Quaternion.identity).GetComponent<CoverPoint>();

            coverPoints.Add(coverPoint);
        }
    }

    public List<CoverPoint> GetCoverPoints() => coverPoints;

}
