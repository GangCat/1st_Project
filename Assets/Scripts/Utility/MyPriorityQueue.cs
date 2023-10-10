//using System;
//using System.Collections.Generic;

//public class MyPriorityQueue
//{   
//    public MyPriorityQueue()
//    {
//        list = new List<PF_PathRequestManager.SPathRequest>();
//        myComparer = new MyComparer();
//    }

//    public void Enqueue(PF_PathRequestManager.SPathRequest value)
//    {
//        list.Add(value);
//        int i = list.Count - 1;
//        while (i > 0)
//        {
//            int parentIndex = (i - 1) / 2;
//            //if (myComparer.Compare((int)list[i].objectType, (int)list[parentIndex].objectType) <= 0)
//            //    break;

//            PF_PathRequestManager.SPathRequest temp = list[i];
//            list[i] = list[parentIndex];
//            list[parentIndex] = temp;

//            i = parentIndex;
//        }
//    }

//    public PF_PathRequestManager.SPathRequest Dequeue()
//    {
//        if (list.Count == 0)
//            throw new InvalidOperationException("Priority queue is empty");

//        PF_PathRequestManager.SPathRequest frontItem = list[0];
//        int lastIndex = list.Count - 1;
//        list[0] = list[lastIndex];
//        list.RemoveAt(lastIndex);

//        int currentIndex = 0;
//        while (true)
//        {
//            int leftChildIndex = currentIndex * 2 + 1;
//            int rightChildIndex = currentIndex * 2 + 2;

//            if (leftChildIndex >= list.Count)
//                break;

//            int smallerChildIndex = leftChildIndex;
//            //if (rightChildIndex < list.Count && myComparer.Compare((int)list[leftChildIndex].objectType, (int)list[rightChildIndex].objectType) < 0)
//            //    smallerChildIndex = rightChildIndex;

//            //if (myComparer.Compare((int)list[currentIndex].objectType, (int)list[smallerChildIndex].objectType) >= 0)
//            //    break;

//            PF_PathRequestManager.SPathRequest temp = list[currentIndex];
//            list[currentIndex] = list[smallerChildIndex];
//            list[smallerChildIndex] = temp;

//            currentIndex = smallerChildIndex;
//        }

//        return list[0];
//    }

//    public int Count
//    {
//        get { return list.Count; }
//    }


//    private List<PF_PathRequestManager.SPathRequest> list = null;
//    private IMyComparer myComparer = null;
//}