using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<I> where I : IHeapItem<I> //clase Heap de tipo I (I puede ser cualquier cosa)
{
    I[] items;//array de I´s
    int currentItemCount;
    public Heap(int maxHeapSize)//constructor
    {
        items = new I[maxHeapSize]; //declarar tamaño del array de I´s
    }
    
    public void Add(I item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public I RemoveFirst()
    {
        I firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(I item)
    {
        SortUp(item);
        
    }

    public bool Contains(I item)
    {
        return Equals(items[item.HeapIndex],item);
    }

    public int Count{ get {return currentItemCount;}}

    void SortDown(I item)
    {
       while(true)
       {
            int leftChildIndex = 2 * item.HeapIndex + 1;
            int rightChildIndex = 2 * item.HeapIndex + 2;
            int swapIndex = 0; 
            if(leftChildIndex < currentItemCount)
            {
                swapIndex = leftChildIndex;
                if(rightChildIndex < currentItemCount)
                {
                    if(items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }
                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                    return;
            }
            else
                return;
       }

    }
    void SortUp(I item)
    {
        int parentIndex = (item.HeapIndex - 1)/2;
        while(true)
        {
            I parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item,parentItem);
            }
            else
                break;
            parentIndex = (item.HeapIndex - 1)/2; 
        }
    }

    void Swap(I itemA, I itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<I> : IComparable<I> {
        int HeapIndex{ get; set;}
    }