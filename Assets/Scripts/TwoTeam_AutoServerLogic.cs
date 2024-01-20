using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoTeam_AutoServerLogic : MonoBehaviour
{
    public Button buttonToPress; // Reference to the button you want to press automatically.
    public Slider sliderProcessSpeed;
    public Toggle toggle; // Reference to the Toggle component.
    public float pressInterval = 1.0f; // Time interval between button presses (1 second in this case).

    public bool startTesting = false;

    public Canvas autoCanvas;

    void Start()
    {
        autoCanvas.enabled = false;
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        // Start invoking the method to press the button automatically.
        InvokeRepeating("PressButton", 0.0f, pressInterval);
        
    }

    void OnToggleValueChanged(bool isOn)
    {
        startTesting = isOn;
        autoCanvas.enabled = false;

        // You can add additional logic here if needed.
    }

    public void updateProcessSpeed()
    {
        CancelInvoke("PressButton");
        pressInterval = sliderProcessSpeed.value;
        InvokeRepeating("PressButton", 0.0f, pressInterval);
    }

    public void OpenAutoCanvasPressed()
    {
        autoCanvas.enabled = true;
    }

    public void CloseAutoCanvasPressed()
    {
        autoCanvas.enabled = false;
    }

    private void PressButton()
    {
        // Check if the button is interactable before simulating a click.
        if (buttonToPress != null && buttonToPress.interactable && startTesting)
        {
            buttonToPress.onClick.Invoke(); // Simulate a button click.
        }
    }
}
