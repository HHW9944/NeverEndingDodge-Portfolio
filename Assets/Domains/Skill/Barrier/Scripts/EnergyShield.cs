using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyShield : MonoBehaviour
{
    [SerializeField] private VisualEffect _sparks;
    public float RippleCooldown = 0.4f;

    private Material _material;
    private float _rippleTime = 100.0f;
    private Coroutine _routine;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _sparks.enabled = false;
    }

    public void GetHit(RaycastHit hit)
    {
        if (_rippleTime < RippleCooldown)
        {
            return;
        }

        _material.SetVector("_RippleOrigin", hit.textureCoord);
        _rippleTime = _material.GetFloat("_RippleThickness") * -2.0f;

        if (_routine != null)
        {
            StopCoroutine(_routine);
        }
        
        _routine = StartCoroutine(Shake(hit));
    }

    public void GetHit(Collision other)
    {
        if (_rippleTime < RippleCooldown)
        {
            return;
        }

        // 첫 번째 ContactPoint를 기준으로 파동 효과를 적용할 위치 설정 (월드 좌표)
        Vector3 rippleOrigin = other.contacts[0].point;

        // 필요한 경우 파동 효과를 위한 로직 추가 (예시로 월드 좌표 사용)
        _material.SetVector("_RippleOriginWorld", rippleOrigin); // 셰이더에 월드 좌표를 사용할 경우 필요
        _rippleTime = _material.GetFloat("_RippleThickness") * -2.0f;

        if (_routine != null)
        {
            StopCoroutine(_routine);
        }

        // 첫 번째 ContactPoint를 Shake 코루틴으로 전달
        _routine = StartCoroutine(Shake(other.contacts[0]));
    }


    private void Update()
    {
        _rippleTime += Time.deltaTime;
        _material.SetFloat("_RippleTime", _rippleTime);
    }

    private IEnumerator Shake(RaycastHit hit)
    {
        _sparks.transform.position = hit.point;
        _sparks.transform.rotation = Quaternion.LookRotation(Vector3.up, hit.normal);
        _sparks.enabled = true;
        _sparks.Play();

        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator Shake(ContactPoint contact)
    {
        _sparks.transform.position = contact.point;
        _sparks.transform.rotation = Quaternion.LookRotation(contact.normal);
        _sparks.enabled = true;
        _sparks.Play();

        yield return new WaitForSeconds(0.1f);
    }
}
