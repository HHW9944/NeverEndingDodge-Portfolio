using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private Life _playerLife;
    [SerializeField] private GameObject _barPrefab;
    [SerializeField] private Transform _barContainer;

    [Header("Tweening Setting")]
    public float TweenDuration = 0.5f;
    public Ease TweenEase = Ease.Linear;

    private List<GameObject> _lifeBars = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < _playerLife.MaxValue; i++)
        {
            GameObject lifeBar = Instantiate(_barPrefab, _barContainer);
            lifeBar.SetActive(true);
            _lifeBars.Add(lifeBar);
        }
    }

    public void UpdateImage(int prev, int current)
    {
        if (prev == current) return;
        // 만약 체력이 감소한거면, current부터 prev까지 줄어든다.
        // 만약 체력이 증가한거면, current부터 prev까지 증가한다.

        // 체력 감소
        if (prev > current)
        {
            for (int i = current; i < prev; i++)
            {
                _lifeBars[i].SetActive(false);
            }
        }
        else
        {
            for (int i = prev; i < current; i++)
            {
                _lifeBars[i].SetActive(true);
            }
        }
    }
}