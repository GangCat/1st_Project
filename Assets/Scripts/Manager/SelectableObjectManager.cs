using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectManager : MonoBehaviour
{
    public bool IsListEmpty => listSelectedObject.Count < 1;
    public bool IsFriendlyUnit => isFriendlyUnit;

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
        isFriendlyUnit = false;
        // ������Ʈ�� �ϳ��ϳ� �˻�.
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
        int numForCalcPos = listSelectedObject.Count - 1;
        float halfOffset = offset * 0.5f;
        float startXPos = _movePos.x - numForCalcPos % 4 * halfOffset;
        float startZPos = _movePos.z + numForCalcPos / 4 * halfOffset;

        // �̵��� �ش� �������� �߽������� �� ������Ʈ���� ��ġ�� ������. �� �� ��ư�����ó�� ���������� �ؼ� 3by4��
        // �ݵ�� �� ������ 4���� �� �ݵ�� /4�� %4�� ��ġ ����
        // /�� % �̿��ؼ� ���ϰ� ����
        for(int i = 0; i < numForCalcPos + 1; ++i)
            listSelectedObject[i].MoveToTargetPos(new Vector3(startXPos + ((i % 4) * offset) ,0f, startZPos - ((i / 4) * offset)));

        // ��ü �̵�
    }
    
    private bool isFriendlyUnit = false;

    private List<SelectableObject> tempListSelectableObject = new List<SelectableObject>();
    private List<SelectableObject> listSelectedObject = new List<SelectableObject>();

    [SerializeField]
    private float offset = 1f;
}
