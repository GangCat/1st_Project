using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectManager : MonoBehaviour, IPublisher
{
    public void Init(VoidSelectObjectTypeDelegate _selectObjectCallback, PF_Grid _grid)
    {
        listSelectedFriendlyObject.Clear();
        tempListSelectableObject.Clear();
        selectObjectCallback = _selectObjectCallback;
        grid = _grid;
        RegisterBroker();

        unitInfoContainer = new UnitInfoContainer();
        listFriendlyUnitInfo = new List<SFriendlyUnitInfo>(12);
        arrMemoryPool = new MemoryPool[arrUnitPrefab.Length];

        for (int i = 0; i < listFriendlyUnitInfo.Capacity; ++i)
            listFriendlyUnitInfo.Add(new SFriendlyUnitInfo());
        for (int i = 0; i < arrUnitPrefab.Length; ++i)
            arrMemoryPool[i] = new MemoryPool(arrUnitPrefab[i], 5, transform);

        arrCrowd = new List<FriendlyObject>[9];
        for(int i = 0; i < arrCrowd.Length; ++i)
            arrCrowd[i] = new List<FriendlyObject>();


        ArrayHUDCommand.Use(EHUDCommand.INIT_DISPLAY_GROUP_INFO, listFriendlyUnitInfo);
        ArrayHUDCommand.Use(EHUDCommand.INIT_DISPLAY_SINGLE_INFO, unitInfoContainer);
    }

    public delegate void VoidSelectObjectTypeDelegate(EObjectType _objectType);
    public bool IsFriendlyUnit => isFriendlyUnitInList;
    public static bool IsListEmpty => listSelectedFriendlyObject.Count < 1;
    public static int LevelRangedUnitDmgUpgrade => levelRangedUnitDmgUpgrade;
    public static int LevelRangedUnitHpUpgrade => levelRangedUnitHpUpgrade;
    public static int LevelMeleeUnitHpUpgrade => levelMeleeUnitHpUpgrade;
    public static int LevelMeleeUnitDmgUpgrade => levelMeleeUnitDmgUpgrade;
    public static float DelayUnitUpgrade => delayUnitUpgrade;
    public static Dictionary<int, PF_Node> DicNodeUnderFriendlyUnit => dicNodeUnderFriendlyUnit;
    public static Dictionary<int, PF_Node> DicNodeUnderEnemyUnit => dicNodeUnderEnemyUnit;

    public static FriendlyObject GetFirstSelectedObjectInList()
    {
        if (listSelectedFriendlyObject.Count > 0)
            return listSelectedFriendlyObject[0];
        else
            return null;
    }

    public static void InitNodeFriendly(Vector3 _pos, out int _idx)
    {
        dicNodeUnderFriendlyUnit.Add(dicFriendlyIdx, grid.GetNodeFromWorldPoint(_pos));
        _idx = dicFriendlyIdx;
        ++dicFriendlyIdx;
    }

    public static void InitNodeEnemy(Vector3 _pos, out int _idx)
    {
        dicNodeUnderEnemyUnit.Add(dicEnemyIdx, grid.GetNodeFromWorldPoint(_pos));
        _idx = dicEnemyIdx;
        ++dicEnemyIdx;
    }

    public static void UpdateFriendlyNodeWalkable(Vector3 _pos, int _idx)
    {
        PF_Node prevNode = null;
        if (dicNodeUnderFriendlyUnit.TryGetValue(_idx, out prevNode))
            prevNode.walkable = true;

        PF_Node curNode = grid.GetNodeFromWorldPoint(_pos);
        curNode.walkable = false;
        dicNodeUnderFriendlyUnit[_idx] = curNode;
    }

    public static void UpdateEnemyNodeWalkable(Vector3 _pos, int _idx)
    {
        PF_Node prevNode = null;
        if (dicNodeUnderEnemyUnit.TryGetValue(_idx, out prevNode))
            prevNode.walkable = true;

        PF_Node curNode = grid.GetNodeFromWorldPoint(_pos);
        curNode.walkable = false;
        dicNodeUnderEnemyUnit[_idx] = curNode;
    }

    public static void ResetHeroUnitNode(Vector3 _pos)
    {
        dicNodeUnderFriendlyUnit[0].walkable = true;
        grid.GetNodeFromWorldPoint(_pos).walkable = true;
    }

    public static void ResetFriendlyNodeWalkable(Vector3 _pos, int _idx)
    {
        dicNodeUnderFriendlyUnit[_idx].walkable = true;
        grid.GetNodeFromWorldPoint(_pos).walkable = true;
        dicNodeUnderFriendlyUnit.Remove(_idx);
    }

    public static void ResetEnemyNodeWalkable(Vector3 _pos, int _idx)
    {
        dicNodeUnderEnemyUnit[_idx].walkable = true;
        grid.GetNodeFromWorldPoint(_pos).walkable = true;
        dicNodeUnderEnemyUnit.Remove(_idx);
    }

    public static Vector3 ResetPosition(Vector3 _pos)
    {
        PF_Node unitNode = grid.GetNodeFromWorldPoint(_pos);

        if (!grid.GetNodeFromWorldPoint(_pos).walkable)
            return grid.GetAccessibleNodeWithoutTargetNode(unitNode).worldPos;

        return unitNode.worldPos;
    }

    public void SetListCrowd(int _arrIdx)
    {
        arrCrowd[_arrIdx].AddRange(listSelectedFriendlyObject.ToArray());
    }

    public void SelectStart()
    {
        for (int i = 0; i < listSelectedFriendlyObject.Count; ++i)
            listSelectedFriendlyObject[i].DestroyCircle();
    }

    public void RemoveUnitAtList(FriendlyObject _removeObj)
    {
        if (_removeObj == null) return;

        bool isDeleted = false;

        for (int i = 0; i < listSelectedFriendlyObject.Count;)
        {
            if (isDeleted)
                listSelectedFriendlyObject[i].UpdatelistIdx(i);
            else if (listSelectedFriendlyObject[i].Equals(_removeObj))
            {
                listSelectedFriendlyObject[i].unSelect();
                listSelectedFriendlyObject.RemoveAt(i);
                isDeleted = true;
                continue;
            }
            ++i;
        }

        UpdateInfo();
    }

    public void InUnit(FriendlyObject _friObj)
    {
        curBunker.InUnit(_friObj);
    }

    public void OutOneUnit()
    {
        if (listSelectedFriendlyObject[0].GetObjectType().Equals(EObjectType.BUNKER))
            listSelectedFriendlyObject[0].GetComponent<StructureBunker>().OutOneUnit();
    }

    public void OutAllUnit()
    {
        if (listSelectedFriendlyObject[0].GetObjectType().Equals(EObjectType.BUNKER))
            listSelectedFriendlyObject[0].GetComponent<StructureBunker>().OutAllUnit();
    }

    public bool CanSpawnunit()
    {
        if (listSelectedFriendlyObject[0].GetObjectType().Equals(EObjectType.BARRACK))
            return listSelectedFriendlyObject[0].GetComponent<StructureBarrack>().CanSpawnUnit();
        else 
            return false;
    }

    public void RequestSpawnUnit(EUnitType _unitType)
    {
        if (listSelectedFriendlyObject[0].GetObjectType().Equals(EObjectType.BARRACK))
            listSelectedFriendlyObject[0].GetComponent<StructureBarrack>().StartSpawnUnit(_unitType);
    }

    public void SpawnUnit(EUnitType _unitType, Vector3 _spawnPos, Vector3 _rallyPoint, Transform _rallyTr = null)
    {
        FriendlyObject tempObj = arrMemoryPool[(int)_unitType].ActivatePoolItem(_spawnPos, 5, transform).GetComponent<FriendlyObject>();
        tempObj.Position = ResetPosition(tempObj.Position);
        tempObj.Init();

        if (_rallyTr != null)
            tempObj.FollowTarget(_rallyTr);
        else if (!_rallyPoint.Equals(_spawnPos))
            tempObj.MoveByPos(_rallyPoint);
    }

    public void DeactivateUnit(GameObject _removeGo, EUnitType _unitType, FriendlyObject _fObj)
    {
        arrMemoryPool[(int)_unitType].DeactivatePoolItem(_removeGo);
        RemoveUnitAtList(_fObj);
    }

    public void SetRallyPoint(Vector3 _pos)
    {
        listSelectedFriendlyObject[0].GetComponent<StructureBarrack>().SetRallyPoint(_pos);
    }

    public void SetRallyPoint(Transform _targetTr)
    {
        listSelectedFriendlyObject[0].GetComponent<StructureBarrack>().SetRallyPoint(_targetTr);
    }

    public void AddSelectedObject(SelectableObject _object)
    {
        if (_object.IsTempSelect) return;

        tempListSelectableObject.Add(_object);
        _object.IsTempSelect = true;
        _object.DisplayCircle();
    }

    public void RemoveSelectedObject(SelectableObject _object)
    {
        tempListSelectableObject.Remove(_object);
        _object.IsTempSelect = false;
        _object.DestroyCircle();
    }

    public void SelectFinish()
    {
        if (tempListSelectableObject.Count < 1)
        {
            for (int i = 0; i < listSelectedFriendlyObject.Count; ++i)
                listSelectedFriendlyObject[i].DisplayCircle();
            return;
        }

        foreach (FriendlyObject obj in listSelectedFriendlyObject)
            obj.unSelect();

        for (int i = 0; i < tempListSelectableObject.Count; ++i)
        {
            tempListSelectableObject[i].IsTempSelect = false;
            tempListSelectableObject[i].DestroyCircle();
        }

        listSelectedFriendlyObject.Clear();
        ArrayHUDCommand.Use(EHUDCommand.HIDE_ALL_INFO);
        SelectableObject tempObj = null;
        isFriendlyUnitInList = false;
        isFriendlyStructureInList = false;
        isEnemyObjectInList = false;
        // 오브젝트를 하나하나 검사.
        foreach (SelectableObject obj in tempListSelectableObject)
        {
            if (obj == null) continue;
            obj.DestroyCircle();

            if (listSelectedFriendlyObject.Count > 11) break;

            switch (obj.GetObjectType())
            {
                case EObjectType.UNIT_01:
                case EObjectType.UNIT_02:
                case EObjectType.UNIT_HERO:
                    if (!isFriendlyUnitInList)
                    {
                        listSelectedFriendlyObject.Add(obj.GetComponent<FriendlyObject>());
                        isFriendlyUnitInList = true;
                        isFriendlyStructureInList = false;
                        isEnemyObjectInList = false;
                    }
                    else
                        listSelectedFriendlyObject.Add(obj.GetComponent<FriendlyObject>());
                    break;
                case EObjectType.MAIN_BASE:
                case EObjectType.TURRET:
                case EObjectType.BUNKER:
                case EObjectType.WALL:
                case EObjectType.BARRACK:
                case EObjectType.NUCLEAR:
                    if (isFriendlyUnitInList) break;
                    isFriendlyStructureInList = true;
                    isEnemyObjectInList = false;
                    if (!tempObj) tempObj = obj;
                    break;
                case EObjectType.ENEMY_UNIT:
                case EObjectType.ENEMY_STRUCTURE:
                    if (isFriendlyUnitInList) break;
                    if (isFriendlyStructureInList) break;
                    isEnemyObjectInList = true;
                    if (!tempObj) tempObj = obj;
                    break;
                default:
                    break;
            }
        }

        // 임시 리스트에 적 유닛만 있을 경우
        if (isEnemyObjectInList)
        {
            tempObj.DisplayCircle();
            selectObjectCallback?.Invoke(tempObj.GetObjectType());
            InputOtherUnitInfo(tempObj);
            ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
        }
        // 임시 리스트에 아군 건물만 있을 경우
        else if (isFriendlyStructureInList)
        {
            tempObj.DisplayCircle();
            InputOtherUnitInfo(tempObj);
            listSelectedFriendlyObject.Add(tempObj.GetComponent<FriendlyObject>());
            Structure structureInList = tempObj.GetComponent<Structure>();
            // 아군 건물이 건설중인 건물일 경우
            if (structureInList.IsProcessingConstruct)
            {
                selectObjectCallback?.Invoke(EObjectType.NONE);
                structureInList.UpdateConstructInfo();
                ArrayHUDConstructCommand.Use(EHUDConstructCommand.DISPLAY_CONSTRUCT_INFO);
                ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
            }
            // 아군 건물이 업그레이드중일 경우
            else if (structureInList.IsProcessingUpgrade)
            {
                selectObjectCallback?.Invoke(EObjectType.NONE);
                structureInList.UpdateUpgradeInfo();
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.DISPLAY_UPGRADE_INFO, structureInList.CurUpgradeType);
                ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
            }
            // 아군 건물이 해체중일 경우
            else if (structureInList.IsProcessingDemolish)
            {
                selectObjectCallback?.Invoke(EObjectType.NONE);
                structureInList.UpdateDemolishInfo();
                ArrayHUDConstructCommand.Use(EHUDConstructCommand.DISPLAY_DEMOLISH_INFO);
                ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
            }
            // 아군 건물이 배럭일 때
            else if (tempObj.GetObjectType().Equals(EObjectType.BARRACK))
            {
                StructureBarrack tempBarrack = tempObj.GetComponent<StructureBarrack>();
                selectObjectCallback?.Invoke(EObjectType.BARRACK);
                // 배럭이 유닛을 생산중일 경우
                if (tempBarrack.IsProcessingSpawnUnit)
                {
                    tempBarrack.UpdateSpawnInfo();
                    ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.DISPLAY_SPAWN_UNIT_INFO);
                    ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                }
                else
                {
                    InputOtherUnitInfo(tempObj);
                    ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
                }
            }
            // 아군 건물이 핵 생산 건물일 경우
            else if(tempObj.GetObjectType().Equals(EObjectType.NUCLEAR))
            {
                StructureNuclear tempNuclear = listSelectedFriendlyObject[0].GetComponent<StructureNuclear>();

                // 핵을 생산중일 경우
                if (tempNuclear.IsProcessingSpawnNuclear)
                {
                    tempNuclear.UpdateSpawnNuclearInfo();
                    ArrayHUDSpawnNuclearCommand.Use(EHUDSpawnNuclearCommand.DISPLAY_SPAWN_NUCLEAR_INFO);
                    ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                }
                else
                {
                    selectObjectCallback?.Invoke(EObjectType.NUCLEAR);
                    InputOtherUnitInfo(tempObj);
                    ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
                }
            }
            // 아무것도 하지 않는 상태의 건물일 경우
            else
            {
                selectObjectCallback?.Invoke(tempObj.GetObjectType());
                InputOtherUnitInfo(tempObj);
                ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
            }
        }
        // 임시 리스트에 아군 유닛이 존재할 경우
        else if (isFriendlyUnitInList)
        {
            selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
            for (int i = 0; i < listSelectedFriendlyObject.Count; ++i)
                listSelectedFriendlyObject[i].DisplayCircle();
            // 아군 유닛 1마리만 존재할 경우
            if (listSelectedFriendlyObject.Count < 2)
            {
                InputOtherUnitInfo(listSelectedFriendlyObject[0]);
                ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
            }
            else
            {
                InputFriendlyUnitInfo();
                ArrayHUDCommand.Use(EHUDCommand.DISPLAY_GROUP_INFO, listSelectedFriendlyObject.Count);
            }
        }

        tempListSelectableObject.Clear();
        return;
    }

    public void UpdateInfo()
    {
        ArrayHUDCommand.Use(EHUDCommand.HIDE_ALL_INFO);
        // 리스트가 비어있지 않을 경우
        if (listSelectedFriendlyObject.Count > 0)
        {
            if (isFriendlyStructureInList)
            {
                Structure curStructure = listSelectedFriendlyObject[0].GetComponent<Structure>();
                // 해당 건물이 현재 업그레이드를 진행중일 경우
                if (curStructure.IsProcessingUpgrade)
                {
                    curStructure.UpdateUpgradeInfo();
                    selectObjectCallback?.Invoke(EObjectType.NONE);
                    ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.DISPLAY_UPGRADE_INFO, curStructure.CurUpgradeType);
                    ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                }
                // 해당 건물이 현재 해체중일 경우
                else if (curStructure.IsProcessingDemolish)
                {
                    curStructure.UpdateDemolishInfo();
                    selectObjectCallback?.Invoke(EObjectType.NONE);
                    ArrayHUDConstructCommand.Use(EHUDConstructCommand.DISPLAY_DEMOLISH_INFO, curStructure);
                    ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                }
                // 해당 건물이 현재 건설중일 경우
                else if (curStructure.IsProcessingConstruct)
                {
                    curStructure.UpdateConstructInfo();
                    selectObjectCallback?.Invoke(EObjectType.NONE);
                    ArrayHUDConstructCommand.Use(EHUDConstructCommand.DISPLAY_CONSTRUCT_INFO, curStructure);
                    ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                }
                // 그렇지 않다면
                else
                {
                    StructureBarrack tempBarrack = listSelectedFriendlyObject[0].GetComponent<StructureBarrack>();
                    StructureNuclear tempNuclear = listSelectedFriendlyObject[0].GetComponent<StructureNuclear>();
                    // 만일 건물이 배럭이고 생산중이라면
                    if (tempBarrack != null && tempBarrack.IsProcessingSpawnUnit)
                    {
                        selectObjectCallback?.Invoke(EObjectType.BARRACK);
                        tempBarrack.UpdateSpawnInfo();
                        ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.DISPLAY_SPAWN_UNIT_INFO);
                        ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                    }
                    // 만일 건물이 핵 건물이고 핵 생산중이라면
                    else if(tempNuclear != null && tempNuclear.IsProcessingSpawnNuclear)
                    {
                        selectObjectCallback?.Invoke(EObjectType.NUCLEAR);
                        tempNuclear.UpdateSpawnNuclearInfo();
                        ArrayHUDSpawnNuclearCommand.Use(EHUDSpawnNuclearCommand.DISPLAY_SPAWN_NUCLEAR_INFO);
                        ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
                    }
                    else
                    {
                        selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
                        InputOtherUnitInfo(listSelectedFriendlyObject[0]);
                        ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
                    }
                }
            }
            // 리스트에 아군 유닛 존재할 경우
            else if (isFriendlyUnitInList)
            {
                // 리스트에 유닛이 하나만 존재할 경우
                if (listSelectedFriendlyObject.Count < 2)
                {
                    InputOtherUnitInfo(listSelectedFriendlyObject[0]);
                    ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
                }
                // 리스트에 유닛이 다수 존재할 경우
                else
                {
                    InputFriendlyUnitInfo();
                    ArrayHUDCommand.Use(EHUDCommand.DISPLAY_GROUP_INFO, listSelectedFriendlyObject.Count);
                }
                selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
            }
            // 리스트에 아군 건물이 존재할 경우

        }
        // 리스트가 비어있거나 적 유닛만 존재할 경우
        else
        {
            ArrayHUDCommand.Use(EHUDCommand.HIDE_UNIT_INFO);
            selectObjectCallback?.Invoke(EObjectType.NONE);
        }
    }

    public static void UpdateHp(int _listIdx = -2)
    {
        if (_listIdx.Equals(-2))
            unitInfoContainer.curHpPercent = listSelectedFriendlyObject[0].GetCurHpPercent;
        else if (_listIdx.Equals(-1))
            return;
        else if (listSelectedFriendlyObject[_listIdx] != null)
        {
            if (listSelectedFriendlyObject.Count < 2)
            {
                unitInfoContainer.curHpPercent = listSelectedFriendlyObject[0].GetCurHpPercent;
            }
            else
            {
                SFriendlyUnitInfo tempInfo = listFriendlyUnitInfo[_listIdx];
                tempInfo.curHpPercent = listSelectedFriendlyObject[_listIdx].GetCurHpPercent;
                listFriendlyUnitInfo[_listIdx] = tempInfo;
            }
        }
    }

    private void InputFriendlyUnitInfo()
    {
        for (int i = 0; i < listSelectedFriendlyObject.Count; ++i)
        {
            SFriendlyUnitInfo tempInfo = listFriendlyUnitInfo[i];
            tempInfo.unitType = listSelectedFriendlyObject[i].GetUnitType;
            tempInfo.curHpPercent = listSelectedFriendlyObject[i].GetCurHpPercent;
            listFriendlyUnitInfo[i] = tempInfo;
            listSelectedFriendlyObject[i].Select(i);
        }
    }

    private void InputOtherUnitInfo(SelectableObject _obj)
    {
        unitInfoContainer.objectType = _obj.GetObjectType();
        unitInfoContainer.maxHp = _obj.MaxHp;
        unitInfoContainer.curHpPercent= _obj.GetCurHpPercent;
        unitInfoContainer.attDmg = _obj.AttDmg;
        unitInfoContainer.attRange = _obj.AttRange;
        unitInfoContainer.attRate = _obj.AttRate;

        if (!_obj.GetObjectType().Equals(EObjectType.ENEMY_UNIT))
        {
            unitInfoContainer.unitType = _obj.GetComponent<FriendlyObject>().GetUnitType;
            _obj.GetComponent<FriendlyObject>().Select();
        }
    }

 

    public void ResetTargetBunker()
    {
        foreach (FriendlyObject obj in listSelectedFriendlyObject)
            obj.ResetTargetBunker();
    }

    public void MoveUnitByPicking(Vector3 _targetPos, bool isAttackMove = false)
    {
        if (IsListEmpty) return;
        if (isFriendlyStructureInList) return;
        if (isEnemyObjectInList) return;

        if (isAttackMove)
        {
            Vector3 centerPos = CalcFormationCenterPos(_targetPos.y);
            foreach (FriendlyObject obj in listSelectedFriendlyObject)
                obj.MoveAttack(_targetPos + CalcPosInFormation(obj.Position, centerPos));
        }
        else if (IsGroupMaxDistOverRange())
        {
            // 지정된 위치에 5열 종대로 헤쳐모여
            CalcNewFormation(_targetPos);
        }
        else
        {
            // 대열 유지하면서 모이기
            Vector3 centerPos = CalcFormationCenterPos(_targetPos.y);
            foreach (FriendlyObject obj in listSelectedFriendlyObject)
                obj.MoveByPos(_targetPos + CalcPosInFormation(obj.Position, centerPos));
        }
    }

    public void Stop()
    {
        foreach (FriendlyObject obj in listSelectedFriendlyObject)
            obj.Stop();
    }

    public void Hold()
    {
        foreach (FriendlyObject obj in listSelectedFriendlyObject)
            obj.Hold();
    }

    public void Patrol(Vector3 _wayPointTo)
    {
        CalcNewFormation(_wayPointTo, true);
    }

    private void CalcNewFormation(Vector3 _targetPos, bool _isPatrol = false)
    {
        if (isFriendlyStructureInList) return;
        if (isEnemyObjectInList) return;

        int unitCnt = listSelectedFriendlyObject.Count;
        int col = Mathf.Clamp(unitCnt, 1, 5);

        float posX = 0f;
        float posZ = 0f;
        Vector3 destPos = Vector3.zero;

        for (int i = 0; i < unitCnt; ++i)
        {
            posX = i % col;
            posZ = i / col;
            destPos = _targetPos + new Vector3(posX, 0f, posZ);

            if(_isPatrol)
                listSelectedFriendlyObject[i].Patrol(destPos);
            else
                listSelectedFriendlyObject[i].MoveByPos(destPos);
        }
    }

    private Vector3 CalcFormationCenterPos(float _targetPosY)
    {
        Vector3 centerPos = Vector3.zero;
        foreach(FriendlyObject obj in listSelectedFriendlyObject)
        {
            centerPos.x += obj.Position.x;
            centerPos.z += obj.Position.z;
        }

        centerPos /= listSelectedFriendlyObject.Count;
        centerPos.y += _targetPosY;

        return centerPos;
    }

    private Vector3 CalcPosInFormation(Vector3 _myPos, Vector3 _centerPos)
    {
        return _myPos - _centerPos;
    }

    private bool IsGroupMaxDistOverRange()
    {
        Vector3 objPos = listSelectedFriendlyObject[0].Position;
        float maxX = objPos.x;
        float minX = objPos.x;
        float maxZ = objPos.z;
        float minZ = objPos.z;

        foreach (FriendlyObject obj in listSelectedFriendlyObject)
        {
            objPos = obj.Position;
            if (objPos.x > maxX) maxX = objPos.x;
            else if (objPos.x < minX) minX = objPos.x;

            if (objPos.z > maxZ) maxZ = objPos.z;
            else if (objPos.z < minZ) minZ = objPos.z;
        }

        return Mathf.Abs(maxX - minX) > rangeGroupLimitDist || Mathf.Abs(maxZ - minZ) > rangeGroupLimitDist;
    }

    public void MoveUnitByPicking(Transform _targetTr)
    {
        if (isFriendlyStructureInList) return;
        if (isEnemyObjectInList) return;

        if (_targetTr.GetComponent<IGetObjectType>().GetObjectType().Equals(EObjectType.BUNKER))
        {
            curBunker = _targetTr.GetComponent<StructureBunker>();
            foreach (FriendlyObject obj in listSelectedFriendlyObject)
                obj.FollowTarget(_targetTr, true);
        }
        else
        {
            foreach (FriendlyObject obj in listSelectedFriendlyObject)
                obj.FollowTarget(_targetTr);
        }
    }

    public void RegisterBroker()
    {
        Broker.Regist(this, EPublisherType.SELECTABLE_MANAGER);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.SELECTABLE_MANAGER);
    }

    public void CompleteUpgradeRangedUnitDmg()
    {
        ++levelRangedUnitDmgUpgrade;
        PushMessageToBroker(EMessageType.UPGRADE_RANGED_DMG);
    }

    public void CompleteUpgradeRangedUnitHp()
    {
        ++levelRangedUnitHpUpgrade;
        PushMessageToBroker(EMessageType.UPGRADE_RANGED_HP);
    }

    public void CompleteUpgradeMeleeUnitDmg()
    {
        ++levelMeleeUnitDmgUpgrade;
        PushMessageToBroker(EMessageType.UPGRADE_MELEE_DMG);
    }

    public void CompleteUpgradeMeleeUnitHp()
    {
        ++levelMeleeUnitHpUpgrade;
        PushMessageToBroker(EMessageType.UPGRADE_MELEE_HP);
    }

    [Header("-Melee/Ranged")]
    [SerializeField]
    private GameObject[] arrUnitPrefab = null;

    [SerializeField]
    private float rangeGroupLimitDist = 5f;

    private static int levelRangedUnitDmgUpgrade = 1;
    private static int levelRangedUnitHpUpgrade = 1;
    private static int levelMeleeUnitDmgUpgrade = 1;
    private static int levelMeleeUnitHpUpgrade = 1;
    private static float delayUnitUpgrade = 5f;

    private bool isFriendlyUnitInList = false;
    private bool isFriendlyStructureInList = false;
    private bool isEnemyObjectInList = false;

    private List<SelectableObject> tempListSelectableObject = new List<SelectableObject>();
    private static List<FriendlyObject> listSelectedFriendlyObject = new List<FriendlyObject>();

    private VoidSelectObjectTypeDelegate selectObjectCallback = null;

    private StructureBunker curBunker = null;

    private static PF_Grid grid = null;
    private static Dictionary<int, PF_Node> dicNodeUnderFriendlyUnit = new Dictionary<int, PF_Node>();
    private static Dictionary<int, PF_Node> dicNodeUnderEnemyUnit = new Dictionary<int, PF_Node>();
    private static int dicFriendlyIdx = 0;
    private static int dicEnemyIdx = 0;

    private static UnitInfoContainer unitInfoContainer = null;
    private static List<SFriendlyUnitInfo> listFriendlyUnitInfo = null;

    private MemoryPool[] arrMemoryPool = null;

    private List<FriendlyObject>[] arrCrowd = null;
}
