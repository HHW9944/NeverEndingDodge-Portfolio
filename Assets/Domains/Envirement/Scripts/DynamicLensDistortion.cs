using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicLensDistortion : MonoBehaviour
{
    public Volume postProcessingVolume;   // Post-Processing Volume 참조
    public PlayerMove playerMove;     // 플레이어의 Rigidbody (이동 감지)
    public float maxDistortion = -0.5f;   // 움직일 때 최대 Lens Distortion 값
    public float returnSpeed = 1.0f;      // 멈췄을 때 원래 값으로 돌아오는 속도

    private LensDistortion lensDistortion; // Lens Distortion 효과
    private float defaultDistortion = 0f;  // 기본 Lens Distortion 값 (정지 시)
    
    void Start()
    {
        // Lens Distortion 효과 가져오기
        if (postProcessingVolume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.value = defaultDistortion;
        }
    }

    void Update()
    {
        if (lensDistortion != null)
        {
            if (playerMove.IsMoving)
            {
                // 속도에 비례하여 Distortion 증가
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, maxDistortion, Time.deltaTime * playerMove.Velocity.sqrMagnitude);
            }
            else
            {
                // Lens Distortion을 기본 값으로 천천히 복귀
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, defaultDistortion, Time.deltaTime * returnSpeed);
            }
        }
    }
}
