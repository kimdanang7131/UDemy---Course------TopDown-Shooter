using System;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Cover points")]
    [SerializeField] private GameObject coverPointPrefab;
    [SerializeField] private List<CoverPoint> coverPoints = new List<CoverPoint>();
    [SerializeField] private float xOffset = 1;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private float zOffset = 1;

    void Start()
    {
        GenerateCoverPoints();
        playerTransform = FindAnyObjectByType<Player>().transform;
    }

    private void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints =
        {
            new Vector3(0, yOffset, zOffset), // Front
            new Vector3(0, yOffset, -zOffset), // Back
            new Vector3(xOffset, yOffset, 0), // Right
            new Vector3(-xOffset, yOffset, 0) // Left
        };

        foreach (Vector3 localPoint in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoint);
            CoverPoint coverPoint = Instantiate(coverPointPrefab, worldPoint, Quaternion.identity, transform).GetComponent<CoverPoint>();

            coverPoints.Add(coverPoint);
        }
    }

    public List<CoverPoint> GetValidCoverPoints(Transform enemy)
    {
        List<CoverPoint> validCoverPoints = new List<CoverPoint>();

        foreach (CoverPoint coverPoint in coverPoints)
        {
            if (IsValidCoverPoint(coverPoint, enemy))
                validCoverPoints.Add(coverPoint);
        }

        return validCoverPoints;
    }

    private bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemy)
    {
        if (coverPoint.occupied)
            return false;

        if (IsFurtherestFromPlayer(coverPoint) == false)
            return false;

        // 너무 플레이어 가까이에 있다면 제외 
        if (IsCoverCloseToPlayer(coverPoint))
            return false;

        // CoverPoint기준 적보다 플레이어가 더 가까우면 false
        if (IsCoverBehindPlayer(coverPoint, enemy))
            return false;

        if (IsCoverCloseToLastCover(coverPoint, enemy))
            return false;

        return true;
    }

    // 가장 플레이어로부터 먼 커버 포인트인지 확인
    private bool IsFurtherestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint furtherestPoint = null;
        float furtherestDistance = 0;

        foreach (var point in coverPoints)
        {
            float distance = Utility.DistanceToTarget(point.transform.position, playerTransform.position);
            if (distance > furtherestDistance)
            {
                furtherestDistance = distance;
                furtherestPoint = point;
            }
        }

        return furtherestPoint == coverPoint;
    }

    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Utility.DistanceToTarget(coverPoint.transform.position, playerTransform.position) < 1;
    }

    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemy)
    {
        float distanceToPlayer = Utility.DistanceToTarget(coverPoint.transform.position, playerTransform.position);
        float distanceToEnemy = Utility.DistanceToTarget(coverPoint.transform.position, enemy.position);

        return distanceToPlayer < distanceToEnemy;
    }


    private bool IsCoverCloseToLastCover(CoverPoint coverPoint, Transform enemy)
    {
        CoverPoint currentCoverPoint = enemy.GetComponent<Enemy_Range>().currentCoverPoint;
        return currentCoverPoint != null && Utility.DistanceToTarget(coverPoint.transform.position, enemy.position) < 3;
    }
}
