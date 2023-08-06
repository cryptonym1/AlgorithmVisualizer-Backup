using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sorting : MonoBehaviour
{
    public GameObject cubePrefab;
    bool clickedStartSort = false;
    bool randomized = true;
    bool isSorting = false;
    bool starting = true;
    int algoSelected;

    private List<GameObject> cubes;
    private List<GameObject> tempCubes;
    private HashSet<int> usedNumbers;
    private int[] numInput;

    public TMP_InputField inputField;
    public float cameraSize = 5;
    public Camera secondCam;
    public int greatNum;
    public TMP_Dropdown dropDownAlgo;
    public TMP_Dropdown dropDownSort;
    public List<int> tempNum;
    public TextMeshProUGUI textMeshPro;

    Renderer rend;

    [SerializeField] public Slider slider;
    [SerializeField] int sortTypeSelected;
    [SerializeField] private float speed;
    [SerializeField] public int numberOfCubes;
    [SerializeField] private Slider _slider;

    private System.String _string;

    GameObject temp;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) => {
            _string = v.ToString();
            numberOfCubes = int.Parse(_string);
            randomized = false;
            Init();
            randomized = true;
        });
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        inputField.onSubmit.AddListener(OnSubmitInput);
        tempCubes = new List<GameObject>();
        cubes = new List<GameObject>();
        tempNum = new List<int>();
        usedNumbers = new HashSet<int>(); 
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        SliderVal();
    }


/////////////////////////////////////////////////////////////////////////////..Cube Creator..//////////////////////////////////////////////////////////////////////////

    private void OnInputFieldEndEdit(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return; 
        }

        string[] inputArray = input.Split(new char[] { ' ', ',', '"', '.', '/', '?', ';' }, System.StringSplitOptions.RemoveEmptyEntries);

        OnSubmitInput(input);
    }

    private void OnSubmitInput(string input)
    {
        textMeshPro.text = "";

        if (string.IsNullOrEmpty(input))
        {
            return; 
        }

        string[] inputArray = input.Split(new char[] { ' ', ',', '"', '.', '/', '?', ';'}, System.StringSplitOptions.RemoveEmptyEntries);

        numInput = new int[inputArray.Length];

        for (int i = 0; i < inputArray.Length; i++)
        {
            if (int.TryParse(inputArray[i], out int value))
            {
                if (value >= 1)
                {
                    numInput[i] = value;
                }
                else
                {
                    inputField.text = "";
                    textMeshPro.text = ("Invalid input: " + inputArray[i] + ". Please input positive integers only.");
                    return;
                }
            }
            else
            {
                inputField.text = "";
                textMeshPro.text = ("Invalid input: " + inputArray[i] + "\nPlease input positive integers only.");
                return;
            }
        }

        if (numInput.Length < 2)
        {
            inputField.text = "";
            textMeshPro.text = "Input array size must be 2 or more.";
            return;
        }

        numberOfCubes = numInput.Length;

        inputInit();
        inputField.text = "";
    }

    void inputInit()
    {
        StopAllCoroutines();

        if(cubes != null)
        {
            for (int i = 0; i < cubes.Count; i++)
                Destroy(cubes[i].gameObject);
            cubes.Clear();
        }

        if (tempCubes != null)
        {
            for (int i = 0; i < tempCubes.Count; i++)
            {
                Destroy(tempCubes[i].gameObject);
            }
            tempCubes.Clear();
            tempNum.Clear();
        }

        float greatNum = 0;

        for(int i=0; i<numberOfCubes; i++)
        {
            tempNum.Add(numInput[i]);

            if(numInput[i] > greatNum)
            {
                greatNum = numInput[i];
            }

            // Instantiate a new cube using the prefab
            GameObject cube = Instantiate(cubePrefab, transform);
            // Add a BoxCollider component to the cube
            BoxCollider collider = cube.AddComponent<BoxCollider>();
            cube.transform.localScale = new Vector3(0.9f, numInput[i] , 1.0f);
            cube.transform.position = new Vector3(i, numInput[i] / 2.0f, 0);

            cube.transform.SetParent(this.transform);
            cube.name = "Cube " + i;

            cubes.Add(cube);
            tempCubes.Add(cube);

            GameObject childObj = new GameObject();

            childObj.transform.parent = cube.transform;
            childObj.name = "Text Holder";

            TextMeshPro textMesh = childObj.AddComponent<TextMeshPro>();
            textMesh.text = numInput[i].ToString();
            //textMesh.characterSize = 0.04f;
            textMesh.fontStyle = FontStyles.Bold;
            textMesh.fontSize = 5f;
            textMesh.color = Color.black;

            //textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.transform.position = new Vector3(cube.transform.position.x,  0.5f, -cube.transform.position.z - 1);
        }

        // Set the position of the second camera
        float camX = (numberOfCubes - 1) / 2.0f;
        float camY = greatNum / 2.0f;
        float camZ = -greatNum;
        secondCam.transform.position = new Vector3(camX, camY, camZ);

        // Set the orthographic size of the second camera
        float orthographicSize = greatNum / 2;
        secondCam.orthographicSize = orthographicSize;
    }

    void Init()
    {
        UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        // Get the URP default shader
        Shader defaultShader = urpAsset.defaultShader;

        StopAllCoroutines();
        
        if (cubes != null)
        {
            for (int i = 0; i < cubes.Count; i++)
                Destroy(cubes[i].gameObject);
            cubes.Clear();
        }

        usedNumbers.Clear();

        float greatNum = 0;

        float maxCubeSize = 0;

        if (cubes.Count > 0)
        {
            maxCubeSize = Mathf.Max(cubes.Select(cube => cube.transform.localScale.y).ToArray());
        }

        cameraSize = maxCubeSize * 0.5f;

        if (randomized == true && starting == false)
        {
            for (int i = 0; i < numberOfCubes; i++)
            {
                if (tempNum[i] > greatNum)
                {
                    greatNum = tempNum[i];
                }

                // Instantiate a new cube using the prefab
                GameObject cube = Instantiate(cubePrefab, transform);
                // Add a BoxCollider component to the cube
                BoxCollider collider = cube.AddComponent<BoxCollider>();
                cube.transform.localScale = new Vector3(0.9f, tempNum[i] , 1.0f);
                cube.transform.position = new Vector3(i, tempNum[i] / 2.0f, 0);

                cube.transform.SetParent(this.transform);
                cube.name = "Cube " + i;

                cubes.Add(cube);
                tempCubes.Add(cube);

                GameObject childObj = new GameObject();

                childObj.transform.parent = cube.transform;
                childObj.name = "Text Holder";

                TextMeshPro textMesh = childObj.AddComponent<TextMeshPro>();
                textMesh.text = tempNum[i].ToString();
                textMesh.fontStyle = FontStyles.Bold;
                textMesh.fontSize = 5f;
                textMesh.color = Color.black;

                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.transform.position = new Vector3(cube.transform.position.x, 0.5f, -cube.transform.position.z - 1);
            }
        }
        
        
        if (!clickedStartSort)
        {
            if (tempCubes != null)
            {
                for (int i = 0; i < tempCubes.Count; i++)
                {
                    Destroy(tempCubes[i].gameObject);
                }
                tempCubes.Clear();
                tempNum.Clear();
            }

            for (int i = 0; i < numberOfCubes; i++)
            {
                int randomNumber;
                do
                {
                    randomNumber = Random.Range(1, numberOfCubes + 1);
                }
                while (usedNumbers.Contains(randomNumber)); 

                usedNumbers.Add(randomNumber);
                tempNum.Add(randomNumber);

                if (randomNumber > greatNum)
                {
                    greatNum = randomNumber;
                }

                // Instantiate a new cube using the prefab
                GameObject cube = Instantiate(cubePrefab, transform);

                // Add a BoxCollider component to the cube
                BoxCollider collider = cube.AddComponent<BoxCollider>();

                cube.transform.localScale = new Vector3(0.9f, randomNumber , 1.0f);
                cube.transform.position = new Vector3(i, randomNumber / 2.0f, 0);

                cube.transform.SetParent(this.transform);
                cube.name = "Cube " + i;

                // Add a MeshRenderer component to the cube
                MeshRenderer renderer = cube.GetComponent<MeshRenderer>();

                // Set the cube's material to a new white material
                Material material = new Material(defaultShader);
                material.color = Color.white;
                renderer.material = material;

                GameObject childObj = new GameObject();

                childObj.transform.parent = cube.transform;
                childObj.name = "Text Holder";

                TextMeshPro textMesh = childObj.AddComponent<TextMeshPro>();
                textMesh.text = randomNumber.ToString();
                textMesh.fontStyle = FontStyles.Bold;
                textMesh.fontSize = 5f;
                textMesh.color = Color.black;

                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.transform.position = new Vector3(cube.transform.position.x, 0.5f, -cube.transform.position.z - 1);

                cubes.Add(cube);
                tempCubes.Add(cube);
                starting = false;
            }
        }

        // Set the position of the second camera
        float camX = (numberOfCubes - 1) / 2.0f;
        float camY = greatNum / 2.0f;
        float camZ = -greatNum;
        secondCam.transform.position = new Vector3(camX, camY, camZ);

        // Set the orthographic size of the second camera
        float orthographicSize = greatNum / 2;
        secondCam.orthographicSize = orthographicSize;
    }

    
