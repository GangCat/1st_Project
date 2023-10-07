using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArea : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void SetPos(Vector3 _pos)
    {
        transform.position = _pos;
    }

    public void SetLocalScale(Vector3 _scale)
    {
        _scale.y = 1f;
        transform.localScale = _scale;
    }

    public void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }

    private void OnTriggerEnter(Collider _other)
    {
        SelectableObject sObj = _other.GetComponent<SelectableObject>();
        if (sObj != null)
            ArraySelectCommand.Use(ESelectCommand.TEMP_SELECT, sObj);
    }

    private void OnTriggerExit(Collider _other)
    {
        SelectableObject sObj = _other.GetComponent<SelectableObject>();
        if (sObj != null)
            ArraySelectCommand.Use(ESelectCommand.TEMP_UNSELECT, sObj);
    }

}
