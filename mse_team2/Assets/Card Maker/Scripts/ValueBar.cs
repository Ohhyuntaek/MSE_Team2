using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueBar : MonoBehaviour
{
    // Maximum and minimum values per unit
    private const int MAX_VALUE = 100;
    private const int MIN_VALUE = 0;

    // Base value at the start of the game
    public int basicValue = 100;

    public float Max_Range = 3.8f;  // UI == 3.8*20=76

    // Rect Mask 2D Component for bar Strips
    [SerializeField]
    private RectMask2D barRect;

    private void Start()
    {
        // Initialising the bar
        InitializeBar();
    }

    // Updating the display of bar
    public void UpdateBar(int newHealth)
    {
        // Ensure that the HP value is between the maximum and minimum values
        int currentValue = Mathf.Clamp(newHealth, MIN_VALUE, MAX_VALUE);

        // Update the Right value of Rect Mask 2D to change the length of the bar
        float valuePercentage = (float)currentValue / MAX_VALUE;
        barRect.padding = new Vector4(0f, 0f, Max_Range - (valuePercentage * Max_Range), 0f);
    }

    // Initialising the bar
    private void InitializeBar()
    {
        // Get Rect Mask 2D component
        barRect = GetComponent<RectMask2D>();

        // Setting the initial HP value
        UpdateBar(basicValue);
    }
}
