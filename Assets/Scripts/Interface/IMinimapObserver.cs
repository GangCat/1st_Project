using UnityEngine;

public interface IMinimapObserver
{
    /// <summary>
    /// ��ü�κ��� ������Ʈ�� �޴� �޼ҵ�
    /// </summary>
    /// <param name="_pos"></param>
    void GetUnitTargetPos(Vector3 _pos);
    void GetCameraTargetPos(Vector3 _pos);
}