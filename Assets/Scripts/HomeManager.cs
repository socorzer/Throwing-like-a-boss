using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class HomeManager : Singleton<HomeManager>
{
    [SerializeField] GameObject _googlePrefab;
    [SerializeField] GoogleManager _google;

    [Header("Login UI")]
    [SerializeField] GameObject _loginUI;

    [Header("Menu UI")]
    [SerializeField] GameObject _menuUI;

    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] RawImage _userImage;
    private void Start()
    {
        Application.targetFrameRate = 120;
        _google = FindObjectOfType<GoogleManager>();

        if (_google != null)
        {
            if (_google.LoginSuccess)
                LoginSuccess();
            else LoginGuest();
        }
        else
        {
            _google = Instantiate(_googlePrefab).GetComponent<GoogleManager>();
            DontDestroyOnLoad(_google);
        }
    }
    public void GoogleSigIn()
    {
        _google.OnSignIn();
    }
    public void GoogleSignOut()
    {
        _google.OnSignOut();
        _menuUI.SetActive(false);
        _loginUI.SetActive(true);
    }
    public void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void LoginSuccess()
    {
        _nameText.text = $"Welcome {_google.Firstname} {_google.Lastname}";
        _userImage.texture = _google.ProfilePicture;
        _menuUI.SetActive(true);
        _loginUI.SetActive(false);
    }
    public void LoginGuest()
    {
        _nameText.text = $"Welcome Guest";
        _userImage.texture = null;
        _menuUI.SetActive(true);
        _loginUI.SetActive(false);
    }
}
