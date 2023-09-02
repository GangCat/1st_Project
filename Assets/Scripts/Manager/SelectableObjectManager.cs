using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectManager : MonoBehaviour
{
    public void Init()
    {
        tempListSelectableObject.Clear();
        listSelectedObject.Clear();
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
        bool isFriendlyUnit = false;
        // 오브젝트를 하나하나 검사.
        foreach (SelectableObject obj in tempListSelectableObject)
        {
            switch (obj.ObjectType)
            {
                case ESelectableObjectType.FriendlyUnit:
                    if (!isFriendlyUnit)
                    {
                        listSelectedObject.Clear();
                        listSelectedObject.Add(obj);
                        isFriendlyUnit = true;
                    }
                    else
                        listSelectedObject.Add(obj);
                    break;
                case ESelectableObjectType.FriendlyStructure:
                    if (isFriendlyUnit) break;
                    if (!tempObj)       tempObj = obj;
                    if (!tempObj.ObjectType.Equals(obj.ObjectType))
                        tempObj = obj;
                    break;
                case ESelectableObjectType.EnemyUnit:
                    if (isFriendlyUnit) break;
                    if (!tempObj)       tempObj = obj;
                    break;
                case ESelectableObjectType.EnemyStructure:
                    if (isFriendlyUnit) break;
                    if (!tempObj)       tempObj = obj;
                    break;
            }
        }
        if (!isFriendlyUnit)
            listSelectedObject.Add(tempObj);
        tempListSelectableObject.Clear();

        return listSelectedObject.ToArray();
    }

    public void MoveUnitByPicking(Vector3 _movePos)
    {
        // 이동시 해당 도착점을 중심점으로 각 오브젝트들의 위치를 결정함. 이 때 버튼만든것처럼 오프셋으로 해서 3by4로
        // 반드시 열 개수는 4개로 즉 반드시 /4랑 %4로 위치 결정
        // /랑 % 이용해서 편하게 ㅇㅇ
        // 단체 이동
    }

    private List<SelectableObject> tempListSelectableObject = new List<SelectableObject>();
    private List<SelectableObject> listSelectedObject = new List<SelectableObject>();
}
