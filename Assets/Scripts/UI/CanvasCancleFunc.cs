using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCancleFunc : MonoBehaviour
{
    public void Init(VoidVoidDelegate _cancleBtnCallback)
    {
        cancleBtn.onClick.AddListener(
            () =>
            {
                _cancleBtnCallback?.Invoke();
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
