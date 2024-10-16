using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image _totalHealthBar;
    [SerializeField] private Image _currentHealthBar;

    void Start()
    {
        _totalHealthBar.fillAmount = _playerHealth.currentHealth / 10;
    }

    void Update()
    {
        _currentHealthBar.fillAmount = _playerHealth.currentHealth / 10;
    }
}
