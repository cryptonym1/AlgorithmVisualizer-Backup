using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Valueupdate : MonoBehaviour
{
    public TMP_InputField inputField;
    public Slider slider;

    private void Awake()
    {
        inputField.onEndEdit.AddListener(OnInputFieldChanged);
        slider.onValueChanged .AddListener( OnSliderChanged);

        OnSliderChanged(slider.value);
    }

    private void OnInputFieldChanged(string newText)
    {
        if (float.TryParse(newText, out var value))
        {
            value = Mathf.Clamp(value, slider.minValue, slider.maxValue);

            inputField.text = value.ToString("0");
            slider.value = value;
        }
        else
        {
            Debug.LogWarning("Input Format Error!", this);
            slider.value = Mathf.Clamp(0, slider.minValue, slider.maxValue);
            inputField.text = slider.value.ToString("0");
        }
    }

    private void OnSliderChanged(float newValue)
    {
        inputField.text = newValue.ToString("0");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
