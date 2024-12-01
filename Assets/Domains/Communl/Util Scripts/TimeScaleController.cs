using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
