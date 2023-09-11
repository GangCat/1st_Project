using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectManager : MonoBehaviour
{
    public delegate void VoidSelectObjectTypeDelegate(ESelectableObjectType _objectType);
    public bool IsListEmpty => listSelectedObject.Count < 1;
    public bool IsFriendlyUnit => isFriendlyUnitInList;
    public SelectableObject GetFirstSelectableObjectInList => listSelectedObject[0];

    public void Init(VoidSelectObjectTypeDelegate _selectObjectCallback)
    {
        tempListSelectableObject.Clear();
        listSelectedObject.Clear();
        selectObjectCallback = _selectObjectCallback;
    }

    public void AddSelectedObject(SelectableObject _object)
    {
        tempListSelectableObject.Add(_object);
    }

    public void RemoveSelectedObject(SelectableObject _object)
    {
        tempListSelectableObject.Remove(_object);
    }

    public SelectableObject[] SelectFinish()
    {
        listSelectedObject.Clear();
        if (tempListSelectableObject.Count < 1) return tempListSelectableObject.ToArray();

        SelectableObject tempObj = null;
        isFriendlyUnitInList = false;
        // 오브젝트를 하나하나 검사.
        foreach (SelectableObject obj in tempListSelectableObject)
        {
            if (obj == null) continue;

            switch (obj.ObjectType)
            {
                case ESelectableObjectType.UNIT:
                case ESelectableObjectType.HERO:
                    if (!isFriendlyUnitInList)
                    {
                        listSelectedObject.Clear();
                        listSelectedObject.Add(obj);
                        isFriendlyUnitInList = true;
                    }
                    else
                        listSelectedObject.Add(obj);
                    break;
                case ESelectableObjectType.MAIN_BASE:
                case ESelectableObjectType.TURRET:
                case ESelectableObjectType.BUNKER:
                case ESelectableObjectType.WALL:
                    if (isFriendlyUnitInList) break;
                    if (!tempObj) tempObj = obj;
                    if (!tempObj.ObjectType.Equals(obj.ObjectType))
                        tempObj = obj;
                    break;
                case ESelectableObjectType.ENEMY_UNIT:
                case ESelectableObjectType.ENEMY_STRUCTURE:
                    if (isFriendlyUnitInList) break;
                    if (!tempObj) tempObj = obj;
                    break;
            }
        }

        if (!isFriendlyUnitInList)
        {
            listSelectedObject.Add(tempObj);
        }
        selectObjectCallback?.Invoke(listSelectedObject[0].ObjectType);

        tempListSelectableObject.Clear();

        return listSelectedObject.ToArray();
    }

    public void MoveUnitByPicking(Vector3 _targetPos, bool isAttackMove = false)
    {
        if (isAttackMove)
        {
            Vector3 centerPos = CalcFormationCenterPos(_targetPos.y);
            foreach (SelectableObject obj in listSelectedObject)
                obj.MoveAttack(_targetPos + CalcPosInFormation(obj.GetPos, centerPos));
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
            foreach (SelectableObject obj in listSelectedObject)
                obj.MoveByTargetPos(_targetPos + CalcPosInFormation(obj.GetPos, centerPos));
        }
    }

    public void Stop()
    {
        foreach (SelectableObject obj in listSelectedObject)
            obj.Stop();
    }

    private void CalcNewFormation(Vector3 _targetPos)
    {
        int unitCnt = listSelectedObject.Count;
        int col = Mathf.Clamp(unitCnt, 1, 5);

        float posX = 0f;
        float posZ = 0f;
        Vector3 destPos = Vector3.zero;

        for (int i = 0; i < unitCnt; ++i)
        {
            posX = i % col;
            posZ = i / col;
            destPos = _targetPos + new Vector3(posX, 0f, posZ);

            listSelectedObject[i].MoveByTargetPos(destPos);
        }
    }

    private Vector3 CalcFormationCenterPos(float _targetPosY)
    {
        Vector3 centerPos = Vector3.zero;
        foreach(SelectableObject obj in listSelectedObject)
        {
            centerPos.x += obj.GetPos.x;
            centerPos.z += obj.GetPos.z;
        }

        centerPos /= listSelectedObject.Count;
        centerPos.y += _targetPosY;

        return centerPos;
    }

    private Vector3 CalcPosInFormation(Vector3 _myPos, Vector3 _centerPos)
    {
        return _myPos - _centerPos;
    }

    private bool IsGroupMaxDistOverRange()
    {
        Vector3 objPos = listSelectedObject[0].GetPos;
        float maxX = objPos.x;
        float minX = objPos.x;
        float maxZ = objPos.z;
        float minZ = objPos.z;

        foreach (SelectableObject obj in listSelectedObject)
        {
            objPos = obj.GetPos;
            if (objPos.x > maxX) maxX = objPos.x;
            else if (objPos.x < minX) minX = objPos.x;

            if (objPos.z > maxZ) maxZ = objPos.z;
            else if (objPos.z < minZ) minZ = objPos.z;
        }

        return Mathf.Abs(maxX - minX) > rangeGroupLimitDist || Mathf.Abs(maxZ - minZ) > rangeGroupLimitDist;
    }

    public void MoveUnitByPicking(Transform _targetTr)
    {
        foreach (SelectableObject obj in listSelectedObject)
            obj.FollowTarget(_targetTr);
    }

    private bool isFriendlyUnitInList = false;

    private List<SelectableObject> tempListSelectableObject = new List<SelectableObject>();
    private List<SelectableObject> listSelectedObject = new List<SelectableObject>();

    [SerializeField]
    private int rowNum = 6;
    [SerializeField]
    private float rangeGroupLimitDist = 5f;
    [SerializeField]
    private PF_Grid grid = null;

    private VoidSelectObjectTypeDelegate selectObjectCallback = null;


}
