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


    void Awake()
    {
        Instance = Instance ?? this;   
    }

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
}
