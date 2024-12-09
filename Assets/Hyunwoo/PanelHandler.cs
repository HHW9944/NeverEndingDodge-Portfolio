using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelHandler : MonoBehaviour
{
    public float maxSize = 2.5f;
    public float reachSize = 2.2f;
    public float time1 = 0.5f;
    public float time2 = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        transform.localScale = Vector3.one * 0.1f;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Show()
    {
        gameObject.SetActive(true);

        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(maxSize, time1));
        seq.Append(transform.DOScale(reachSize, time2));

        seq.Play();
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        transform.localScale = Vector3.one * 0.2f;

        seq.Append(transform.DOScale(1.1f, 0.2f));
        seq.Append(transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
