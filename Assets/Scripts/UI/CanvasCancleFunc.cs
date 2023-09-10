using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCancleFunc : MonoBehaviour
{
    public void Init()
    {
        cancleBtn.onClick.AddListener(
            () =>
            {
                ListUnitCommand.Use(0);
                gameObject.SetActive(false);
            });
        gameObject.SetActive(false);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    [SerializeField]
    private Button cancleBtn = null;
}
