using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectableObjectManager : MonoBehaviour, IPublisher
{
    public delegate void VoidSelectObjectTypeDelegate(EObjectType _objectType);
    public bool IsListEmpty => listSelectedFriendlyObject.Count < 1;
    public bool IsFriendlyUnit => isFriendlyUnitInList;
    public FriendlyObject GetFirstSelectedObjectInList => listSelectedFriendlyObject[0];
    public static int LevelRangedUnitDmgUpgrade => levelRangedUnitDmgUpgrade;
    public static int LevelRangedUnitHpUpgrade => levelRangedUnitHpUpgrade;
    public static int LevelMeleeUnitHpUpgrade => levelMeleeUnitHpUpgrade;
    public static int LevelMeleeUnitDmgUpgrade => levelMeleeUnitDmgUpgrade;
    public static float DelayUnitUpgrade => delayUnitUpgrade;
    public static Dictionary<int, PF_Node> DicNodeUnderFriendlyUnit => dicNodeUnderFriendlyUnit;
    public static Dictionary<int, PF_Node> DicNodeUnderEnemyUnit => dicNodeUnderEnemyUnit;

    public void Init(VoidSelectObjectTypeDelegate _selectObjectCallback, PF_Grid _grid)
    {
        listSelectedFriendlyObject.Clear();
        tempListSelectableObject.Clear();
        selectObjectCallback = _selectObjectCallback;
        grid = _grid;
        RegisterBroker();

        unitInfoContainer = new UnitInfoContainer();
        listFriendlyUnitInfo = new List<SFriendlyUnitInfo>(12);

        for(int i = 0; i < listFriendlyUnitInfo.Capacity; ++i)
            listFriendlyUnitInfo.Add(new SFriendlyUnitInfo());

        ArrayHUDCommand.Use(EHUDCommand.INIT_DISPLAY_GROUP_INFO, listFriendlyUnitInfo);
        ArrayHUDCommand.Use(EHUDCommand.INIT_DISPLAY_SINGLE_INFO, unitInfoContainer);
    }

    public static int InitNodeFriendly(Vector3 _pos)
    {
        dicNodeUnderFriendlyUnit.Add(dicFriendlyIdx, grid.GetNodeFromWorldPoint(_pos));
        return dicFriendlyIdx++;
    }

    public static int InitNodeEnemy(Vector3 _pos)
    {
        dicNodeUnderEnemyUnit.Add(dicEnemyIdx, grid.GetNodeFromWorldPoint(_pos));
        return dicEnemyIdx++;
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

    public static bool isCurNodwWalkable(Vector3 _pos)
    {
        return grid.GetNodeFromWorldPoint(_pos).walkable;
    }

    public static Vector3 ResetPosition(Vector3 _pos)
    {
        PF_Node unitNode = grid.GetNodeFromWorldPoint(_pos);

        if (!grid.GetNodeFromWorldPoint(_pos).walkable)
            return grid.GetAccessibleNodeWithoutTargetNode(unitNode).worldPos;

        return unitNode.worldPos;
    }

    public void RemoveUnitAtList(FriendlyObject _removeObj)
    {
        if (_removeObj == null) return;

        foreach (FriendlyObject obj in listSelectedFriendlyObject)
        {
            if (obj.Equals(_removeObj))
            {
                obj.unSelect();
                listSelectedFriendlyObject.Remove(obj);
                break;
            }
        }

        UpdateFuncButton();
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

    public void SpawnUnit(EUnitType _unitType)
    {
        if (listSelectedFriendlyObject[0].GetObjectType().Equals(EObjectType.BARRACK))
            listSelectedFriendlyObject[0].GetComponent<StructureBarrack>().SpawnUnit(_unitType);
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
        tempListSelectableObject.Add(_object);
    }

    public void RemoveSelectedObject(SelectableObject _object)
    {
        tempListSelectableObject.Remove(_object);
    }

    public void SelectFinish()
    {
        listSelectedFriendlyObject.Clear();
        if (tempListSelectableObject.Count < 1)
        {
            selectObjectCallback?.Invoke(EObjectType.NONE);
            ArrayHUDCommand.Use(EHUDCommand.HIDE_UNIT_INFO);
            return;
        }

        SelectableObject tempObj = null;
        isFriendlyUnitInList = false;
        isFriendlyStructureInList = false;
        isEnemyObjectInList = false;
        // 오브젝트를 하나하나 검사.
        foreach (SelectableObject obj in tempListSelectableObject)
        {
            if (obj == null) continue;

            if (listSelectedFriendlyObject.Count > 11) break;

            switch (obj.GetObjectType())
            {
                case EObjectType.UNIT:
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

        if (isEnemyObjectInList)
        {
            selectObjectCallback?.Invoke(tempObj.GetObjectType());
            InputEnemyUnitInfo(tempObj);
            ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO, unitInfoContainer);
        }
        else if (isFriendlyStructureInList)
        {
            InputStructureInfo(tempObj);
            listSelectedFriendlyObject.Add(tempObj.GetComponent<FriendlyObject>());
            if (tempObj.GetComponent<Structure>().IsUnderConstruction)
                selectObjectCallback?.Invoke(EObjectType.HBEAM);
            else if (tempObj.GetComponent<Structure>().IsProcessingUpgrade)
                selectObjectCallback?.Invoke(EObjectType.PROCESSING_UPGRADE_STRUCTURE);
            else
                selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
            ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO, unitInfoContainer);
        }
        else if (isFriendlyUnitInList)
        {
            selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());

            if (listSelectedFriendlyObject.Count < 2)
            {
                InputEnemyUnitInfo(listSelectedFriendlyObject[0]);
                ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO, unitInfoContainer);
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

    public static void UpdateHp(int _listIdx)
    {
        SFriendlyUnitInfo tempInfo = listFriendlyUnitInfo[_listIdx];
        tempInfo.curHpPercent = listSelectedFriendlyObject[_listIdx].GetCurHpPercent;
        listFriendlyUnitInfo[_listIdx] = tempInfo;
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

    private void InputEnemyUnitInfo(SelectableObject _obj)
    {
        unitInfoContainer.objectType = _obj.GetObjectType();
        unitInfoContainer.maxHp = _obj.MaxHp;
        unitInfoContainer.attDmg = _obj.AttDmg;
        unitInfoContainer.attRange = _obj.AttRange;
        unitInfoContainer.attRate = _obj.AttRate;
    }

    private void InputStructureInfo(SelectableObject _obj)
    {
        unitInfoContainer.objectType = _obj.GetObjectType();
        unitInfoContainer.maxHp = _obj.MaxHp;
        unitInfoContainer.attDmg = 0;
        unitInfoContainer.attRange = 0;
        unitInfoContainer.attRate = 0;
    }

    public void UpdateFuncButton()
    {
        if (listSelectedFriendlyObject.Count > 0)
        {
            if (isFriendlyUnitInList)
            {
                if (listSelectedFriendlyObject.Count > 1)
                {
                    if (listSelectedFriendlyObject.Count == 1)
                    {
                        InputEnemyUnitInfo(listSelectedFriendlyObject[0]);
                        ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO, unitInfoContainer);
                    }
                    else
                    {
                        InputFriendlyUnitInfo();
                        ArrayHUDCommand.Use(EHUDCommand.DISPLAY_GROUP_INFO, listSelectedFriendlyObject.Count);
                    }
                    selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
                }
                else
                {
                    isFriendlyStructureInList = false;
                    ArrayHUDCommand.Use(EHUDCommand.HIDE_UNIT_INFO);
                    selectObjectCallback?.Invoke(EObjectType.NONE);
                }
            }
            else if (isFriendlyStructureInList)
            {
                if (listSelectedFriendlyObject[0].GetComponent<Structure>().IsProcessingUpgrade)
                    selectObjectCallback?.Invoke(EObjectType.PROCESSING_UPGRADE_STRUCTURE);
                else
                    selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
                InputStructureInfo(listSelectedFriendlyObject[0]);
                ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO, unitInfoContainer);
            }
            else
            {
                ArrayHUDCommand.Use(EHUDCommand.HIDE_UNIT_INFO);
                selectObjectCallback?.Invoke(EObjectType.NONE);
            }
        }
    }

    public void ResetTargetBunker()
    {
        foreach (FriendlyObject obj in listSelectedFriendlyObject)
            obj.ResetTargetBunker();
    }

    public void MoveUnitByPicking(Vector3 _targetPos, bool isAttackMove = false)
    {
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

    private UnitInfoContainer unitInfoContainer = null;
    private static List<SFriendlyUnitInfo> listFriendlyUnitInfo = null;
}
