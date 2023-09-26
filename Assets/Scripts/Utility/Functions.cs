using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Functions
{
    /// <summary>
    /// euler값으로 transform을 회전시키는 메서드
    /// </summary>
    /// <param name="_tr"></param>
    /// <param name="_euler"></param>
    public static void SetRotation(Transform _tr, Vector3 _euler)
    {
        _tr.rotation = Quaternion.Euler(_euler);
    }

    /// <summary>
    /// y축 기준 _angle값으로 transform을 회전시키는 메서드
    /// </summary>
    /// <param name="_tr"></param>
    /// <param name="_angle_degree"></param>
    public static void RotateYaw(Transform _tr, float _angle_degree)
    {
        _tr.rotation = Quaternion.Euler(0f, -_angle_degree + 90f, 0f);
    }

    /// <summary>
    /// z축 기준 _angle값으로 transform을 회전시키는 메서드
    /// </summary>
    /// <param name="_tr"></param>
    /// <param name="_angle_degree"></param>
    public static void RotatePitch(Transform _tr, float _angle_degree)
    {
        _tr.rotation = Quaternion.Euler(0f, 0f, -_angle_degree + 90f);
    }

    /// <summary>
    /// x축 기준 _angle값으로 transform을 회전시키는 메서드
    /// </summary>
    /// <param name="_tr"></param>
    /// <param name="_angle_degree"></param>
    public static void RotateRoll(Transform _tr, float _angle_degree)
    {
        _tr.rotation = Quaternion.Euler(-_angle_degree + 90f, 0f, 0f);
    }

    /// <summary>
    /// _oriPos와 _targetPos가 이루는 degree를 반환하는 메서드
    /// </summary>
    /// <param name="_oriPos"></param>
    /// <param name="_targetPos"></param>
    /// <returns></returns>
    public static float CalcAngleToTarget(Vector3 _oriPos, Vector3 _targetPos)
    {
        Vector3 oriPos = _oriPos;
        oriPos.y = 0f;
        Vector3 targetPos = _targetPos;
        targetPos.y = 0f;

        Vector3 dirToTarget = (targetPos - oriPos).normalized;
        return Mathf.Atan2(dirToTarget.z, dirToTarget.x) * Mathf.Rad2Deg;
    }

    public static bool Picking(out RaycastHit _hit)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out _hit))
            return true;

        return false;
    }

    /// <summary>
    /// 태그만을 가지고 피킹하는 함수.
    /// _point로 피킹된 위치를 참조하고 성공 여부를 반환한다.
    /// </summary>
    /// <param name="_tag"></param>
    /// <param name="_point"></param>
    /// <returns></returns>
    public static bool Picking(string _tag, ref Vector3 _point)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(_tag))
            {
                _point = hit.point;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 태그와 레이어마스크를 이용해 피킹하는 메서드
    /// _point로 피킹된 위치를 참조하고 성공 여부를 반환한다.
    /// _layerMask는 int형으로 넣어줘야 한다.
    /// </summary>
    /// <param name="_tag"></param>
    /// <param name="_layerMask"></param>
    /// <param name="_point"></param>
    /// <returns></returns>
    public static bool Picking(string _tag, LayerMask _layerMask, ref Vector3 _point)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, _layerMask))
        {
            if (hit.transform.CompareTag(_tag))
            {
                _point = hit.point;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 레이어마스크만 가지고 피킹하는 함수.
    /// </summary>
    /// <param name="_layerMask"></param>
    /// <param name="_hit"></param>
    /// <returns></returns>
    public static bool Picking(LayerMask _layerMask, out RaycastHit _hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 3f);
        if (Physics.Raycast(ray, out _hit, 1000f, _layerMask))
            return true;

        return false;
    }

    /// <summary>
    /// _rad 반지름을 가지는 원 안의 랜덤한 위치를 반환하는 메서드.
    /// 이 때 y값은 0으로 고정
    /// </summary>
    /// <param name="_rad"></param>
    /// <returns></returns>
    public static Vector3 ComputeRandomPointInCircle(float _rad)
    {
        Vector2 rnd = Random.insideUnitCircle * _rad;
        return new Vector3(rnd.x, 0f, rnd.y);
    }

    /// <summary>
    /// _outerCircle과 _innerCircle 사이의 공간 중 랜덤한 위치를 반환하는 메서드
    /// _outerCircleRad < _innerCircleRad일 경우 Vector3.zero 반환.
    /// </summary>
    /// <param name="_outerCircleRad"></param>
    /// <param name="_innerCircleRad"></param>
    /// <returns></returns>
    public static Vector3 GetRandomPosition(float _outerCircleRad, float _innerCircleRad)
    {
        if (_outerCircleRad < _innerCircleRad)
        {
            Debug.LogError("OuterCircleRad must bigger than InnerCircleRad");
            Debug.Break();
            return Vector3.zero;
        }

        float rndLength = Random.Range(_outerCircleRad, _innerCircleRad);
        float rndAngle = Random.Range(-180, 180);
        return new Vector3(rndLength * Mathf.Cos(rndAngle), 0f, rndLength * Mathf.Sin(rndAngle));
    }

    /// <summary>
    /// 설정 범위 내에 int값이 존재하는지 확인한다.
    /// 최소 <= 타겟 < 최대
    /// </summary>
    /// <param name="_targetInt"></param>
    /// <param name="_minRange"></param>
    /// <param name="_maxRange"></param>
    /// <returns></returns>
    public static bool CheckRange(int _targetInt, int _minRange, int _maxRange)
    {
        if (_minRange > _maxRange)
        {
            Debug.LogError("Range Parameter Error!!");
            Debug.Break();
        }

        if (_targetInt >= _minRange)
        {
            if (_targetInt < _maxRange)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 설정 범위 내에 float값이 존재하는지 확인한다.
    /// 최소 <= 타겟 <= 최대
    /// </summary>
    /// <param name="_targetFloat"></param>
    /// <param name="_minRange"></param>
    /// <param name="_maxRange"></param>
    /// <returns></returns>
    public static bool CheckRange(float _targetFloat, float _minRange, float _maxRange)
    {
        if (_minRange > _maxRange)
        {
            Debug.LogError("Range Parameter Error!!");
            Debug.Break();
        }

        if (_targetFloat >= _minRange)
        {
            if (_targetFloat <= _maxRange)
                return true;
        }
        return false;
    }
}

