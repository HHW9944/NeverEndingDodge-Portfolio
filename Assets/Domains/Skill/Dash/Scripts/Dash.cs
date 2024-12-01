using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Dash : Skill
{
    [Header("Dash Settings")]
    [Tooltip("대시 Skill이 소모하는 Cost")]
    public float Cost = 20f;

    [Tooltip("대시의 힘 또는 속도")]
    public float Force = 20f;

    [Tooltip("대시 재사용 대기 시간")]
    public float Cooldown = 1f;

    [Header("Trail Settings")]
    [Tooltip("Trail의 활성화 시간")]
    public float TrailDuration = 0.5f;

    [Tooltip("Trail의 활성화 간격")]
    public float MeshRefreshRate = 0.1f;
    [SerializeField] private Material _trailMaterial;
    [SerializeField] private string _propertyName = "_Alpha";
    [SerializeField] private float _endValue = 0.1f;
    [SerializeField] private float _refreshRate = 0.05f;

    [Header("References")]
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private Cost _playerCost;

    [Header("Event")]
    public UnityEvent OnUse;
    public UnityEvent OnEnd;

    private float _cooldownTimer = 0f;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    private void Update()
    {
        // 쿨다운 시간 업데이트
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    public override void UseSkill()
    {
        // 쿨다운 중이거나 코스트가 부족하면 스킬 사용 불가
        if (_cooldownTimer > 0f || _playerCost.Value < Cost)
        {
            return;
        }

        _playerCost.UseCost(Cost);
        StartDash();
    }

    public override float GetCost()
    {
        return Cost;
    }

    private void StartDash()
    {
        Vector3 movementDirection = _playerMove.GetMovementDirection();

        if (movementDirection == Vector3.zero)
        {
            return;
        }
        else
        {
            movementDirection = _player.transform.rotation * movementDirection.normalized;
        }

        // 대시 시작
        _cooldownTimer = Cooldown;
        _playerMove.AddForce(movementDirection * Force, ForceMode.Impulse);
        OnUse?.Invoke();
        StartCoroutine(ActivateTrail(TrailDuration));
    }

    private System.Collections.IEnumerator ActivateTrail(float timeActive)
    {
        if (_skinnedMeshRenderers == null)
        {
            _skinnedMeshRenderers = _player.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        while (timeActive > 0)
        {
            foreach (var meshRenderer in _skinnedMeshRenderers)
            {
                GameObject trailObject = new GameObject("TrailObject");
                trailObject.transform.position = meshRenderer.transform.position;
                trailObject.transform.rotation = meshRenderer.transform.rotation;

                MeshRenderer renderer = trailObject.AddComponent<MeshRenderer>();
                MeshFilter filter = trailObject.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                meshRenderer.BakeMesh(mesh);

                filter.mesh = mesh;
                renderer.material = _trailMaterial;

                StartCoroutine(AnimatedMaterialFloat(renderer.material, 0f, _endValue, _refreshRate));

                Destroy(trailObject, TrailDuration);
            }

            timeActive -= MeshRefreshRate;
            yield return new WaitForSeconds(MeshRefreshRate);
        }
    }

    private IEnumerator AnimatedMaterialFloat(Material material, float endValue, float rate, float refreshRate)
    {
        float valueToAnimated = material.GetFloat(_propertyName);
        while (valueToAnimated > endValue)
        {
            valueToAnimated -= rate;
            material.SetFloat(_propertyName, valueToAnimated);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}