using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Description : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public TMP_Dropdown SortAlgo;

    public void DropDownAlgo(int index)
    {
        switch (index)
        {
            case 0:
                BubbleSort();
                break;
                
            case 1:
                QuickSort();
                break;

            case 2:
                SelectionSort();
                break;

            case 3:
                InsertionSort();
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BubbleSort();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BubbleSort()
    {
        textMesh.text = "<b>Visualization Legend</b>";
        textMesh.text += "\n<color=yellow>Yellow</color> = The current element being compared with the next element is colored yellow. This helps visually distinguish the elements being compared.\n";
        textMesh.text += "<color=blue>Blue</color> = The next element being compared with the current element is colored blue. This helps identify the element being compared with the current element.\n";

        textMesh.text += "\n<color=red><b>Bubble Sort Algorithm</b></color>";
        textMesh.text += "\nBubble sort works on the repeatedly swapping of adjacent elements until they are not in the intended order. It is called bubble sort because the movement of array elements is just like the movement of air bubbles in the water. Bubbles in water rise up to the surface; similarly, the array elements in bubble sort move to the end in each iteration.";
        textMesh.text += "\nAlthough it is simple to use, it is primarily used as an educational tool because the performance of bubble sort is poor in the real world. It is not suitable for large data sets. The average and worst-case complexity of Bubble sort is O(n2), where n is a number of items.";
    
        textMesh.text += "\n\n<color=red><b>Time Complexity</b></color>\n";
        textMesh.text += "<b><color=red>Best Case Complexity</color></b> - It occurs when there is no sorting required, i.e. the array is already sorted. The best-case time complexity of bubble sort is O(n).\n\n";
        textMesh.text += "<b><color=red>Average Case Complexity</color></b> - It occurs when the array elements are in jumbled order that is not properly ascending and not properly descending. The average case time complexity of bubble sort is O(n^2).\n\n";
        textMesh.text += "<b><color=red>Worst Case Complexity</color></b> - It occurs when the array elements are required to be sorted in reverse order. That means suppose you have to sort the array elements in ascending order, but its elements are in descending order. The worst-case time complexity of bubble sort is O(n^2).";

        textMesh.text += "\n\n<color=red><b>Algorithm (C++)</b></color>\n";
        textMesh.text += "<color=green>// loop to access each array element</color>\n";
        textMesh.text += "<color=blue>for</color> (<color=orange>int</color> step = 0; step < size; ++step) {\n";
        textMesh.text += "    <color=green>// loop to compare array elements</color>\n";
        textMesh.text += "    <color=blue>for</color> (<color=orange>int</color> i = 0; i < size - step; ++i) {\n";
        textMesh.text += "        <color=green>// compare two adjacent elements</color>\n";
        textMesh.text += "        <color=green>// change > to < to sort in descending order</color>\n";
        textMesh.text += "        <color=blue>if</color> (array[i] > array[i + 1]) {\n";
        textMesh.text += "            <color=green>// swapping elements if elements are not in the intended order</color>\n";
        textMesh.text += "            <color=orange>int</color> temp = array[i];\n";
        textMesh.text += "            array[i] = array[i + 1];\n";
        textMesh.text += "            array[i + 1] = temp;\n";
        textMesh.text += "        }\n";
        textMesh.text += "    }\n";
        textMesh.text += "}\n";
        }

    void QuickSort()
    {
        textMesh.text = "<b>Visualization Legend</b>";
        textMesh.text += "\n<color=red>Red</color> =  The pivot element is colored red. This helps visually identify the pivot element during the partitioning process.\n";
        textMesh.text += "<color=blue>Blue</color> = Elements being compared with the pivot are colored blue. This helps distinguish the elements being compared.\n";
        textMesh.text += "<color=yellow>Yellow</color> = Elements that are swapped with each other are temporarily colored yellow. This helps indicate the swapping operation.\n";

        textMesh.text += "\n<color=red><b>Quick Sort Algorithm</b></color>\n";
        textMesh.text += "Quicksort is the widely used sorting algorithm that makes n log n comparisons in average case for sorting an array of n elements. It is a faster and highly efficient sorting algorithm. This algorithm follows the divide and conquer approach. Divide and conquer is a technique of breaking down the algorithms into subproblems, then solving the subproblems, and combining the results back together to solve the original problem.\n\n";
        textMesh.text += "<b><color=red>Divide:</color></b> In Divide, first pick a pivot element. After that, partition or rearrange the array into two sub-arrays such that each element in the left sub-array is less than or equal to the pivot element and each element in the right sub-array is larger than the pivot element.\n";
        textMesh.text += "<b><color=red>Conquer:</color></b> Recursively, sort two subarrays with Quicksort.\n";
        textMesh.text += "<b><color=red>Combine:</color></b> Combine the already sorted array.\n\n";        

        textMesh.text += "<b><color=red>Choosing the pivot</color></b> - Picking a good pivot is necessary for the fast implementation of quicksort. However, it is typical to determine a good pivot. Some of the ways of choosing a pivot are as follows -\n";
        textMesh.text += "* Pivot can be random, i.e. select the random pivot from the given array.\n";
        textMesh.text += "* Pivot can either be the rightmost element or the leftmost element of the given array.\n";
        textMesh.text += "* Select median as the pivot element.\n";
        textMesh.text += "(Note that in the visualization, we always use the rightmost element as the pivot).\n";

        textMesh.text += "\n<color=red><b>Time Complexity</b></color>\n";
        textMesh.text += "<color=red><b>Best Case Complexity</b></color> - In Quicksort, the best-case occurs when the pivot element is the middle element or near to the middle element. The best-case time complexity of quicksort is O(n*logn).\n";
        textMesh.text += "<color=red><b>Average Case Complexity</b></color> - It occurs when the array elements are in jumbled order that is not properly ascending and not properly descending. The average case time complexity of quicksort is O(n*logn).\n";
        textMesh.text += "<color=red><b>Worst Case Complexity</b></color> - In quick sort, worst case occurs when the pivot element is either greatest or smallest element. Suppose, if the pivot element is always the last element of the array, the worst case would occur when the given array is sorted already in ascending or descending order. The worst-case time complexity of quicksort is O(n2).\n";

        textMesh.text += "\n<color=red><b>Algorithm (C++)</b></color>\n";
        textMesh.text += "<color=green>// Swap two values</color>\n";
        textMesh.text += "<color=green>void</color> swap(<color=blue>int</color> *a, <color=blue>int</color> *b) {\n";
        textMesh.text += "  <color=blue>int</color> t = *a;\n";
        textMesh.text += "  *a = *b;\n";
        textMesh.text += "  *b = t;\n";
        textMesh.text += "}\n\n";
        textMesh.text += "<color=green>// Rearrange the array by finding the partition point</color>\n";
        textMesh.text += "<color=blue>int</color> partition(<color=blue>int</color> array[], <color=blue>int</color> low, <color=blue>int</color> high) {\n";
        textMesh.text += "  <color=blue>int</color> pivot = array[high];\n";
        textMesh.text += "  <color=blue>int</color> i = (low - 1);\n\n";
        textMesh.text += "  <color=green>// Compare elements with the pivot and swap if necessary</color>\n";
        textMesh.text += "  <color=blue>for</color> (<color=blue>int</color> j = low; j < high; j++) {\n";
        textMesh.text += "    <color=green>if</color> (array[j] <= pivot) {\n";
        textMesh.text += "      i++;\n";
        textMesh.text += "      swap(&array[i], &array[j]);\n";
        textMesh.text += "    }\n";
        textMesh.text += "  }\n\n";
        textMesh.text += "  swap(&array[i + 1], &array[high]);\n\n";
        textMesh.text += "  <color=green>// Return the partition point</color>\n";
        textMesh.text += "  <color=blue>return</color> (i + 1);\n";
        textMesh.text += "}\n\n";
        textMesh.text += "<color=green>// Perform the QuickSort algorithm recursively</color>\n";
        textMesh.text += "void quickSort(<color=blue>int</color> array[], <color=blue>int</color> low, <color=blue>int</color> high) {\n";
        textMesh.text += "  <color=blue>if</color> (low < high) {\n";
        textMesh.text += "    <color=blue>int</color> pi = partition(array, low, high);\n\n";
        textMesh.text += "    quickSort(array, low, pi - 1);\n";
        textMesh.text += "    quickSort(array, pi + 1, high);\n";
        textMesh.text += "  }\n";
        textMesh.text += "}\n";
    }

    void SelectionSort()
    {
        textMesh.text = "<b>Visualization Legend</b>";
        textMesh.text += "\n<color=blue>Blue</color> = The current minimum element being compared is colored blue. This helps identify the element being compared with the rest of the array.\n";
        textMesh.text += "<color=yellow>Yellow</color> = Elements that are compared with the current minimum element are colored yellow. This helps distinguish the elements being compared.\n";

        textMesh.text += "\n<color=red><b>Selection Sort Algorithm</b></color>\n";
        textMesh.text += "In selection sort, the smallest value among the unsorted elements of the array is selected in every pass and inserted into its appropriate position in the array. It is also the simplest algorithm and an in-place comparison sorting algorithm. The array is divided into two parts: the sorted part on the left and the unsorted part on the right.\n";
        textMesh.text += "In each iteration, the smallest element from the unsorted part is selected and placed at the end of the sorted part. This process continues until the array is completely sorted.\n";
        textMesh.text += "The average and worst-case complexity of selection sort is O(n^2), where n is the number of items. Due to this, it is not suitable for large data sets.\n";

        textMesh.text += "\n<color=red><b>Time Complexity</b></color>\n";
        textMesh.text += "<color=red><b>Best Case Complexity</b></color> - It occurs when there is no sorting required, i.e., the array is already sorted. The best-case time complexity of selection sort is O(n^2).\n\n";
        textMesh.text += "<color=red><b>Average Case Complexity</b></color> - It occurs when the array elements are in jumbled order that is not properly ascending and not properly descending. The average case time complexity of selection sort is O(n^2).\n\n";
        textMesh.text += "<color=red><b>Worst Case Complexity</b></color> - It occurs when the array elements are required to be sorted in reverse order. That means, suppose you have to sort the array elements in ascending order, but its elements are in descending order. The worst-case time complexity of selection sort is O(n^2).";

        textMesh.text += "\n\n<color=red><b>Algorithm (C++)</b></color>\n";
        textMesh.text += "<color=blue>// function to swap the position of two elements</color>\n";
        textMesh.text += "void <color=orange>swap</color>(int *a, int *b) {\n";
        textMesh.text += "  int temp = *a;\n";
        textMesh.text += "  *a = *b;\n";
        textMesh.text += "  *b = temp;\n";
        textMesh.text += "}\n\n";
        textMesh.text += "void <color=orange>selectionSort</color>(int array[], int size) {\n";
        textMesh.text += "  for (int step = 0; step < size - 1; step++) {\n";
        textMesh.text += "    int min_idx = step;\n";
        textMesh.text += "    for (int i = step + 1; i < size; i++) {\n";
        textMesh.text += "      <color=blue>// To sort in descending order, change > to < in this line.</color>\n";
        textMesh.text += "      <color=blue>// Select the minimum element in each loop.</color>\n";
        textMesh.text += "      if (array[i] < array[min_idx])\n";
        textMesh.text += "        min_idx = i;\n";
        textMesh.text += "    }\n\n";
        textMesh.text += "    <color=blue>// put min at the correct position</color>\n";
        textMesh.text += "    <color=orange>swap</color>(&array[min_idx], &array[step]);\n";
        textMesh.text += "  }\n";
        textMesh.text += "}";
    }
    
    void InsertionSort()
    {
        textMesh.text = "<b>Visualization Legend</b>";
        textMesh.text += "\n<color=red>Red</color> = The key element is highlighted red, it indicates that the cube is currently being compared or moved.\n";
        textMesh.text += "<color=blue>Blue</color> = The blue color is used to highlight the predecessor cube during the sorting process. It helps visualize the comparison between the current cube and its predecessor.\n";
    
        textMesh.text += "\n<b><color=red>Insertion Sort:</color></b>\n" +
            "Insertion sort works similar to the sorting of playing cards in hands. It is assumed that the first card is already sorted in the card game, and then we select an unsorted card. If the selected unsorted card is greater than the first card, it will be placed at the right side; otherwise, it will be placed at the left side. Similarly, all unsorted cards are taken and put in their exact place.\n" +
            "The same approach is applied in insertion sort. The idea behind the insertion sort is that first take one element, iterate it through the sorted array. Although it is simple to use, it is not appropriate for large data sets as the time complexity of insertion sort in the average case and worst case is O(n^2), where n is the number of items. Insertion sort is less efficient than the other sorting algorithms like heap sort, quick sort, merge sort, etc.";

        textMesh.text += "\n\n<color=red><b>Time Complexity</b></color>\n";
        textMesh.text += "<color=red><b>Best Case Complexity</b></color> - It occurs when there is no sorting required, i.e. the array is already sorted. The best-case time complexity of insertion sort is O(n).\n" +
            "<color=red><b>Average Case Complexity</b></color> - It occurs when the array elements are in jumbled order that is not properly ascending and not properly descending. The average case time complexity of insertion sort is O(n^2).\n" +
            "<color=red><b>Worst Case Complexity</b></color> - It occurs when the array elements are required to be sorted in reverse order. That means suppose you have to sort the array elements in ascending order, but its elements are in descending order. The worst-case time complexity of insertion sort is O(n^2).";

        textMesh.text += "\n\n<color=red><b>Algorithm (C++)</b></color>\n";
        textMesh.text += "void <color=orange>insertionSort</color>(<color=green>int</color> <color=yellow>array</color>[], <color=green>int</color> <color=yellow>size</color>) {\n";
        textMesh.text += "  <color=red>for</color> (<color=green>int</color> <color=yellow>step</color> = 1; <color=yellow>step</color> < <color=yellow>size</color>; <color=yellow>step</color>++) {\n";
        textMesh.text += "    <color=green>int</color> <color=yellow>key</color> = <color=yellow>array</color>[<color=yellow>step</color>];\n";
        textMesh.text += "    <color=green>int</color> <color=yellow>j</color> = <color=yellow>step</color> - 1;\n\n";
        textMesh.text += "    <color=blue>// Compare key with each element on the left of it until an element smaller than it is found.</color>\n";
        textMesh.text += "    <color=blue>// For descending order, change key<array[j] to key>array[j].</color>\n";
        textMesh.text += "    <color=red>while</color> (<color=yellow>key</color> < <color=yellow>array</color>[<color=yellow>j</color>] && <color=yellow>j</color> >= 0) {\n";
        textMesh.text += "      <color=yellow>array</color>[<color=yellow>j</color> + 1] = <color=yellow>array</color>[<color=yellow>j</color>];\n";
        textMesh.text += "      --<color=yellow>j</color>;\n";
        textMesh.text += "    }\n";
        textMesh.text += "    <color=yellow>array</color>[<color=yellow>j</color> + 1] = <color=yellow>key</color>;\n";
        textMesh.text += "  }\n";
        textMesh.text += "}";
    }
}
