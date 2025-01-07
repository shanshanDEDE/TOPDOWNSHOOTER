using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Cover point")]
    [SerializeField] private GameObject coverPointPrefab;
    [SerializeField] private List<CoverPoint> coverPoints = new List<CoverPoint>();
    [SerializeField] private float xoffset = 1;
    [SerializeField] private float yoffset = 0.2f;
    [SerializeField] private float zoffset = 1;

    private void Start()
    {
        GenerateCoverPoints();
        playerTransform = FindObjectOfType<Player>().transform;
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

    //取得有效的Cover
    public List<CoverPoint> GetValidCoverPoints(Transform enemy)
    {
        List<CoverPoint> validCoverPoint = new List<CoverPoint>();

        foreach (CoverPoint coverPoint in coverPoints)
        {
            //判斷是否為有效的Cover
            if (IsValidCoverPoint(coverPoint, enemy))
            {
                validCoverPoint.Add(coverPoint);
            }
        }
        return validCoverPoint;
    }

    //判斷是否為有效的Cover
    private bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemy)
    {
        //判斷是否已經被佔領
        if (coverPoint.occupied)
        {
            return false;
        }

        //檢查該掩體那個coverpint離玩家最遠
        if (IsFutherestFromPlayer(coverPoint) == false)
        {
            return false;
        }

        //是否在玩家附近
        if (IsCoverCloseToPlayer(coverPoint))
        {
            return false;
        }

        //是否在玩家後面(判斷cover離誰比較近)
        if (IsCoverBehindPlayer(coverPoint, enemy))
        {
            return false;
        }

        //檢查是否離上一個cover太近
        if (IsCoverCloseToLastCover(coverPoint, enemy))
        {
            return false;
        }

        return true;
    }

    //檢查該掩體那個coverpint離玩家最遠
    private bool IsFutherestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint futherestPoint = null;
        float futherestDistance = 0;

        foreach (CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, playerTransform.transform.position);
            if (distance > futherestDistance)
            {
                futherestDistance = distance;
                futherestPoint = point;
            }
        }

        return futherestPoint == coverPoint;
    }

    //是否在玩家後面(判斷cover離誰比較近)
    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemy)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, playerTransform.position);
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemy.position);
        return distanceToPlayer < distanceToEnemy;
    }

    //是否在玩家附近
    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Vector3.Distance(coverPoint.transform.position, playerTransform.transform.position) < 2;
    }

    //檢查是否離上一個cover太近
    private bool IsCoverCloseToLastCover(CoverPoint coverPoint, Transform enemy)
    {
        CoverPoint lastCover = enemy.GetComponent<Enemy_Range>().currentCover;
        return lastCover != null && Vector3.Distance(coverPoint.transform.position, lastCover.transform.position) < 3;
    }
}
