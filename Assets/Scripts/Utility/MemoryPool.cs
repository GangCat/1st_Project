using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    public int TotalCnt => ttlCnt;
    public int ActiveCnt => activeCnt;

    /// <summary>
    /// 입력받은 오브젝트를 대상으로 메모리풀 생성
    /// </summary>
    /// <param name="_poolObject"></param>
    public MemoryPool(GameObject _poolObject, int _increaseCnt = 5, Transform _parentTr = null)
    {
        ttlCnt = 0;
        activeCnt = 0;

        poolObject = _poolObject;

        arrList = new List<GameObject>[10];
        for(int i = 0; i < 10; ++i)
            arrList[i] = new List<GameObject>(30);

        poolListEnable = new List<GameObject>();
        poolQueueDisable = new Queue<GameObject>();

        InstantiateObjects(_increaseCnt, _parentTr);
    }

    /// <summary>
    /// increaseCnt 단위로 오브젝트를 생성
    /// </summary>
    public void InstantiateObjects(int _increaseCnt = 5, Transform _parentTr = null)
    {
        for (int i = 0; i < _increaseCnt; ++i)
        {
            GameObject poolGo = GameObject.Instantiate(poolObject);
            poolGo.SetActive(false);
            if (_parentTr != null)
                poolGo.transform.parent = _parentTr;

            poolQueueDisable.Enqueue(poolGo);
        }

        ttlCnt += _increaseCnt;
    }

    /// <summary>
    /// 현재 관리중인 모든 오브젝트를 '삭제'
    /// 씬이 바뀌거나 게임이 종료될 때 한 번만 호출
    /// </summary>
    public void DestroyObjects()
    {
        if (poolListEnable == null || poolQueueDisable == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
            GameObject.Destroy(poolListEnable[i]);

        cnt = poolQueueDisable.Count;
        for(int i = 0; i < cnt; ++i)
            GameObject.Destroy(poolQueueDisable.Dequeue());

        poolListEnable.Clear();
        poolQueueDisable.Clear();
    }

    /// <summary>
    /// 해당 오브젝트를 활성화
    /// </summary>
    /// <returns></returns>
    public GameObject ActivatePoolItem(int _increaseCnt = 5, Transform _parentTr = null)
    {
        if (poolListEnable == null || poolQueueDisable == null) return null;

        if (poolQueueDisable.Count <= 0)
            InstantiateObjects(_increaseCnt, _parentTr);

        GameObject poolGo = poolQueueDisable.Dequeue();
        poolListEnable.Add(poolGo);

        poolGo.SetActive(true);

        ++activeCnt;

        return poolGo;
    }


    public GameObject ActivatePoolItemWithIdx(int _idx, int _increaseCnt = 5, Transform _parentTr = null)
    {
        if (arrList == null || poolQueueDisable == null) return null;

        if (poolQueueDisable.Count <= 0)
            InstantiateObjects(_increaseCnt, _parentTr);

        GameObject poolGo = poolQueueDisable.Dequeue();
        arrList[_idx / 30].Add(poolGo);

        poolGo.SetActive(true);

        ++activeCnt;

        return poolGo;
    }

    /// <summary>
    /// 해당 오브젝트를 비활성화
    /// </summary>
    /// <param name="_removeObject"></param>
    public void DeactivatePoolItem(GameObject _removeObject)
    {
        if (poolListEnable == null || poolQueueDisable == null || _removeObject == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
        {
            GameObject poolGo = poolListEnable[i];
            if (poolGo == _removeObject)
            {
                poolGo.SetActive(false);
                poolListEnable.Remove(poolGo);
                poolQueueDisable.Enqueue(poolGo);
                poolGo.transform.position = tempStorePos;

                --activeCnt;

                return;
            }
        }
    }

    public GameObject DeactivatePoolItemWithIdx(GameObject _removeObject, int _idx)
    {
        if (arrList == null || poolQueueDisable == null || _idx < 0) return null;

        int arrIdx = _idx / 30;

        for(int i = 0; i < arrList[arrIdx].Count; ++i)
        {
            GameObject poolGo = arrList[arrIdx][i];
            if (poolGo.Equals(_removeObject))
            {
                poolGo.SetActive(false);
                arrList[arrIdx].RemoveAt(i);
                poolQueueDisable.Enqueue(poolGo);
                poolGo.transform.position = tempStorePos;

                --activeCnt;

                return poolGo;
            }
        }

        return null;
    }

    /// <summary>
    /// 모든 오브젝트를 비활성화
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolListEnable == null || poolQueueDisable == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
        {
            GameObject poolGo = poolListEnable[i];

            poolGo.SetActive(false);
            poolGo.transform.position = tempStorePos;

            poolListEnable.Remove(poolGo);
            poolQueueDisable.Enqueue(poolGo);
        }

        activeCnt = 0;
    }

    public bool IsEnableListEmpty()
    {
        return poolListEnable.Count < 1;
    }

    private int ttlCnt = 0;
    private int activeCnt = 0;

    private Vector3 tempStorePos = new Vector3(-3000f, 0f, 0f);
    private GameObject poolObject = null; // 오브젝트 풀링에서 관리하는 오브젝트 프리팹

    private List<GameObject>[] arrList = null;
    private List<GameObject> poolListEnable = null;
    private Queue<GameObject> poolQueueDisable = null;
}
