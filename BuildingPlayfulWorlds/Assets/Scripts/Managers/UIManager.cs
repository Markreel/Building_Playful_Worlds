using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text healthText;
    [SerializeField] Image healthImage;
    [SerializeField] AnimationCurve healthbarCurve;
    [SerializeField] [Range(0,1)] float lerpDuration;
    Coroutine LerpHealthbarRoutine;

    [Space]

    [SerializeField]
    GameObject gameOverCanvas;
    [SerializeField]
    GameObject victoryCanvas;


    void Awake()
    {
        Instance = this;   
    }

    /// <summary>
    /// Updates the health text and starts lerping the healthbar
    /// </summary>
    /// <param name="_newHealth"></param>
    /// <param name="_maxHealth"></param>
    public void UpdateHealth(float _newHealth, float _maxHealth)
    {
        healthText.text =  ((int)_newHealth).ToString();

        StartLerpingHealthbar(1 / _maxHealth * _newHealth);
    }

    void StartLerpingHealthbar(float _newHealth)
    {
        if (LerpHealthbarRoutine != null) StopCoroutine(LerpHealthbarRoutine);
        LerpHealthbarRoutine = StartCoroutine(ILerpHealthbar(_newHealth));
    }
    /// <summary>
    /// Smoothly animates the change in health over the healthbar
    /// </summary>
    /// <param name="_newHealth"></param>
    /// <returns></returns>
    IEnumerator ILerpHealthbar(float _newHealth)
    {
        float _currentHealth = healthImage.fillAmount;
        float _tweenTime = 0;


        while (_tweenTime < 1)
        {
            _tweenTime += Time.deltaTime / lerpDuration;
            float _tweenKey = healthbarCurve.Evaluate(_tweenTime);

            healthImage.fillAmount = Mathf.Lerp(_currentHealth, _newHealth, _tweenKey);
            yield return null;
        }

        yield return null;
    }

    //Opens the game over canvas
    public void OpenGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
    }
    //Opens the victory canvas
    public void OpenVictoryCanvas()
    {
        victoryCanvas.SetActive(true);
    }
}
