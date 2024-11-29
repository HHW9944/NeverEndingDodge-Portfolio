using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private float targetDistance = 10f; // Target distance to change to
    [SerializeField] private float targetShoulderOffsetY = 2f; // Target Y-axis shoulder offset to change to
    [SerializeField] private float duration = 1f; // Duration of the tween
    [SerializeField] private Ease easeType = Ease.InOutQuad; // Choose the ease type

    private CinemachineThirdPersonFollow thirdPersonFollow;

    // Original settings for rollback
    private float originalDistance;
    private float originalShoulderOffsetY;

    private void Start()
    {
        if (virtualCamera != null)
        {
            thirdPersonFollow = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineThirdPersonFollow;
            if (thirdPersonFollow != null)
            {
                // Save the original settings at the start
                originalDistance = thirdPersonFollow.CameraDistance;
                originalShoulderOffsetY = thirdPersonFollow.ShoulderOffset.y;
            }
            else
            {
                Debug.LogError("Cinemachine3rdPersonFollow component not found on the Virtual Camera!");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera not assigned!");
        }
    }

    public void ChangeCameraSettings()
    {
        if (thirdPersonFollow == null) return;

        // Change to the new settings with the selected ease type
        DOTween.To(() => thirdPersonFollow.CameraDistance, x => thirdPersonFollow.CameraDistance = x, targetDistance, duration)
            .SetEase(easeType);

        DOTween.To(() => thirdPersonFollow.ShoulderOffset.y, y =>
        {
            var offset = thirdPersonFollow.ShoulderOffset;
            offset.y = y;
            thirdPersonFollow.ShoulderOffset = offset;
        }, targetShoulderOffsetY, duration).SetEase(easeType);
    }

    public void RollbackCameraSettings()
    {
        if (thirdPersonFollow == null) return;

        // Rollback to the original settings with the selected ease type
        DOTween.To(() => thirdPersonFollow.CameraDistance, x => thirdPersonFollow.CameraDistance = x, originalDistance, duration)
            .SetEase(easeType);

        DOTween.To(() => thirdPersonFollow.ShoulderOffset.y, y =>
        {
            var offset = thirdPersonFollow.ShoulderOffset;
            offset.y = y;
            thirdPersonFollow.ShoulderOffset = offset;
        }, originalShoulderOffsetY, duration).SetEase(easeType);
    }
}
