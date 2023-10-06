using UnityEngine;

public interface IMinimapObserver
{
    /// <summary>
    /// 주체로부터 업데이트를 받는 메소드
    /// </summary>
    /// <param name="_pos"></param>
    void GetUnitTargetPos(Vector3 _pos);
    void GetCameraTargetPos(Vector3 _pos);
}