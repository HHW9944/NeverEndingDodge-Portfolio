using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Vector3 targetDistance = new Vector3(0f, 0f, 10f);
    [SerializeField] private float duration = 1f; // Duration of the tween
    [SerializeField] private Ease easeType = Ease.InOutQuad; // Choose the ease type

    private CinemachineFollow cinemachineFollow;

    // Original settings for rollback
    private Vector3 originalDistance;

    private void Start()
    {
        if (cinemachineCamera != null)
        {
            cinemachineFollow = cinemachineCamera.GetComponent<CinemachineFollow>();
            if (cinemachineFollow != null)
            {
                // Save the original settings at the start
                originalDistance = cinemachineFollow.FollowOffset;
            }
            else
            {
                Debug.LogError("cinemachineFollow component not found on the Virtual Camera!");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera not assigned!");
        }
    }

    public void ChangeCameraSettings()
    {
        if (cinemachineFollow == null) return;

        // Change to the new settings with the selected ease type
        DOTween.To(() => cinemachineFollow.FollowOffset, v => cinemachineFollow.FollowOffset = v, targetDistance, duration).SetEase(easeType);
    }

    public void RollbackCameraSettings()
    {
        if (cinemachineFollow == null) return;

        // Rollback to the original settings with the selected ease type
        DOTween.To(() => cinemachineFollow.FollowOffset, v => cinemachineFollow.FollowOffset = v, originalDistance, duration).SetEase(easeType);
    }
}
