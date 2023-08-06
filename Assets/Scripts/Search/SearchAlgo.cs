using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SearchAlgo : MonoBehaviour
{
    bool isFinding = false;
    bool starting = true;
    bool clickedStartFind = false;
    bool randomized = true;

    public TMP_Dropdown dropDownAlgo;
    private int[] numInput;
    public TextMeshProUGUI textMeshPro;
    public TMP_InputField inputField;
    int algoSelected = 0;
    public int arraySize = 25;
    public GameObject cubePrefab;
    public float spacing = 0.1f;
    public Camera secondCam;
    public int numberToFind;
    public float animationSpeed = 1.0f;
    public float scaleAmount = 1.5f;
    public float scaleDuration = 0.5f;
    public List<int> usedNumbers;
    public List<int> tempNum;
    public List<int> binaryNum;
    List<GameObject> cubeList = new List<GameObject>();
    List<GameObject> tempcubelist = new List<GameObject>();
    public TMP_InputField findNum;
    string inputText;
    [SerializeField] public Slider slider;
    [SerializeField] private Slider _slider;
    string _string;
    

    void Start()
    {
        findNum.text = "0";
        _slider.onValueChanged.AddListener((v) => {
            _string = v.ToString();
            arraySize = int.Parse(_string);
            randomized = false;
            CreateCubeArray(arraySize);
            randomized = true;
        });

        findNum.onSelect.AddListener(OnInputFieldClick);
        inputField.onEndEdit.AddListener(InputFieldEndEdit);
        inputField.onSubmit.AddListener(SubmitInput);
        usedNumbers = new List<int>();
        tempNum = new List<int>();
        binaryNum = new List<int>();
        CreateCubeArray(arraySize);
        //findNum.onSubmit.AddListener(OnSubmitInput);
        findNum.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnInputFieldClick(string value)
    {
        clickedStartFind = true;
        CreateCubeArray(arraySize);
        clickedStartFind = false;
    }

    void Update()
    {
        SliderVal();
    }

    public void SliderVal()
    {
        Time.timeScale=(int)(slider.value);
    }

    // private void OnSubmitInput(string input)
    // {
    //     if (string.IsNullOrEmpty(input))
    //     {
    //         return; 
    //     }
    //     else if (int.Parse(input) >= 0)
    //     {
    //         inputText  = findNum.text;
    //         numberToFind = int.Parse(input);
    //     }
    //     else
    //     {
    //         inputField.text = "";
    //         textMeshPro.text = ("Invalid input: " + int.Parse(input) + ". Please input positive integers only.");
    //         return;
    //     }

    //     startFind();
    // }

    private void OnInputFieldEndEdit(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            else if (int.Parse(input) >= 0)
            {
                inputText  = findNum.text;
                numberToFind = int.Parse(input);
            }
            else
            {
                textMeshPro.text = ("Invalid input: " + input + ". Please input positive integers only.");
                findNum.text = "0";
                return;
            }
        }
        else
        {
            textMeshPro.text = ("Invalid input: " + input + ". Please input positive integers only.");
            findNum.text = "0";
            return;
        }
    }

    public void randomizeArray()
    {
        if(!isFinding)
        {
            CreateCubeArray(arraySize);
            randomized = true;
        }
    }

    public void startFind()
    {
        if(isFinding == false)
        {
            textMeshPro.text = "";
            clickedStartFind = true;
            CreateCubeArray(arraySize);
            disableUI();

            if(algoSelected == 0)
            {
                isFinding = true;
                StartCoroutine(PerformLinearSearch());
            }
            else if(algoSelected == 1)
            {
                isFinding = true;
                StartCoroutine(PerformBinarySearch());
            }
            clickedStartFind = false;
        }
    }

    public void DropDownAlgo(int index)
    {
        switch (index)
        {
            case 0:
                clickedStartFind = true;
                textMeshPro.text = "";
                CreateCubeArray(arraySize);
                clickedStartFind = false;
                algoSelected = 0;
                break;
                
            case 1:
                clickedStartFind = true;
                textMeshPro.text = "";
                CreateCubeArray(arraySize);
                clickedStartFind = false;
                algoSelected = 1;
                break;
        }
    }

    public void stopVisualizing()
    {
        if(isFinding)
        {
            clickedStartFind = true;
            textMeshPro.text = "";
            StopAllCoroutines();
            CreateCubeArray(arraySize);
            enableUI();
            isFinding = false;
            clickedStartFind = false;
        }
    }
    
    public void enableUI()
    {
        findNum.enabled = true;
        dropDownAlgo.enabled = true;
        inputField.enabled = true;
        _slider.enabled = true;
    }

    public void disableUI()
    {
        findNum.enabled = false;
        dropDownAlgo.enabled = false;
        inputField.enabled = false;
        _slider.enabled = false;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    private void InputFieldEndEdit(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return; 
        }

        string[] inputArray = input.Split(new char[] { ' ', ',', '"', '.', '/', '?', ';' }, System.StringSplitOptions.RemoveEmptyEntries);

        SubmitInput(input);
    }

    private void SubmitInput(string input)
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
                textMeshPro.text = ("Invalid input: " + inputArray[i] + ". Please input positive integers only.");
                return;
            }
        }

        if (numInput.Length < 5)
        {
            inputField.text = "";
            textMeshPro.text = "Input array size must be 5 or more.";
            return;
        }

        arraySize = numInput.Length;

        InputCreateCube();
        inputField.text = "";
    }

    // ----------------------------------------------------------------------------------------------------------------------------------------------------- //

    void InputCreateCube()
    {
        // Calculate the number of columns based on the square root of the array size
        int numColumns = Mathf.CeilToInt(Mathf.Sqrt(arraySize));

        // Calculate the width and height of each cube based on the number of columns and spacing
        float totalSpacing = (numColumns - 1) * spacing;
        float cubeWidth = (5f - totalSpacing) / numColumns;
        float cubeHeight = (5f - totalSpacing) / numColumns;

        float maxFontSize = Mathf.Min(cubeWidth, cubeHeight) * 0.5f;
        float fontSize = Mathf.Lerp(5f, 2f, Mathf.InverseLerp(10, 100, arraySize));

        // Get the URP asset
        UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        // Get the URP default shader
        Shader defaultShader = urpAsset.defaultShader;

        if(cubeList != null)
        {
            for (int i = 0; i < cubeList.Count; i++)
                Destroy(cubeList[i].gameObject);
            cubeList.Clear();
        }

        if (tempcubelist != null)
        {
            for (int i = 0; i < tempcubelist.Count; i++)
            {
                Destroy(tempcubelist[i].gameObject);
            }
            tempcubelist.Clear();
            tempNum.Clear();
        }

        for (int i = 0; i < arraySize; i++)
        {
            
            tempNum.Add(numInput[i]);

            // Calculate the row and column indices
            int row = i / numColumns;
            int column = i % numColumns;

            // Instantiate a new cube using the prefab
            GameObject cube = Instantiate(cubePrefab, transform);

            // Add a BoxCollider component to the cube
            BoxCollider collider = cube.AddComponent<BoxCollider>();

            // Calculate the position of the cube based on the row and column indices
            float xPos = column * (cubeWidth + spacing) + cubeWidth / 2f;
            float yPos = (numColumns - 1 - row) * (cubeHeight + spacing) + cubeHeight / 2f;

            // Set the position of the cube
            cube.transform.position = new Vector3(xPos, yPos, 0f);

            // Set the scale of the cube
            cube.transform.localScale = new Vector3(cubeWidth, cubeHeight, cubeWidth);

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
            textMesh.text = numInput[i].ToString();
            //textMesh.characterSize = 0.04f;
            textMesh.fontStyle = FontStyles.Bold;
            textMesh.fontSize = fontSize;
            textMesh.color = Color.black;

            //textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.transform.position = new Vector3(cube.transform.position.x,  cube.transform.position.y, -cube.transform.position.z-1f);

            cubeList.Add(cube);
            tempcubelist.Add(cube);
        }

        float totalWidth = numColumns * (cubeWidth + spacing) - spacing;
        float totalHeight = numColumns * (cubeHeight + spacing) - spacing;
        float centerX = totalWidth / 2f;
        float centerY = totalHeight / 2f;
        secondCam.transform.position = new Vector3(centerX, centerY, secondCam.transform.position.z);
        float camSize = Mathf.Max(totalWidth, totalHeight) / 2f;
        secondCam.orthographicSize = camSize;
    }

    void CreateCubeArray(int arraySize)
    {
        if (cubeList != null)
        {
            for (int i = 0; i < cubeList.Count; i++)
                Destroy(cubeList[i].gameObject);
            cubeList.Clear();
        }

        usedNumbers.Clear();

        // Calculate the number of columns based on the square root of the array size
        int numColumns = Mathf.CeilToInt(Mathf.Sqrt(arraySize));

        // Calculate the width and height of each cube based on the number of columns and spacing
        float totalSpacing = (numColumns - 1) * spacing;
        float cubeWidth = (5f - totalSpacing) / numColumns;
        float cubeHeight = (5f - totalSpacing) / numColumns;

        float maxFontSize = Mathf.Min(cubeWidth, cubeHeight) * 0.5f;
        float fontSize = Mathf.Lerp(5f, 2f, Mathf.InverseLerp(10, 100, arraySize));

        // Get the URP asset
        UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        // Get the URP default shader
        Shader defaultShader = urpAsset.defaultShader;
    
        if(randomized == true && starting == false)
        {
            for (int i = 0; i < arraySize; i++)
            {
                // Calculate the row and column indices
                int row = i / numColumns;
                int column = i % numColumns;

                // Instantiate a new cube using the prefab
                GameObject cube = Instantiate(cubePrefab, transform);

                // Add a BoxCollider component to the cube
                BoxCollider collider = cube.AddComponent<BoxCollider>();

                // Calculate the position of the cube based on the row and column indices
                float xPos = column * (cubeWidth + spacing) + cubeWidth / 2f;
                float yPos = (numColumns - 1 - row) * (cubeHeight + spacing) + cubeHeight / 2f;

                // Set the position of the cube
                cube.transform.position = new Vector3(xPos, yPos, 0f);

                // Set the scale of the cube
                cube.transform.localScale = new Vector3(cubeWidth, cubeHeight, cubeWidth);

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
                textMesh.text = tempNum[i].ToString();
                //textMesh.characterSize = 0.04f;
                textMesh.fontStyle = FontStyles.Bold;
                textMesh.fontSize = fontSize;
                textMesh.color = Color.black;

                //textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.transform.position = new Vector3(cube.transform.position.x,  cube.transform.position.y, -cube.transform.position.z-1f);

                cubeList.Add(cube);
                tempcubelist.Add(cube);
            }
        }


        if(!clickedStartFind)
        {
            if (tempcubelist != null)
            {
                for (int i = 0; i < tempcubelist.Count; i++)
                {
                    Destroy(tempcubelist[i].gameObject);
                }
                tempcubelist.Clear();
                tempNum.Clear();
            }


            // Loop through the array and create cubes
            for (int i = 0; i < arraySize; i++)
            {
                // Calculate the row and column indices
                int row = i / numColumns;
                int column = i % numColumns;

                // Instantiate a new cube using the prefab
                GameObject cube = Instantiate(cubePrefab, transform);

                // Add a BoxCollider component to the cube
                BoxCollider collider = cube.AddComponent<BoxCollider>();

                // Calculate the position of the cube based on the row and column indices
                float xPos = column * (cubeWidth + spacing) + cubeWidth / 2f;
                float yPos = (numColumns - 1 - row) * (cubeHeight + spacing) + cubeHeight / 2f;

                // Set the position of the cube
                cube.transform.position = new Vector3(xPos, yPos, 0f);

                // Set the scale of the cube
                cube.transform.localScale = new Vector3(cubeWidth, cubeHeight, cubeWidth);

                int randomNumber;
                do
                {
                    randomNumber = Random.Range(0, 101);
                }
                while (usedNumbers.Contains(randomNumber));

                usedNumbers.Add(randomNumber);
                tempNum.Add(randomNumber);

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
                textMesh.text = usedNumbers[i].ToString();
                //textMesh.characterSize = 0.04f;
                textMesh.fontStyle = FontStyles.Bold;
                textMesh.fontSize = fontSize;
                textMesh.color = Color.black;

                //textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.transform.position = new Vector3(cube.transform.position.x,  cube.transform.position.y, -cube.transform.position.z-1f);
                cubeList.Add(cube);
                tempcubelist.Add(cube);
            }
            starting = false;
        }
        

        float totalWidth = numColumns * (cubeWidth + spacing) - spacing;
        float totalHeight = numColumns * (cubeHeight + spacing) - spacing;
        float centerX = totalWidth / 2f;
        float centerY = totalHeight / 2f;
        secondCam.transform.position = new Vector3(centerX, centerY, secondCam.transform.position.z);
        float camSize = Mathf.Max(totalWidth, totalHeight) / 2f;
        secondCam.orthographicSize = camSize;
    }

    //----------------------------------------------------------------------------------------------------------------------------------------------//

    //Linear Search//
    private IEnumerator PerformLinearSearch()
    {
        int i = 0;
        int iterations = 0;
        bool numberFound = false;

        foreach (GameObject cube in cubeList)
        {
            Material cubeMaterial = cube.GetComponent<Renderer>().material;
            Color originalColor = cubeMaterial.color;

            LTDescr colorTween = LeanTween.value(cube, originalColor, Color.yellow, animationSpeed);
            colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
            yield return new WaitForSeconds(animationSpeed);

            iterations++;

            if (tempNum[i] == numberToFind)
            {
                LTDescr scaleTween = LeanTween.scale(cube, Vector3.one * scaleAmount, scaleDuration);
                scaleTween.setEasePunch();
                yield return new WaitForSeconds(scaleDuration);

                colorTween = LeanTween.value(cube, Color.yellow, Color.green, animationSpeed);
                colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
                textMeshPro.text = numberToFind + " found.";
                enableUI();
                isFinding = false;
                numberFound = true;
                break;
            }
            else
            {
                LTDescr scaleTween = LeanTween.scale(cube, Vector3.one * scaleAmount, scaleDuration);
                scaleTween.setEasePunch();
                yield return new WaitForSeconds(scaleDuration);

                colorTween = LeanTween.value(cube, Color.yellow, Color.red, animationSpeed);
                colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
                yield return new WaitForSeconds(animationSpeed);

                colorTween = LeanTween.value(cube, Color.red, Color.white, animationSpeed);
                colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
            }
            i++;
        }

        if (!numberFound)
        {
            enableUI();
            isFinding = false;
            textMeshPro.text = numberToFind + " not found.";
        }

        if (iterations == 0)
        {
            textMeshPro.text += "\nT(n): " + iterations;
            textMeshPro.text += "\nTime Complexity: O(1)";
            textMeshPro.text += "\nCase Complexity: Best Case";
        }
        else if (iterations >= cubeList.Count)
        {
            textMeshPro.text += "\nT(n): " + iterations;
            textMeshPro.text += "\nTime Complexity: O(n)";
            textMeshPro.text += "\nCase Complexity: Worst Case";
            
        }
        else
        {
            textMeshPro.text += "\nT(n): " + iterations;
            textMeshPro.text += "\nTime Complexity: O(n)";
            textMeshPro.text += "\nCase Complexity: Average Case";
        }
    }



