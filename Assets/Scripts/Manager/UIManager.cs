using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        funcBtnMng = GetComponentInChildren<FuncButtonManager>();
        displayHUDMng = GetComponentInChildren<DisplayHUDManager>();
        funcBtnMng.Init();
        displayHUDMng.Init();
        imagePauseBack.Init();

        tempPauseButton.onClick.AddListener(
            () =>
            {
                ArrayPauseCommand.Use(EPauseCOmmand.TOGGLE_PAUSE);
            });
    }

    public void HeroDead()
    {
        displayHUDMng.HeroDead();
    }

    public void ShowFuncButton(EObjectType _selectObjectType)
    {
        funcBtnMng.ShowFuncButton(_selectObjectType);
    }

    public void UpdateEnergy(uint _curEnergy)
    {
        displayHUDMng.UpdateEnergy(_curEnergy);
    }

    public void UpdateCore(uint _curCore)
    {
        displayHUDMng.UpdateCore(_curCore);
    }

    public void UpdateCurPopulation(uint _curPopulation)
    {
        displayHUDMng.UpdateCurPopulation(_curPopulation);
    }

    public void UpdateCurMaxPopulation(uint _curMaxPopulation)
    {
        displayHUDMng.UpdateCurMaxPopulation(_curMaxPopulation);
    }

    private FuncButtonManager funcBtnMng = null;
    private DisplayHUDManager displayHUDMng = null;

    [SerializeField]
    private Button tempPauseButton = null;
    [SerializeField]
    private ImagePauseBackground imagePauseBack = null;
}
