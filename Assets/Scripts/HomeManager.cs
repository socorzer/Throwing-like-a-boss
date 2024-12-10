using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] GoogleManager _google;
    [SerializeField] TextMeshProUGUI NameText;
    public void GoogleSigin()
    {
        _google.OnSignIn();
        StartCoroutine(WaitGoogleSignIn());
    }
    IEnumerator WaitGoogleSignIn()
    {
        yield return new WaitUntil(() => _google.IsDone);

        NameText.text = $"{_google.FirstName} {_google.LastName}";
    }
    public void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
