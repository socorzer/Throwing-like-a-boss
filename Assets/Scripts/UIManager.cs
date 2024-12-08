using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform _chargingGageTransform;
    [SerializeField] Image _chargingGage;

    [Header("HPBar")]
    [SerializeField] List<Image> _hpGages;

    [Header("WindBar")]
    [SerializeField] List<Image> _windGages;

    [Header("Marker")]
    [SerializeField] Transform _markerTransform;
    [SerializeField] Transform _arrowTransform;


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
    public void UpdateHPBar(List<PlayerStateMachine> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            _hpGages[i].fillAmount = players[i].GetPlayerHPPercent();
        }
    }
    public void SetMarkerPosition(Vector2 position)
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(position);
        _markerTransform.position = screenPosition;
    }
    public void SetWindGage(float windPower)
    {
        if (windPower > 0)
        {
            _windGages[0].fillAmount = 0;
            _windGages[1].fillAmount = windPower;
            _arrowTransform.eulerAngles = new Vector3(0, 0, 180);

        }
        else if(windPower < 0)
        {
            _windGages[0].fillAmount = Mathf.Abs(windPower);
            _windGages[1].fillAmount = 0;
            _arrowTransform.eulerAngles = new Vector3(0, 0, 0);

        }
        else
        {
            _windGages[0].fillAmount = 0;
            _windGages[1].fillAmount = 0;
        }
        _arrowTransform.gameObject.SetActive(windPower != 0);
    }

}
