using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class CostManager : MonoBehaviour
{
    [SerializeField] private Cost _playerCost;
    [SerializeField] private GameObject _barPrefab;
    [SerializeField] private Transform _barContainer;

    [Header("Tweening Setting")]
    public float TweenDuration = 0.5f;
    public Ease TweenEase = Ease.Linear;

    private List<GameObject> _costBars = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < _playerCost.MaxCost; i++)
        {
            GameObject lifeBar = Instantiate(_barPrefab, _barContainer);
            lifeBar.SetActive(true);
            _costBars.Add(lifeBar);
        }
    }

    public void UpdateImage(float prev, float current)
    {
        if ((int) prev == (int) current) return;
        // 만약 체스트가 감소한거면, current부터 prev까지 줄어든다.
        // 만약 체스트가 증가한거면, current부터 prev까지 증가한다.

        // 체스트 감소
        if (prev > current)
        {
            for (int i = (int)current; i < (int)prev; i++)
            {
                _costBars[i].SetActive(false);
            }
        }
        else
        {
            for (int i = (int)prev; i < (int)current; i++)
            {
                _costBars[i].SetActive(true);
            }
        }
    }
}