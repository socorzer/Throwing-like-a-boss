using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform _chargingGageTransform;
    [SerializeField] Image _chargingGage;

    public void SetChargingGagePosition(Vector2 position)
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(position);
        _chargingGageTransform.position = screenPosition;
    }
    public void ShowChargingGage(bool isShow)
    {
        _chargingGageTransform.gameObject.SetActive(isShow);
    }
    public void SetChargingGageValue(float value)
    {
        _chargingGage.fillAmount = value;
    }

}