//--------------------------------------------------------------------------------------------------------------------------------------------------------------------//

    //Binary Search//
    private IEnumerator PerformBinarySearch()
    {
        if(binaryNum != null)
        {
            binaryNum.Clear();
        }

        for(int i = 0; i<tempNum.Count; i++)
        {
            binaryNum.Add(tempNum[i]);
        }
        // Sort the usedNumbers list in ascending order
        binaryNum.Sort();
        SortCubesByNumber();

        yield return new WaitForSeconds(2);

        int left = 0;
        int right = binaryNum.Count - 1;
        GameObject highlightedCube = null; // Store the cube to highlight

        int previousMid = -1; // Store the index of the previous middle element
        int iterations = 0;
        bool numberFound = false;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            GameObject cube = cubeList[mid];
            Material cubeMaterial = cube.GetComponent<Renderer>().material;
            Color originalColor = cubeMaterial.color;

            LTDescr colorTween = LeanTween.value(cube, originalColor, Color.yellow, animationSpeed);
            colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
            yield return new WaitForSeconds(animationSpeed);

            if (highlightedCube != null)
            {
                // Reset the color of the previously highlighted cube
                Material highlightedCubeMaterial = highlightedCube.GetComponent<Renderer>().material;
                LTDescr resetColorTween = LeanTween.value(highlightedCube, Color.blue, Color.yellow, animationSpeed);
                resetColorTween.setOnUpdateColor((Color color) => highlightedCubeMaterial.color = color);
            }

            if (binaryNum[mid] == numberToFind)
            {
                LTDescr sscaleTween = LeanTween.scale(cube, Vector3.one * scaleAmount, scaleDuration);
                sscaleTween.setEasePunch();
                yield return new WaitForSeconds(scaleDuration);

                colorTween = LeanTween.value(cube, Color.yellow, Color.green, animationSpeed);
                colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
                textMeshPro.text = numberToFind + " found.";

                // Destroy the previous middle element if it exists
                if (previousMid >= 0 && previousMid != mid)
                {
                    GameObject previousCube = cubeList[previousMid];
                    Destroy(previousCube);
                }

                // Destroy remaining cubes from left to right (excluding the cube with the found number)
                for (int i = left; i <= right; i++)
                {
                    if (i != mid) // Exclude the cube with the found number
                    {
                        GameObject cubeToDestroy = cubeList[i];
                        Destroy(cubeToDestroy);
                    }
                }

                enableUI();
                isFinding = false;
                numberFound = true;
                break;
            }
            else if (binaryNum[mid] < numberToFind)
            {
                // Destroy cubes from left to mid (exclusive)
                for (int i = left; i < mid; i++)
                {
                    GameObject cubeToDestroy = cubeList[i];
                    Destroy(cubeToDestroy);
                }
                left = mid + 1;
            }
            else
            {
                // Destroy cubes from mid to right (inclusive)
                for (int i = mid + 1; i <= right; i++)
                {
                    GameObject cubeToDestroy = cubeList[i];
                    Destroy(cubeToDestroy);
                }
                right = mid - 1;
            }

            if (binaryNum[mid] != numberToFind)
            {
                LTDescr scaleTween = LeanTween.scale(cube, Vector3.one * scaleAmount, scaleDuration);
                scaleTween.setEasePunch();
                yield return new WaitForSeconds(scaleDuration);

                colorTween = LeanTween.value(cube, Color.yellow, Color.red, animationSpeed);
                colorTween.setOnUpdateColor((Color color) => cubeMaterial.color = color);
                yield return new WaitForSeconds(animationSpeed);
            }

            // Update the previous middle element index
            previousMid = mid - 1;
            iterations++;
        }

        // Destroy the previous middle element if it exists
        if (previousMid >= 0)
        {
            GameObject previousCube = cubeList[previousMid];
            Destroy(previousCube);
        }

        isFinding = false;
        enableUI();

        if (!numberFound)
        {
            textMeshPro.text = numberToFind + " not found.";
        }

        int n = cubeList.Count;
        int bestCaseIterations = 1; // Element found at the first position
        int averageCaseIterations = (int)Mathf.Log(n, 2); // Average number of iterations for a successful search (base 2 logarithm)
        int worstCaseIterations = (int)Mathf.Log(n, 2); // Element not found, traverse the entire array (base 2 logarithm)

        int iterationsDiff = Mathf.Abs(iterations - bestCaseIterations);
        if (iterationsDiff == 0)
        {
            textMeshPro.text += "\nT(n): " + iterations;
            textMeshPro.text += "\nTime Complexity: O(1)";
            textMeshPro.text += "\nCase Complexity: Best Case";
        }
        else if (iterationsDiff <= averageCaseIterations)
        {
            textMeshPro.text += "\nT(n): " + iterations;
            textMeshPro.text += "\nTime Complexity: O(log n)";
            textMeshPro.text += "\nCase Complexity: Average Case";
        }
        else
        {
            textMeshPro.text += "\nT(n): " + iterations;
            textMeshPro.text += "\nTime Complexity: O(log n)";
            textMeshPro.text += "\nCase Complexity: Worst Case";
        }
    }

    void SortCubesByNumber()
    {
        cubeList.Sort((a, b) =>
        {
            int aNumber = int.Parse(a.transform.GetChild(0).GetComponent<TextMeshPro>().text);
            int bNumber = int.Parse(b.transform.GetChild(0).GetComponent<TextMeshPro>().text);
            return aNumber.CompareTo(bNumber);
        });

        // Reorder the cubes based on the sorted cubeList
        for (int i = 0; i < cubeList.Count; i++)
        {
            int numColumns = Mathf.CeilToInt(Mathf.Sqrt(arraySize));
            float spacing = 0.1f;
            float cubeWidth = (5f - (numColumns - 1) * spacing) / numColumns;
            float cubeHeight = cubeWidth;

            GameObject cube = cubeList[i];
            int row = i / numColumns;
            int column = i % numColumns;

            float xPos = column * (cubeWidth + spacing) + cubeWidth / 2f;
            float yPos = (numColumns - 1 - row) * (cubeHeight + spacing) + cubeHeight / 2f;

            cube.transform.position = new Vector3(xPos, yPos, 0f);
        }
    }
}
