using UnityEngine;

public class Cost : MonoBehaviour
{
    [Tooltip("재생되는 Cost 양")]
    public float CostRegen = 0.1f;

    [Tooltip("Cost의 최대치")]
    public float MaxCost = 100f;

    private float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            // _value를 0 이상 MaxCost 이하로 제한
            _value = Mathf.Clamp(value, 0f, MaxCost);
        }
    }

    void Start()
    {
        _value = MaxCost;
    }

    void Update()
    {
        RegenerateCost();
    }

    private void RegenerateCost()
    {
        if (Value >= MaxCost)
        {
            return;
        }

        Value += CostRegen * Time.deltaTime;
    }
}
