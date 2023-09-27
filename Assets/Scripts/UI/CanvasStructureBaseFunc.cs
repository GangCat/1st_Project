using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStructureBaseFunc : CanvasFunc
{
    public void Init()
    {
        btnDemolishStructure.onClick.AddListener(
            () =>
            {
                ArrayStructureButtonCommand.Use(EStructureButtonCommand.DEMOLISH);
            });

        btnUpgradeStructure.onClick.AddListener(
            () =>
            {
                ArrayStructureButtonCommand.Use(EStructureButtonCommand.UPGRADE);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button btnDemolishStructure = null;
    [SerializeField]
    private Button btnUpgradeStructure = null;
}
