using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJMemoryPool
{
    public class MemoryPool
    {
        public int TotalCnt => ttlCnt;
        public int ActiveCnt => activeCnt;

        /// <summary>
        /// 입력받은 오브젝트를 대상으로 메모리풀을 생성하는 생성자.
        /// </summary>
        /// <param name="_poolObject"></param>
        public MemoryPool(GameObject _poolObject, int _increaseCnt = 5, Transform _parentTr = null)
        {
            ttlCnt = 0;
            activeCnt = 0;

            poolObject = _poolObject;

            poolEnableList = new List<GameObject>();
            poolDisableQueue = new Queue<GameObject>();

            InstantiateObjects(_increaseCnt, _parentTr);
        }

        private void InstantiateObjects(int _increaseCnt, Transform _parentTr)
        {
            for (int i = 0; i < _increaseCnt; ++i)
            {
                GameObject poolGo = GameObject.Instantiate(poolObject);
                poolGo.SetActive(false);
                if (_parentTr != null)
                    poolGo.transform.parent = _parentTr;

                poolDisableQueue.Enqueue(poolGo);
            }

            ttlCnt += _increaseCnt;
        }

        /// <summary>
        /// 현재 관리중인 모든 오브젝트를 파괴하는 메서드
        /// 씬이 바뀌거나 게임이 종료될 때 한 번만 호출
        /// </summary>
        public void DestroyObjects()
        {
            if (poolEnableList == null || poolDisableQueue == null) return;

            int cnt = poolEnableList.Count;
            for (int i = 0; i < cnt; ++i)
                GameObject.Destroy(poolEnableList[i]);

            cnt = poolDisableQueue.Count;
            for (int i = 0; i < cnt; ++i)
                GameObject.Destroy(poolDisableQueue.Dequeue());

            poolEnableList.Clear();
            poolDisableQueue.Clear();
        }

        /// <summary>
        /// 오브젝트를 활성화하는 메서드
        /// </summary>
        /// <returns></returns>
        public GameObject ActivatePoolItem(int _increaseCnt = 5, Transform _parentTr = null)
        {
            if (poolEnableList == null || poolDisableQueue == null) return null;

            // 모든 오브젝트가 활성화되어있는 상태라면 increaseCnt만큼 추가 생성
            if (ttlCnt == activeCnt)
                InstantiateObjects(_increaseCnt, _parentTr);

            GameObject poolGo = poolDisableQueue.Dequeue();
            poolEnableList.Add(poolGo);

            poolGo.SetActive(true);

            ++activeCnt;

            return poolGo;
        }

        /// <summary>
        /// 해당 오브젝트를 비활성화하는 메서드
        /// </summary>
        /// <param name="_removeObject"></param>
        public void DeactivatePoolItem(GameObject _removeObject)
        {
            if (poolEnableList == null || poolDisableQueue == null || _removeObject == null) return;

            int cnt = poolEnableList.Count;
            for (int i = 0; i < cnt; ++i)
            {
                GameObject poolGo = poolEnableList[i];
                if (poolGo == _removeObject)
                {
                    poolGo.SetActive(false);
                    poolEnableList.Remove(poolGo);
                    poolDisableQueue.Enqueue(poolGo);
                    poolGo.transform.position = tempPos;

                    --activeCnt;

                    return;
                }
            }
        }

        /// <summary>
        /// 활성화되어있는 모든 오브젝트를 비활성화하는 메서드
        /// </summary>
        public void DeactivateAllPoolItems()
        {
            if (poolEnableList == null || poolDisableQueue == null) return;

            int cnt = poolEnableList.Count;
            for (int i = 0; i < cnt; ++i)
            {
                GameObject poolGo = poolEnableList[i];

                poolGo.SetActive(false);
                poolGo.transform.position = tempPos;

                poolEnableList.Remove(poolGo);
                poolDisableQueue.Enqueue(poolGo);
            }

            activeCnt = 0;
        }

        /// <summary>
        /// 모든 오브젝트가 비활성화 상태인지를 반환하는 메서드
        /// </summary>
        /// <returns></returns>
        public bool IsEnableListEmpty()
        {
            return activeCnt < 1;
        }

        private int ttlCnt = 0;
        private int activeCnt = 0;

        private Vector3 tempPos = new Vector3(0.0f, -30.0f, 0.0f);
        private GameObject poolObject; // 오브젝트 풀링에서 관리하는 오브젝트 프리팹

        private List<GameObject> poolEnableList = null;
        private Queue<GameObject> poolDisableQueue = null;

    }
}