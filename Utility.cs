
using UnityEngine;

public static class Utility
{
    public static float DistanceToTarget(Vector3 origin, Vector3 target)
    {
        Vector3 originPos = new Vector3(origin.x, 0, origin.z);
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        return Vector3.Distance(originPos, targetPos);
    }
}