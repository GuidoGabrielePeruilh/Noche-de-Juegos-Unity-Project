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

    public void SetName(string teamName, Color color, Sprite sprite)
    {
        nameText.text = teamName;
        fillArea.sprite = sprite;
        fillArea.color = Color.white;
    }

    public void SetSliderValue(int currentScore, int maxScore)
    {
        var maxPosibleValue = Mathf.Max(maxScore, 1);
        scoreText.text = currentScore.ToString();
        mySlider.value = (float)currentScore / maxPosibleValue;
    }
}
