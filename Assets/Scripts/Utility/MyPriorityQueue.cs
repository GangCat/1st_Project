using System;
using System.Collections.Generic;

public class MyPriorityQueue<T>
{   
    public MyPriorityQueue()
    {
        list = new List<T>();
        comparer = Comparer<T>.Default;
    }

    public void Enqueue(T value)
    {
        list.Add(value);
        int i = list.Count - 1;
        while (i > 0)
        {
            int parentIndex = (i - 1) / 2;
            if (comparer.Compare(list[i], list[parentIndex]) >= 0)
                break;

            T temp = list[i];
            list[i] = list[parentIndex];
            list[parentIndex] = temp;

            i = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (list.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");

        T frontItem = list[0];
        int lastIndex = list.Count - 1;
        list[0] = list[lastIndex];
        list.RemoveAt(lastIndex);

        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = currentIndex * 2 + 1;
            int rightChildIndex = currentIndex * 2 + 2;

            if (leftChildIndex >= list.Count)
                break;

            int smallerChildIndex = leftChildIndex;
            if (rightChildIndex < list.Count && comparer.Compare(list[leftChildIndex], list[rightChildIndex]) > 0)
                smallerChildIndex = rightChildIndex;

            if (comparer.Compare(list[currentIndex], list[smallerChildIndex]) <= 0)
                break;

            T temp = list[currentIndex];
            list[currentIndex] = list[smallerChildIndex];
            list[smallerChildIndex] = temp;

            currentIndex = smallerChildIndex;
        }

        return frontItem;
    }

    public int Count
    {
        get { return list.Count; }
    }


    private List<T> list;
    private IComparer<T> comparer;
}