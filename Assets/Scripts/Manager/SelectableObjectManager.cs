using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectManager : MonoBehaviour
{
    public delegate void VoidSelectObjectTypeDelegate(EObjectType _objectType);
    public bool IsListEmpty => listSelectedFriendlyObject.Count < 1;
    public bool IsFriendlyUnit => isFriendlyUnitInList;
    public FriendlyObject GetFirstSelectedObjectInList => listSelectedFriendlyObject[0];

    public void Init(VoidSelectObjectTypeDelegate _selectObjectCallback, PF_Grid _grid)
    {
        listSelectedFriendlyObject.Clear();
        tempListSelectableObject.Clear();
        selectObjectCallback = _selectObjectCallback;
        grid = _grid;
    }

    public static int InitNode(Vector3 _pos)
    {
        dicNodeUnderUnit.Add(dicNodeUnderUnit.Count, grid.GetNodeFromWorldPoint(_pos));
        return dicNodeUnderUnit.Count - 1;
    }

    public static void UpdateNodeWalkable(Vector3 _pos, int _idx)
    {
        PF_Node tempNode = null;
        if (dicNodeUnderUnit.TryGetValue(_idx, out tempNode))
            tempNode.walkable = true;

        tempNode = grid.GetNodeFromWorldPoint(_pos);
        tempNode.walkable = false;
        dicNodeUnderUnit[_idx] = tempNode;
    }

    public static void ResetNodeWalkable(Vector3 _pos, int _idx)
    {
        dicNodeUnderUnit[_idx].walkable = true;
        grid.GetNodeFromWorldPoint(_pos).walkable = true;
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
                listSelectedFriendlyObject.Remove(obj);
                return;
            }
        }
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

    public void SpawnUnit(ESpawnUnitType _unitType)
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
            return;
        }

        SelectableObject tempObj = null;
        isFriendlyUnitInList = false;
        bool isFriendlyStructure = false;
        bool isEnemyObject = false;
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
                        isFriendlyStructure = false;
                        isEnemyObject = false;
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
                    isFriendlyStructure = true;
                    isEnemyObject = false;
                    if (!tempObj) tempObj = obj;
                    break;
                case EObjectType.ENEMY_UNIT:
                case EObjectType.ENEMY_STRUCTURE:
                    if (isFriendlyUnitInList) break;
                    if (isFriendlyStructure) break;
                    isEnemyObject = true;
                    if (!tempObj) tempObj = obj;
                    break;
                default:
                    break;
            }
        }

        if (isEnemyObject)
        {
            selectedEnemyObject = tempObj.GetComponent<EnemyObject>();
            selectObjectCallback?.Invoke(selectedEnemyObject.GetObjectType());
        }
        else if (isFriendlyStructure)
        {
            listSelectedFriendlyObject.Add(tempObj.GetComponent<FriendlyObject>());
            if (tempObj.GetComponent<Structure>().IsUnderConstruction)
                selectObjectCallback?.Invoke(EObjectType.HBEAM);
            else
                selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());
        }
        else if (isFriendlyUnitInList)
            selectObjectCallback?.Invoke(listSelectedFriendlyObject[0].GetObjectType());

        tempListSelectableObject.Clear();
        return;
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

    [SerializeField]
    private float rangeGroupLimitDist = 5f;

    private bool isFriendlyUnitInList = false;

    private List<SelectableObject> tempListSelectableObject = new List<SelectableObject>();
    private List<FriendlyObject> listSelectedFriendlyObject = new List<FriendlyObject>();
    private EnemyObject selectedEnemyObject = null;

    private VoidSelectObjectTypeDelegate selectObjectCallback = null;

    private StructureBunker curBunker = null;

    private static PF_Grid grid = null;
    private static Dictionary<int, PF_Node> dicNodeUnderUnit = new Dictionary<int, PF_Node>();
    private static int unitUpgradeLimit = 1;
}
