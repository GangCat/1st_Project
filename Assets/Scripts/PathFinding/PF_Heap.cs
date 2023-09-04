using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PF_Heap<T> where T : IHeapItem<T>
{

    public int Count => curItemCount;

    public PF_Heap(int _maxHeapSize)
    {
        items = new T[_maxHeapSize];
    }


    public void Add(T _item)
    {
        _item.HeapIdx = curItemCount;
        items[curItemCount] = _item;
        SortUp(_item);
        ++curItemCount;
    }

    public T RemoveFirstItem()
    {
        T firstItem = items[0];
        --curItemCount;
        items[0] = items[curItemCount];
        items[0].HeapIdx = 0;
        SortDown(items[0]);
        return firstItem;
    }

    /// <summary>
    /// Heap 내의 _item의 위치를 재설정
    /// </summary>
    /// <param name="_item"></param>
    public void UpdateItem(T _item)
    {
        SortUp(_item);
    }


    public bool Contains(T _item)
    {
        return Equals(items[_item.HeapIdx], _item);
    }

    /// <summary>
    /// 힙 위에서 아래로 내려가면서 자리를 찾아감
    /// </summary>
    /// <param name="_item"></param>
    void SortDown(T _item)
    {
        while (true)
        {
            int childIdxLeft = (_item.HeapIdx << 1) + 1;
            int childIdxRight = (_item.HeapIdx << 1) + 2;
            int swapIdx = 0;

            // 힙에서는 항상 좌측부터 채워지기 때문에 왼쪽 자식부터 체크
            if(childIdxLeft < curItemCount)
            {
                swapIdx = childIdxLeft;

                // 오른쪽 자식도 존재한다면
                if (childIdxRight < curItemCount)
                {
                    // 둘 중 더 높은 우선순위와 비교
                    if (items[childIdxLeft].CompareTo(items[childIdxRight]) < 0)
                        swapIdx = childIdxRight;
                }

                // 자식보다 우선순위가 더 낮다면 Swap
                if (_item.CompareTo(items[swapIdx]) < 0)
                    Swap(_item, items[swapIdx]);
                // 아니라면 자리를 찾았으니 종료
                else
                    return;
            }
            // 자식이 없다면 종료
            else
                return;
        }
    }

    /// <summary>
    /// 해당 아이템 정렬
    /// </summary>
    /// <param name="_item"></param>
    private void SortUp(T _item)
    {
        int parentIdx = (_item.HeapIdx - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIdx];

            // CompareTo
            // _item의 우선순위가 더 높으면 1, 같으면 0, 낮으면 -1을 반환한다.
            // 여기서는 우선순위를 fCost로 할것임.
            if (_item.CompareTo(parentItem) > 0)
                Swap(_item, parentItem);
            else
                break;

            parentIdx = (_item.HeapIdx - 1) / 2;
        }
    }

    void Swap(T _lhsItem, T _rhsItem)
    {
        items[_lhsItem.HeapIdx] = _rhsItem;
        items[_rhsItem.HeapIdx] = _lhsItem;

        int lhsItemIdx = _lhsItem.HeapIdx;
        _lhsItem.HeapIdx = _rhsItem.HeapIdx;
        _rhsItem.HeapIdx = lhsItemIdx;
    }

    private T[] items;
    private int curItemCount;
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIdx { get; set; }
}