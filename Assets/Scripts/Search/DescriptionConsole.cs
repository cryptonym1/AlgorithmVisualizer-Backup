using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionConsole : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public TMP_Dropdown SortAlgo;

    // Start is called before the first frame update
    void Start()
    {
        LinearSearch();
    }

    public void DropDownAlgo(int index)
    {
        switch (index)
        {
            case 0:
                LinearSearch();
                break;
                
            case 1:
                BinarySearch();
                break;
        }
    }

    void LinearSearch()
    {
        textMesh.text = "<b>Visualization Legend</b>";
        textMesh.text += "\n<color=yellow>Yellow</color> = During the linear search, when a cube is being examined but is not the target number, it is temporarily changed to yellow. This color is used to highlight the cube being checked in the search process.";
        textMesh.text += "\n<color=green>Green</color> = When the target number is found in the cubeList, the cube containing the target number is changed to green. This color represents the successful finding of the target number.";
        textMesh.text += "\n<color=red>Red</color> = If a cube is examined during the search process, but it does not contain the target number, it is temporarily changed to red. This color is used to indicate that the cube does not match the target number";
    
        textMesh.text += "\n<color=red><b>Linear Search Algorithm</b></color>";
        textMesh.text += "\nLinear search is also called as sequential search algorithm. It is the simplest searching algorithm. In Linear search, we simply traverse the list completely and match each element of the list with the item whose location is to be found. If the match is found, then the location of the item is returned; otherwise, the algorithm returns NULL.\n\nIt is widely used to search an element from the unordered list, i.e., the list in which items are not sorted. The worst-case time complexity of linear search is O(n).";

        textMesh.text += "\n\n<color=red><b>Time Complexity</b></color>\n";
        textMesh.text += "\n<b><color=red>Best Case Complexity</color></b> - In Linear search, best case occurs when the element we are finding is at the first position of the array. The best-case time complexity of linear search is O(1).";
        textMesh.text += "\n<b><color=red>Average Case Complexity</color></b> - The average case time complexity of linear search is O(n).";
        textMesh.text += "\n<b><color=red>Worst Case Complexity</color></b> - In Linear search, the worst case occurs when the element we are looking is present at the end of the array. The worst-case in linear search could be when the target element is not present in the given array, and we have to traverse the entire array. The worst-case time complexity of linear search is O(n).";
    
        textMesh.text += "\n\n<color=red><b>Algorithm (C++)</b></color>\n";
        textMesh.text += "#include<iostream>\n";
        textMesh.text += "using namespace std;\n\n";
        textMesh.text += "int main() {\n";
        textMesh.text += "  int arr[10], i, num, index;\n";
        textMesh.text += "  cout << \"Enter 10 Numbers: \";\n";
        textMesh.text += "  for (i = 0; i < 10; i++)\n";
        textMesh.text += "    cin >> arr[i];\n\n";
        textMesh.text += "  cout << \"\\nEnter a Number to Search: \";\n";
        textMesh.text += "  cin >> num;\n\n";
        textMesh.text += "  for (i = 0; i < 10; i++) {\n";
        textMesh.text += "    if (arr[i] == num) {\n";
        textMesh.text += "      index = i;\n";
        textMesh.text += "      break;\n";
        textMesh.text += "    }\n";
        textMesh.text += "  }\n\n";
        textMesh.text += "  cout << \"\\nFound at Index No.\" << index;\n";
        textMesh.text += "  cout << endl;\n";
        textMesh.text += "  return 0;\n";
        textMesh.text += "}";
    }


    void BinarySearch()
    {
        textMesh.text = "<b>Visualization Legend</b>";
        textMesh.text += "\n<color=yellow>Yellow</color> = When a cube is highlighted with the color yellow, it means that it is being examined or considered during the binary search. The color yellow indicates the current middle element being checked.";
        textMesh.text += "\n<color=green>Green</color> = If the number being searched for (numberToFind) is found in the binaryNum list, the cube representing that number is scaled up and then colored green. This color indicates that the desired number has been found.";
        textMesh.text += "\n<color=red>Red</color> = If the number being searched for is not found at the current middle index, the cube is scaled up and then colored red. This color represents that the current middle element does not match the desired number.";
    
        textMesh.text += "\n<color=red><b>Binary Search Algorithm</b></color>";
        textMesh.text += "\nBinary search is the search technique that works efficiently on sorted lists. Hence, to search an element into some list using the binary search technique, we must ensure that the list is sorted.\n\nBinary search follows the divide and conquer approach in which the list is divided into two halves, and the item is compared with the middle element of the list. If the match is found then, the location of the middle element is returned. Otherwise, we search into either of the halves depending upon the result produced through the match.";

        textMesh.text += "\n\n<color=red><b>Time Complexity</b></color>\n";
        textMesh.text += "\n<b><color=red>Best Case Complexity</color></b> - In Binary search, best case occurs when the element to search is found in first comparison, i.e., when the first middle element itself is the element to be searched. The best-case time complexity of Binary search is O(1).";
        textMesh.text += "\n<b><color=red>Average Case Complexity</color></b> - The average case time complexity of Binary search is O(logn).";
        textMesh.text += "\n<b><color=red>Worst Case Complexity</color></b> - In Binary search, the worst case occurs, when we have to keep reducing the search space till it has only one element. The worst-case time complexity of Binary search is O(logn).";

        textMesh.text += "#include<iostream>\n";
        textMesh.text += "using namespace std;\n\n";
        textMesh.text += "int main() {\n";
        textMesh.text += "  int i, arr[10], num, first, last, middle;\n";
        textMesh.text += "  cout << \"Enter 10 Elements (in ascending order): \";\n";
        textMesh.text += "  for (i = 0; i < 10; i++)\n";
        textMesh.text += "    cin >> arr[i];\n\n";
        textMesh.text += "  cout << \"\\nEnter Element to be Search: \";\n";
        textMesh.text += "  cin >> num;\n";
        textMesh.text += "  first = 0;\n";
        textMesh.text += "  last = 9;\n";
        textMesh.text += "  middle = (first + last) / 2;\n";
        textMesh.text += "  while (first <= last) {\n";
        textMesh.text += "    if (arr[middle] < num)\n";
        textMesh.text += "      first = middle + 1;\n";
        textMesh.text += "    else if (arr[middle] == num) {\n";
        textMesh.text += "      cout << \"\\nThe number, \" << num << \" found at Position \" << middle + 1;\n";
        textMesh.text += "      break;\n";
        textMesh.text += "    }\n";
        textMesh.text += "    else\n";
        textMesh.text += "      last = middle - 1;\n";
        textMesh.text += "    middle = (first + last) / 2;\n";
        textMesh.text += "  }\n\n";
        textMesh.text += "  if (first > last)\n";
        textMesh.text += "    cout << \"\\nThe number, \" << num << \" is not found in the given Array\";\n";
        textMesh.text += "  cout << endl;\n";
        textMesh.text += "  return 0;\n";
        textMesh.text += "}";
    }
}
