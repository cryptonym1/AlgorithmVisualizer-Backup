using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchAlgo()
    {
        // Load the specified scene
        SceneManager.LoadScene("SearchAlgo");
    }

    public void SortAlgo()
    {
        SceneManager.LoadScene("SortAlgo");
    }

    public void Pathfind()
    {
        SceneManager.LoadScene("Pathfinding Visualizer");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void exit()
    {
        Application.Quit();
    }
}
