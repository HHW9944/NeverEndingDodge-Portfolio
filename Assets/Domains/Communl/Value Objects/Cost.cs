using UnityEngine;

public class Cost : MonoBehaviour
{
    [Tooltip("초당 재생되는 Cost 양 (CPS)")]
    public float CostRegen = 1f;

    [Tooltip("Cost의 최대치")]
    public float MaxCost = 100f;

    [Tooltip("디버그 모드 On/Off")]
    public bool IsDebug = false;

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

    public void UseCost(float cost)
    {
        Value -= cost;
    }

    private void RegenerateCost()
    {
        if (Value >= MaxCost)
        {
            return;
        }

        Value += CostRegen * Time.deltaTime;

        if (IsDebug)
        {
            Debug.Log("Cost : " + Value);
        }
    }
}
