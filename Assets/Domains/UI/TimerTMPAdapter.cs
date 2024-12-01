using MPUIKIT;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TimerTMPAdapter : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private TMP_Text _tmpText;

    public void UpdateTMPText(int prev, int current)
    {
        _tmpText.text = current.ToString();
    }
}