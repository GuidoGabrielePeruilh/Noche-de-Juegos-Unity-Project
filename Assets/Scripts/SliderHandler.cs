using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image fillArea;
    [SerializeField] private Slider mySlider;
    [SerializeField] private float duration = 0.5f;

    public void SetName(string teamName, Color color)
    {
        nameText.text = teamName;
        fillArea.color = color;
    }

    public void SetSliderValue(int currentScore, int maxScore)
    {
        var maxPossibleValue = Mathf.Max(maxScore, 1);
        scoreText.text = currentScore.ToString();
        StartCoroutine(SlideToValue((float)currentScore / maxPossibleValue));
    }

    private IEnumerator SlideToValue(float targetValue)
    {
        float elapsedTime = 0f;
        float startingValue = mySlider.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            mySlider.value = Mathf.Lerp(startingValue, targetValue, elapsedTime / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        mySlider.value = targetValue;
    }
}
