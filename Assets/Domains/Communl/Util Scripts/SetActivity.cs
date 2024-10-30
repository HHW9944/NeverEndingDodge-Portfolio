using UnityEngine;

public class SetActivity : MonoBehaviour
{
    public GameObject[] objects;

    public void SetEnable()
    {
        foreach (var obj in objects)
        {
            obj.SetActive(true);
        }
    }

    public void SetDisable()
    {
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
    }
}
