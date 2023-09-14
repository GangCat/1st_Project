using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBarrack : Building
{
    public void Init()
    {
        ArraySpawnUnitCommand.Add(EBarrackCommand.SPAWN_MELEE, new CommandSpawnUnit(this, ESpawnUnitType.MELEE));
        ArraySpawnUnitCommand.Add(EBarrackCommand.SPAWN_RANGE, new CommandSpawnUnit(this, ESpawnUnitType.RANGE));
        ArraySpawnUnitCommand.Add(EBarrackCommand.SPAWN_ROCKET, new CommandSpawnUnit(this, ESpawnUnitType.ROCKET));
    }

    public void SpawnUnit(ESpawnUnitType _unitType)
    {
        Debug.Log(_unitType);
    }
}