////////////////////////////////////////////////////////////////////////////..End Cube Creator../////////////////////////////////////////////////////////////////////////

    // Slider ///////////////////////////////////////////////////////////////
    public void SliderVal()
    {
        Time.timeScale=(int)(slider.value);
    }

    // DropDown /////////////////////////////////////////////////////////////
    public void DropDownAlgo(int index)
    {
        switch (index)
        {
            case 0:
                clickedStartSort = true;
                Init();
                clickedStartSort = false;
                algoSelected = 0;
                break;
                
            case 1:
                clickedStartSort = true;
                Init();
                clickedStartSort = false;
                algoSelected = 1;
                break;

            case 2:
                clickedStartSort = true;
                Init();
                clickedStartSort = false;
                algoSelected = 2;
                break;

            case 3:
                clickedStartSort = true;
                Init();
                clickedStartSort = false;
                algoSelected = 3;
                break;
        }
    }

    public void DropDownSort(int index)
    {
        switch (index)
        {
            case 0: 
                clickedStartSort = true;
                Init();
                clickedStartSort = false;
                sortTypeSelected = 0;
                break;
            case 1: 
                clickedStartSort = true;
                Init();
                clickedStartSort = false;
                sortTypeSelected = 1;
                break;
        }
    }

    // Sort Button ////////////////////////////////////////////////////////////
    public void StartSort()
    {
        if(isSorting == false)
        {
            textMeshPro.text = "";
            clickedStartSort = true;
            disableUI();
            Init();
        }

        if(sortTypeSelected == 0 && isSorting == false)
        {
            StartCoroutine(SortAscending());
        }
        else if(sortTypeSelected == 1 && isSorting == false)
        {
            StartCoroutine(SortDescending());
        }

        clickedStartSort = false;
    }

    public void enableUI()
    {
        dropDownAlgo.enabled = true;
        dropDownSort.enabled = true;
        inputField.enabled = true;
        _slider.enabled = true;
    }

    public void disableUI()
    {
        dropDownAlgo.enabled = false;
        dropDownSort.enabled = false;
        inputField.enabled = false;
        _slider.enabled = false;
    }

    // Ascending  //////////////////////////////////////////////////////////////
    IEnumerator SortAscending()
    {
        if(algoSelected == 0)
        {
            yield return new WaitForSeconds(1f);
            isSorting = true;
            StartCoroutine(BubbleSort(cubes));
        }
        else if(algoSelected == 1)
        {
            StartCoroutine(OutputFirst());
            yield return new WaitForSeconds(1f);
            isSorting = true;
            StartCoroutine(QuickSort(cubes, 0, cubes.Count - 1));
        }
        else if(algoSelected == 2)
        {
            yield return new WaitForSeconds(1f);
            isSorting = true;
            StartCoroutine(SelectionSort(cubes));
        }
        else if(algoSelected == 3)
        {
            yield return new WaitForSeconds(1f);
            isSorting = true;
            StartCoroutine(InsertionSort(cubes));
        }
        yield return new WaitForSeconds(0.5f);
    }

    // Descending  ///////////////////////////////////////////////////////////////
    IEnumerator SortDescending()
    {
        if(algoSelected == 0)
        {
            yield return new WaitForSeconds(0.3f);
            isSorting = true;
            StartCoroutine(DesBubbleSort(cubes));
        }
        else if(algoSelected == 1)
        {
            StartCoroutine(DesOutputFirst());
            yield return new WaitForSeconds(0.3f);
            isSorting = true;
            StartCoroutine(DesQuickSort(cubes, 0, cubes.Count - 1));
        }
        else if(algoSelected == 2)
        {
            yield return new WaitForSeconds(0.3f);
            isSorting = true;
            StartCoroutine(DesSelectionSort(cubes));
        }
        else if(algoSelected == 3)
        {
            yield return new WaitForSeconds(0.3f);
            isSorting = true;
            StartCoroutine(DesInsertionSort(cubes));
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void randomizeArray()
    {
        if(isSorting == false)
        {
            textMeshPro.text = "";
            Init();
            randomized = true;
        }
    }

    // Tools ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator Complete()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            yield return new WaitForSeconds(0.05f);
            LeanTween.color(cubes[i], Color.green, 0.1f).setEase(LeanTweenType.easeOutQuad);
        }

        enableUI();
        yield return new WaitForSeconds(0.05f);
        isSorting = false;
        StopAllCoroutines();
    }

    bool IsSorted(List<GameObject> c)
    {
        for (int i = 0; i < c.Count - 1; i++)
        {
            if (c[i].transform.localScale.y > c[i + 1].transform.localScale.y)
            {
                return false;
            }
        }
            return true;
    }

    bool DesIsSorted(List<GameObject> c)
    {
        int n = c.Count;
        for (int i = 1; i < n; i++)
        {
            if (c[i].transform.localScale.y > c[i - 1].transform.localScale.y)
            {
                return false;
            }
        }
        return true;
    }


    void Swap(List<GameObject> c, int i, int j)
    {
        GameObject temp = c[i];
        Vector3 tempPosition = c[i].transform.localPosition;

        LeanTween.moveLocalX(c[i], c[j].transform.localPosition.x, 0.3f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveLocalZ(c[i], -1.5f, 0.3f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad);
        c[i] = c[j];

        LeanTween.moveLocalX(c[j], tempPosition.x, 0.3f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveLocalZ(c[j], 1.5f, 0.3f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad);
        c[j] = temp;

        LeanTween.color(c[j], Color.white, 0.2f).setEase(LeanTweenType.easeOutQuad);
    }

    public void stopVisualizing()
    {
        if(isSorting)
        {
            clickedStartSort = true;
            StopAllCoroutines();
            Init();
            enableUI();
            isSorting = false;
            clickedStartSort = false;
        }
    }



///////////////////////////////////////////////////////////////////////Sorting Algorithms/////////////////////////////////////////////////////////////////////////////////



    // Sorting Algorithms Ascending ///////////////////////////////////////////////////////////
    // Bubble Sort
    IEnumerator BubbleSort(List<GameObject> c)
    {
        // Unsorted array
        textMeshPro.text += "Unsorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        int comparisons = 0;
        int swaps = 0;

        bool swapped; // Track if any swaps occurred

        for (int i = 0; i < c.Count; i++)
        {
            swapped = false; // Reset the swapped flag for each pass

            for (int j = 0; j < c.Count - i - 1; j++)
            {
                comparisons++;

                yield return new WaitForSeconds(speed);

                LeanTween.color(c[j], Color.yellow, 0.1f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.color(c[j + 1], Color.blue, 0.1f).setEase(LeanTweenType.easeOutQuad);

                if (c[j].transform.localScale.y > c[j + 1].transform.localScale.y)
                {
                    swaps++;
                    swapped = true;

                    // Swap
                    GameObject temp = c[j];

                    yield return new WaitForSeconds(speed);

                    LeanTween.moveLocalX(c[j], c[j + 1].transform.localPosition.x, 0.3f).setEase(LeanTweenType.easeOutQuad);
                    LeanTween.moveLocalZ(c[j], -1.5f, 0.3f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad);
                    c[j] = c[j + 1];

                    LeanTween.moveLocalX(c[j + 1], temp.transform.localPosition.x, 0.3f).setEase(LeanTweenType.easeOutQuad);
                    LeanTween.moveLocalZ(c[j + 1], 1.5f, 0.3f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad);
                    c[j + 1] = temp;

                    yield return new WaitForSeconds(speed);
                }

                yield return new WaitForSeconds(speed);

                LeanTween.color(c[j], Color.white, 0.1f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.color(c[j + 1], Color.white, 0.1f).setEase(LeanTweenType.easeOutQuad);
            }

            // If no swaps occurred in the inner loop, the array is already sorted
            if (!swapped)
            {
                break;
            }
        }

        // Sorted array
        textMeshPro.text += "Sorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        // Time complexity analysis
        int n = c.Count;

        // Best Case: O(n) (when the array is already sorted)
        if (swaps == 0)
        {
            textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
            textMeshPro.text += "Total Swaps: " + swaps + "\n";
            textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
            textMeshPro.text += "Case Complexity: Best case" + "\n";
            textMeshPro.text += "Time Complexity: O(n)" + "\n";
        }
        // Worst Case: O(n^2) (when the array is in reverse order)
        else if (swaps == (n * (n - 1)) / 2)
        {
            textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
            textMeshPro.text += "Total Swaps: " + swaps + "\n";
            textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
            textMeshPro.text += "Case Complexity: Worst case" + "\n";
            textMeshPro.text += "Time Complexity: O(n^2)" + "\n";
        }
        // Average Case: O(n^2)
        else
        {
            textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
            textMeshPro.text += "Total Swaps: " + swaps + "\n";
            textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
            textMeshPro.text += "Case Complexity: Average case" + "\n";
            textMeshPro.text += "Time Complexity: O(n^2)" + "\n";
        }

        if (IsSorted(c))
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Complete());
        }
    }

    // Quick Sort
    int comparisons = 0;
    int swaps = 0;

    IEnumerator QuickSort(List<GameObject> c, int left, int right)
    {
        if (left < right)
        {
            // Partition Begin !!!
            int pivot = (int)c[right].transform.localScale.y;
            LeanTween.color(c[right], Color.red, 0.2f);

            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j], Color.blue, 0.2f);

                comparisons++; // Increase comparison count

                if (c[j].transform.localScale.y < pivot)
                {
                    yield return new WaitForSeconds(speed);
                    i++;
                    LeanTween.color(c[i], Color.yellow, 0.2f);

                    yield return new WaitForSeconds(speed * 1.5f);
                    // Swap
                    GameObject temp = c[i];
                    Vector3 tempPosition = c[i].transform.localPosition;

                    if (c[i] != c[j])
                    {
                        swaps++;
                    }

                    LeanTween.moveLocalX(c[i], c[j].transform.localPosition.x, speed).setEase(LeanTweenType.easeOutCubic);
                    LeanTween.moveZ(c[i], -1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
                    c[i] = c[j];

                    LeanTween.moveLocalX(c[j], tempPosition.x, speed).setEase(LeanTweenType.easeOutCubic);
                    LeanTween.moveZ(c[j], 1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
                    c[j] = temp;

                    yield return new WaitForSeconds(speed * 1.5f);
                    LeanTween.color(c[i], Color.white, 0.2f);
                }
                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j], Color.white, 0.2f);
            }

            yield return new WaitForSeconds(speed * 1.5f);
            // Swap Again
            GameObject tempSwap = c[i + 1];
            Vector3 tP = c[i + 1].transform.localPosition;

            if (c[i + 1] != c[right])
            {
                swaps++;
            }

            LeanTween.moveLocalX(c[i + 1], c[right].transform.localPosition.x, speed).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveZ(c[i + 1], -1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
            c[i + 1] = c[right];

            LeanTween.moveLocalX(c[right], tP.x, speed).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveZ(c[right], 1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
            c[right] = tempSwap;

            LeanTween.color(c[i + 1], Color.white, 0.2f);
            yield return new WaitForSeconds(speed * 1.5f);

            // Partition End !!!

            int p = i + 1;
            yield return new WaitForSeconds(speed * 1.5f);
            StartCoroutine(QuickSort(c, p + 1, right));
            yield return new WaitForSeconds(speed * 1.5f);
            StartCoroutine(QuickSort(c, left, p - 1));
            
            if (IsSorted(cubes))
            {
                yield return new WaitForSeconds(speed);
                StartCoroutine(Complete());
                StartCoroutine(Output());
            }
        }
    }

    IEnumerator OutputFirst()
    {
        textMeshPro.text += "Unsorted array: " + string.Join(", ", cubes.Select(cube => cube.transform.localScale.y.ToString())) + "\n";
        yield return null;
    }

    IEnumerator Output()
    {
        // Output the sorted array
        string sortedArray = "";
        foreach (GameObject obj in cubes)
        {
            sortedArray += obj.transform.localScale.y + ", ";
        }
        sortedArray = sortedArray.TrimEnd(',', ' ');

        // Output the computed values
        textMeshPro.text += "Sorted Array: " + sortedArray + "\n";
        textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
        textMeshPro.text += "Total Swaps: " + swaps + "\n";
        textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
        
        // Calculate and output the Case Complexity and time complexity
        if (swaps == 0) // Best Case
        {
            textMeshPro.text += "Case Complexity: Best Case" + "\n";
            textMeshPro.text += "Time Complexity: O(n*logn)" + "\n";
        }
        else if (swaps == (cubes.Count - 1)) // Worst Case
        {
            textMeshPro.text += "Case Complexity: Worst Case" + "\n";
            textMeshPro.text += "Time Complexity: O(n^2)" + "\n";
        }
        else // Average Case
        {
            textMeshPro.text += "Case Complexity: Average Case" + "\n";
            textMeshPro.text += "Time Complexity: O(n*logn)" + "\n";
        }
        swaps = 0;
        comparisons = 0;
        yield return null;
    }

    // Selection Sort
    IEnumerator SelectionSort(List<GameObject> c)
    {
        textMeshPro.text += "Unsorted array: " + string.Join(", ", cubes.Select(cube => cube.transform.localScale.y.ToString())) + "\n";
        int comparisons = 0;
        int swaps = 0;

        for (int i = 0; i < c.Count - 1; i++)
        {
            int min_index = i;
            LeanTween.color(c[i], Color.blue, 0.2f).setEase(LeanTweenType.easeOutQuad);

            for (int j = i + 1; j < c.Count; j++)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j], Color.yellow, 0.2f).setEase(LeanTweenType.easeOutQuad);
                comparisons++;

                yield return new WaitForSeconds(speed);

                int min_scale_Y = (int)c[min_index].transform.localScale.y;
                int second_scale_Y = (int)c[j].transform.localScale.y;

                if (second_scale_Y < min_scale_Y)
                {
                    if (min_index != i)
                    {
                        LeanTween.color(c[min_index], Color.white, 0.2f).setEase(LeanTweenType.easeOutQuad);
                    }
                    min_index = j;
                    LeanTween.color(c[min_index], Color.blue, 0.2f).setEase(LeanTweenType.easeOutQuad);
                }
                else
                {
                    LeanTween.color(c[j], Color.white, 0.2f).setEase(LeanTweenType.easeOutQuad);
                }
            }

            yield return new WaitForSeconds(speed);

            if (min_index != i)
            {
                Swap(c, i, min_index);
                swaps++;
                yield return new WaitForSeconds(speed);
            }

            LeanTween.color(c[i], Color.cyan, 0.2f).setEase(LeanTweenType.easeOutQuad);
        }

        LeanTween.color(c[c.Count - 1], Color.cyan, 0.2f).setEase(LeanTweenType.easeOutQuad);

        if (IsSorted(c))
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(Complete());
        }

        // Output the results
        textMeshPro.text += "Sorted array: " + string.Join(", ", cubes.Select(cube => cube.transform.localScale.y.ToString())) + "\n" +
                        "Total Comparisons: " + comparisons + "\n" +
                        "Total Swaps: " + swaps + "\n" +
                        "T(n): " + (comparisons + swaps) + "\n";

        // Determine time complexity and Case Complexity
        if (swaps == 0)
        {
            textMeshPro.text += "Case Complexity: Best case\n" +
                            "Time Complexity: O(n^2)";
        }
        else if (comparisons + swaps == c.Count * (c.Count - 1) / 2)
        {
            textMeshPro.text += "Case Complexity: Worst case\n" +
                            "Time Complexity: O(n^2)";
        }
        else
        {
            textMeshPro.text += "Case Complexity: Average case\n" +
                            "Time Complexity: O(n^2)";
        }
    }

    // Insertion Sort
    IEnumerator InsertionSort(List<GameObject> c)
    {
        float moveDuration = speed * 1.2f; // Adjusted speed for smoother animations
        float originalCameraSize = secondCam.orthographicSize;
        float zoomDuration = speed * 0.8f; // Adjusted speed for smoother animations

        float maxCubeHeight = c.Max(cube => cube.transform.localScale.y);
        float maxCameraSize = maxCubeHeight / 2.0f + 1.0f;

        int comparisons = 0;
        int inserts = 0;
        bool lmao = true;

        textMeshPro.text += "Unsorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        for (int i = 1; i < c.Count; i++)
        {
            if (lmao == true)
            {
                LeanTween.moveLocalY(c[0], c[0].transform.localScale.y / 2.0f, moveDuration).setEase(LeanTweenType.easeOutQuad);
                lmao = false;
            }

            GameObject temp = c[i];
            LeanTween.color(temp, Color.red, 0.1f).setEase(LeanTweenType.easeOutQuad);

            bool nextCubeVisible = true;
            if (i + 1 < c.Count)
            {
                float nextCubeHeight = c[i + 1].transform.localScale.y;
                float nextCubeY = numberOfCubes + (nextCubeHeight / 2.0f);
                nextCubeVisible = nextCubeY < secondCam.transform.position.y;
            }

            float targetSize = nextCubeVisible ? originalCameraSize : maxCameraSize;
            if (targetSize > secondCam.orthographicSize)
            {
                Vector3 cameratargetPosition = secondCam.transform.position;
                cameratargetPosition.y = maxCubeHeight / 2.0f + 1.0f;
                LeanTween.move(secondCam.gameObject, cameratargetPosition, zoomDuration)
                    .setEase(LeanTweenType.easeOutQuad);

                LeanTween.value(secondCam.gameObject, secondCam.orthographicSize, targetSize, zoomDuration)
                    .setOnUpdate((float value) =>
                    {
                        secondCam.orthographicSize = value;
                    })
                    .setEase(LeanTweenType.easeOutQuad);
            }

            yield return new WaitForSeconds(speed);

            LeanTween.moveLocalY(temp, numberOfCubes + (c[i].transform.localScale.y / 2), moveDuration).setEase(LeanTweenType.easeOutQuad);

            int j = i - 1;

            float tempX = -100;

            while (j >= 0 && c[j].transform.localScale.y > temp.transform.localScale.y)
            {
                yield return new WaitForSeconds(speed);

                // Highlight predecessor cube
                LeanTween.color(c[j], Color.blue, 0.2f).setEase(LeanTweenType.easeOutQuad);

                LeanTween.moveLocalX(c[j], c[j].transform.localPosition.x + 1f, moveDuration).setEase(LeanTweenType.easeOutQuad);
                c[j + 1] = c[j];

                tempX = c[j].transform.localPosition.x;
                j--;

                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j+1], Color.white, 0.5f).setEase(LeanTweenType.easeOutQuad);

                comparisons++;
            }

            if (tempX >= 0)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.moveLocalX(temp, tempX, moveDuration).setEase(LeanTweenType.easeOutQuad);
            }

            yield return new WaitForSeconds(speed);
            LeanTween.moveLocalY(temp, temp.transform.localScale.y / 2.0f, moveDuration).setEase(LeanTweenType.easeOutQuad);
            LeanTween.color(temp, Color.white, 0.1f).setEase(LeanTweenType.easeOutQuad);

            c[j + 1] = temp;

            for (int k = 0; k <= i; k++)
            {
                yield return new WaitForSeconds(speed * 0.8f); // Adjusted speed for smoother animations
                LeanTween.moveLocalX(c[k], k, moveDuration).setEase(LeanTweenType.easeOutQuad);
            }

            // Reset predecessor cube color
            if (j >= 0)
            {
                yield return new WaitForSeconds(speed);
                
            }
        }

        yield return new WaitForSeconds(0.2f); // Delay before camera zoom-out

        // Center camera on the sorted array
        Vector3 cameraTargetPosition = new Vector3(((numberOfCubes - 1) / 2.0f) - 0.5f, (maxCubeHeight/2.0f) + 0.4f, -maxCubeHeight);
        LeanTween.move(secondCam.gameObject, cameraTargetPosition, zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(secondCam.gameObject, secondCam.orthographicSize, originalCameraSize, zoomDuration)
            .setOnUpdate((float value) =>
            {
                secondCam.orthographicSize = value;
            })
            .setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(zoomDuration);

        if (IsSorted(cubes))
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Complete());
        }

        // Additional output code
        for (int i = 1; i < c.Count; i++)
        {
            inserts++;
        }

        textMeshPro.text += "Sorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        textMeshPro.text += "Total comparisons: " + comparisons + "\n";
        textMeshPro.text += "Total inserts: " + inserts + "\n";
        textMeshPro.text += "T(n): " + (comparisons + inserts) + "\n";

        // Calculate and output the time complexity
        string timeComplexity;
        if (comparisons == 0)
        {
            timeComplexity = "Best case\nTime Complexity: O(n)";
        }
        else if (comparisons == c.Count * (c.Count - 1) / 2)
        {
            timeComplexity = "Worst case\nTime Complexity: O(n^2)";
        }
        else
        {
            timeComplexity = "Average case\nTime Complexity: O(n^2)";
        }

        textMeshPro.text += "Case Complexity: " + timeComplexity;
    }


    // Sorting Algorithms Descending ////////////////////////////////////////////////////////////////////

    // Bubble Sort
    IEnumerator DesBubbleSort(List<GameObject> c)
    {
        // Unsorted array
        textMeshPro.text += "Unsorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        int comparisons = 0;
        int swaps = 0;

        bool swapped; // Track if any swaps occurred

        for (int i = 0; i < c.Count; i++)
        {
            swapped = false; // Reset the swapped flag for each pass

            for (int j = 0; j < c.Count - i - 1; j++)
            {
                comparisons++;

                yield return new WaitForSeconds(speed);

                LeanTween.color(c[j], Color.yellow, 0.1f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.color(c[j + 1], Color.blue, 0.1f).setEase(LeanTweenType.easeOutQuad);

                if (c[j].transform.localScale.y < c[j + 1].transform.localScale.y)
                {
                    swaps++;
                    swapped = true;

                    // Swap
                    GameObject temp = c[j];

                    yield return new WaitForSeconds(speed);

                    LeanTween.moveLocalX(c[j], c[j + 1].transform.localPosition.x, 0.3f).setEase(LeanTweenType.easeOutQuad);
                    LeanTween.moveLocalZ(c[j], -1.5f, 0.3f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad);
                    c[j] = c[j + 1];

                    LeanTween.moveLocalX(c[j + 1], temp.transform.localPosition.x, 0.3f).setEase(LeanTweenType.easeOutQuad);
                    LeanTween.moveLocalZ(c[j + 1], 1.5f, 0.3f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad);
                    c[j + 1] = temp;

                    yield return new WaitForSeconds(speed);
                }

                yield return new WaitForSeconds(speed);

                LeanTween.color(c[j], Color.white, 0.1f).setEase(LeanTweenType.easeOutQuad);
                LeanTween.color(c[j + 1], Color.white, 0.1f).setEase(LeanTweenType.easeOutQuad);
            }

            // If no swaps occurred in the inner loop, the array is already sorted
            if (!swapped)
            {
                break;
            }
        }

        // Sorted array
        textMeshPro.text += "Sorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        // Time complexity analysis
        int n = c.Count;

        // Best Case: O(n) (when the array is already sorted)
        if (swaps == 0)
        {
            textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
            textMeshPro.text += "Total Swaps: " + swaps + "\n";
            textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
            textMeshPro.text += "Case Complexity: Best case" + "\n";
            textMeshPro.text += "Time Complexity: O(n)" + "\n";
        }
        // Worst Case: O(n^2) (when the array is in reverse order)
        else if (swaps == (n * (n - 1)) / 2)
        {
            textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
            textMeshPro.text += "Total Swaps: " + swaps + "\n";
            textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
            textMeshPro.text += "Case Complexity: Worst case" + "\n";
            textMeshPro.text += "Time Complexity: O(n^2)" + "\n";
        }
        // Average Case: O(n^2)
        else
        {
            textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
            textMeshPro.text += "Total Swaps: " + swaps + "\n";
            textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
            textMeshPro.text += "Case Complexity: Average case" + "\n";
            textMeshPro.text += "Time Complexity: O(n^2)" + "\n";
        }

        if (DesIsSorted(c))
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Complete());
        }
    }
    // Quick Sort
    IEnumerator DesQuickSort(List<GameObject> c, int left, int right)
    {
        if (left < right)
        {
            // Partition Begin !!!
            int pivot = (int)c[right].transform.localScale.y;
            LeanTween.color(c[right], Color.red, 0.2f);

            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j], Color.blue, 0.2f);

                comparisons++; // Increase comparison count

                if (c[j].transform.localScale.y > pivot) // Change comparison condition
                {
                    yield return new WaitForSeconds(speed);
                    i++;
                    LeanTween.color(c[i], Color.yellow, 0.2f);

                    yield return new WaitForSeconds(speed * 1.5f);
                    // Swap
                    GameObject temp = c[i];
                    Vector3 tempPosition = c[i].transform.localPosition;

                    if (c[i] != c[j])
                    {
                        swaps++;
                    }

                    LeanTween.moveLocalX(c[i], c[j].transform.localPosition.x, speed).setEase(LeanTweenType.easeOutCubic);
                    LeanTween.moveZ(c[i], -1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
                    c[i] = c[j];

                    LeanTween.moveLocalX(c[j], tempPosition.x, speed).setEase(LeanTweenType.easeOutCubic);
                    LeanTween.moveZ(c[j], 1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
                    c[j] = temp;

                    yield return new WaitForSeconds(speed * 1.5f);
                    LeanTween.color(c[i], Color.white, 0.2f);
                }
                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j], Color.white, 0.2f);
            }

            yield return new WaitForSeconds(speed * 1.5f);
            // Swap Again
            GameObject tempSwap = c[i + 1];
            Vector3 tP = c[i + 1].transform.localPosition;

            if (c[i + 1] != c[right])
            {
                swaps++;
            }

            LeanTween.moveLocalX(c[i + 1], c[right].transform.localPosition.x, speed).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveZ(c[i + 1], -1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
            c[i + 1] = c[right];

            LeanTween.moveLocalX(c[right], tP.x, speed).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveZ(c[right], 1.5f, speed).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
            c[right] = tempSwap;

            LeanTween.color(c[i + 1], Color.white, 0.2f);
            yield return new WaitForSeconds(speed * 1.5f);

            // Partition End !!!

            int p = i + 1;
            yield return new WaitForSeconds(speed * 1.5f);
            StartCoroutine(DesQuickSort(c, p + 1, right));
            yield return new WaitForSeconds(speed * 1.5f);
            StartCoroutine(DesQuickSort(c, left, p - 1));

            if (DesIsSorted(cubes))
            {
                yield return new WaitForSeconds(speed);
                StartCoroutine(Complete());
                StartCoroutine(Output());
            }
        }
    }

    IEnumerator DesOutputFirst()
    {
        textMeshPro.text += "Unsorted array: " + string.Join(", ", cubes.Select(cube => cube.transform.localScale.y.ToString())) + "\n";
        yield return null;
    }

    IEnumerator DesOutput()
    {
        // Output the sorted array in descending order
        string sortedArray = "";
        foreach (GameObject obj in cubes)
        {
            sortedArray += obj.transform.localScale.y + ", ";
        }
        sortedArray = sortedArray.TrimEnd(',', ' ');

        // Output the computed values
        textMeshPro.text += "Sorted Array: " + sortedArray + "\n";
        textMeshPro.text += "Total Comparisons: " + comparisons + "\n";
        textMeshPro.text += "Total Swaps: " + swaps + "\n";
        textMeshPro.text += "T(n): " + (comparisons + swaps) + "\n";
        
        // Calculate and output the Case Complexity and time complexity
        if (swaps == 0) // Best Case
        {
            textMeshPro.text += "Case Complexity: Best Case" + "\n";
            textMeshPro.text += "Time Complexity: O(n*logn)" + "\n";
        }
        else if (swaps == (cubes.Count - 1)) // Worst Case
        {
            textMeshPro.text += "Case Complexity: Worst Case" + "\n";
            textMeshPro.text += "Time Complexity: O(n^2)" + "\n";
        }
        else // Average Case
        {
            textMeshPro.text += "Case Complexity: Average Case" + "\n";
            textMeshPro.text += "Time Complexity: O(n*logn)" + "\n";
        }
        swaps = 0;
        comparisons = 0;
        yield return null;
    }


    // Selection Sort
    IEnumerator DesSelectionSort(List<GameObject> c)
    {
        textMeshPro.text += "Unsorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";
        int comparisons = 0;
        int swaps = 0;

        for (int i = 0; i < c.Count - 1; i++)
        {
            int max_index = i;
            LeanTween.color(c[i], Color.blue, 0.2f).setEase(LeanTweenType.easeOutQuad);

            for (int j = i + 1; j < c.Count; j++)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j], Color.yellow, 0.2f).setEase(LeanTweenType.easeOutQuad);
                comparisons++;

                yield return new WaitForSeconds(speed);

                int max_scale_Y = (int)c[max_index].transform.localScale.y;
                int second_scale_Y = (int)c[j].transform.localScale.y;

                if (second_scale_Y > max_scale_Y) // Updated condition for descending order
                {
                    if (max_index != i)
                    {
                        LeanTween.color(c[max_index], Color.white, 0.2f).setEase(LeanTweenType.easeOutQuad);
                    }
                    max_index = j;
                    LeanTween.color(c[max_index], Color.blue, 0.2f).setEase(LeanTweenType.easeOutQuad);
                }
                else
                {
                    LeanTween.color(c[j], Color.white, 0.2f).setEase(LeanTweenType.easeOutQuad);
                }
            }

            yield return new WaitForSeconds(speed);

            if (max_index != i)
            {
                Swap(c, i, max_index);
                swaps++;
                yield return new WaitForSeconds(speed);
            }

            LeanTween.color(c[i], Color.cyan, 0.2f).setEase(LeanTweenType.easeOutQuad);
        }

        LeanTween.color(c[c.Count - 1], Color.cyan, 0.2f).setEase(LeanTweenType.easeOutQuad);

        if (DesIsSorted(c))
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(Complete());
        }

        // Output the results
        textMeshPro.text += "Sorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n" +
                            "Total Comparisons: " + comparisons + "\n" +
                            "Total Swaps: " + swaps + "\n" +
                            "T(n): " + (comparisons + swaps) + "\n";

        // Determine time complexity and Case Complexity
        if (swaps == 0)
        {
            textMeshPro.text += "Case Complexity: Best case\n" +
                                "Time Complexity: O(n^2)";
        }
        else if (comparisons + swaps == c.Count * (c.Count - 1) / 2)
        {
            textMeshPro.text += "Case Complexity: Worst case\n" +
                                "Time Complexity: O(n^2)";
        }
        else
        {
            textMeshPro.text += "Case Complexity: Average case\n" +
                                "Time Complexity: O(n^2)";
        }
    }

    // Insertion Sort
    IEnumerator DesInsertionSort(List<GameObject> c)
    {
        float moveDuration = speed * 1.2f; // Adjusted speed for smoother animations
        float originalCameraSize = secondCam.orthographicSize;
        float zoomDuration = speed * 0.8f; // Adjusted speed for smoother animations

        float maxCubeHeight = c.Max(cube => cube.transform.localScale.y);
        float maxCameraSize = maxCubeHeight / 2.0f + 1.0f;

        int comparisons = 0;
        int inserts = 0;
        bool lmao = true;

        textMeshPro.text += "Unsorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        for (int i = 1; i < c.Count; i++)
        {
            if (lmao == true)
            {
                LeanTween.moveLocalY(c[0], c[0].transform.localScale.y / 2.0f, moveDuration).setEase(LeanTweenType.easeOutQuad);
                lmao = false;
            }

            GameObject temp = c[i];
            LeanTween.color(temp, Color.red, 0.1f).setEase(LeanTweenType.easeOutQuad);

            bool nextCubeVisible = true;
            if (i + 1 < c.Count)
            {
                float nextCubeHeight = c[i + 1].transform.localScale.y;
                float nextCubeY = numberOfCubes + (nextCubeHeight / 2.0f);
                nextCubeVisible = nextCubeY < secondCam.transform.position.y;
            }

            float targetSize = nextCubeVisible ? originalCameraSize : maxCameraSize;
            if (targetSize > secondCam.orthographicSize)
            {
                Vector3 cameratargetPosition = secondCam.transform.position;
                cameratargetPosition.y = maxCubeHeight / 2.0f + 1.0f;
                LeanTween.move(secondCam.gameObject, cameratargetPosition, zoomDuration)
                    .setEase(LeanTweenType.easeOutQuad);

                LeanTween.value(secondCam.gameObject, secondCam.orthographicSize, targetSize, zoomDuration)
                    .setOnUpdate((float value) =>
                    {
                        secondCam.orthographicSize = value;
                    })
                    .setEase(LeanTweenType.easeOutQuad);
            }

            yield return new WaitForSeconds(speed);

            LeanTween.moveLocalY(temp, numberOfCubes + (c[i].transform.localScale.y / 2), moveDuration).setEase(LeanTweenType.easeOutQuad);

            int j = i - 1;

            float tempX = -100;

            while (j >= 0 && c[j].transform.localScale.y < temp.transform.localScale.y) // Modified comparison
            {
                yield return new WaitForSeconds(speed);

                // Highlight predecessor cube
                LeanTween.color(c[j], Color.blue, 0.2f).setEase(LeanTweenType.easeOutQuad);

                LeanTween.moveLocalX(c[j], c[j].transform.localPosition.x + 1f, moveDuration).setEase(LeanTweenType.easeOutQuad);
                c[j + 1] = c[j];

                tempX = c[j].transform.localPosition.x;
                j--;

                yield return new WaitForSeconds(speed);
                LeanTween.color(c[j + 1], Color.white, 0.5f).setEase(LeanTweenType.easeOutQuad);

                comparisons++;
            }

            if (tempX >= 0)
            {
                yield return new WaitForSeconds(speed);
                LeanTween.moveLocalX(temp, tempX, moveDuration).setEase(LeanTweenType.easeOutQuad);
            }

            yield return new WaitForSeconds(speed);
            LeanTween.moveLocalY(temp, temp.transform.localScale.y / 2.0f, moveDuration).setEase(LeanTweenType.easeOutQuad);
            LeanTween.color(temp, Color.white, 0.1f).setEase(LeanTweenType.easeOutQuad);

            c[j + 1] = temp;

            for (int k = 0; k <= i; k++)
            {
                yield return new WaitForSeconds(speed * 0.8f); // Adjusted speed for smoother animations
                LeanTween.moveLocalX(c[k], k, moveDuration).setEase(LeanTweenType.easeOutQuad);
            }

            // Reset predecessor cube color
            if (j >= 0)
            {
                yield return new WaitForSeconds(speed);

            }
        }

        yield return new WaitForSeconds(0.2f); // Delay before camera zoom-out

        // Center camera on the sorted array
        Vector3 cameraTargetPosition = new Vector3(((numberOfCubes - 1) / 2.0f) - 0.5f, (maxCubeHeight/2.0f) + 0.4f, -maxCubeHeight);
        LeanTween.move(secondCam.gameObject, cameraTargetPosition, zoomDuration)
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(secondCam.gameObject, secondCam.orthographicSize, originalCameraSize, zoomDuration)
            .setOnUpdate((float value) =>
            {
                secondCam.orthographicSize = value;
            })
            .setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(zoomDuration);

        if (DesIsSorted(cubes))
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Complete());
        }

        // Additional output code
        for (int i = 1; i < c.Count; i++)
        {
            inserts++;
        }

        textMeshPro.text += "Sorted array: " + string.Join(", ", c.Select(cube => cube.transform.localScale.y.ToString())) + "\n";

        textMeshPro.text += "Total comparisons: " + comparisons + "\n";
        textMeshPro.text += "Total inserts: " + inserts + "\n";
        textMeshPro.text += "T(n): " + (comparisons + inserts) + "\n";

        // Calculate and output the time complexity
        string timeComplexity;
        if (comparisons == 0)
        {
            timeComplexity = "Best case\nTime Complexity: O(n)";
        }
        else if (comparisons == c.Count * (c.Count - 1) / 2)
        {
            timeComplexity = "Worst case\nTime Complexity: O(n^2)";
        }
        else
        {
            timeComplexity = "Average case\nTime Complexity: O(n^2)";
        }

        textMeshPro.text += "Case Complexity: " + timeComplexity;
    }

////////////////////////////////////////////////////////////////////////Code output and Highlights///////////////////////////////////////////////////////////////////////////

}