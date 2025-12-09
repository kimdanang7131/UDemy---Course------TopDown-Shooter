using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachinePositionComposer composer;

    [Header("Camera distance")]
    [SerializeField] private bool canChangeCameraDistance;
    [SerializeField] private float distanceChangeRate;
    private float targetCamreaDistance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        composer = GetComponentInChildren<CinemachinePositionComposer>();
    }

    void Update()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistance()
    {
        if (canChangeCameraDistance == false)
            return;

        float currentDistance = composer.CameraDistance;

        if (Mathf.Abs(targetCamreaDistance - currentDistance) < .01f)
            return;

        composer.CameraDistance = Mathf.Lerp(composer.CameraDistance, targetCamreaDistance, distanceChangeRate * Time.deltaTime);
    }

    public void ChangeCameraDistance(float distance) => targetCamreaDistance = distance;

}
