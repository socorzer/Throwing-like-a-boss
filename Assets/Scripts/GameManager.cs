using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStateMachine _gameStateMachine;
    [SerializeField] List<PlayerStateMachine> _playerStateMachines;

    public float CurrentWind { get; private set; }
    [SerializeField] float _windMultiplier;
    [SerializeField] AreaEffector2D _windEffector;


    public GameStateMachine GameStateMachine { get { return _gameStateMachine; } }

    public void ChangePlayerTurn()
    {
        _gameStateMachine.ChangePlayerTurn();
    }
    public void PlayerTakeDamage()
    {
        UIManager.Instance.UpdateHPBar(_playerStateMachines);
    }
    public void RandomWind()
    {
        bool isRightWind = Random.Range(0, 2) == 0 ? false : true;
        float windPower = Random.Range(0f, 1.0f);

        if (windPower < 0.05f && windPower > -0.05f) windPower = 0;
        CurrentWind = isRightWind ? windPower : -windPower;
        SetWindEffector();
        UIManager.Instance.SetWindGage(CurrentWind);
    }
    public void SetWindEffector()
    {
        _windEffector.forceMagnitude = CurrentWind * _windMultiplier;
    }
    public void ShareScreenShot()
    {
        // Panel_share.SetActive(true);//show the panel
        StartCoroutine(TakeScreenShotAndShare()) ;
    }
    IEnumerator TakeScreenShotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D tx = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tx.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tx.Apply();

        string path = Path.Combine(Application.temporaryCachePath, "sharedImage.png");//image name
        File.WriteAllBytes(path, tx.EncodeToPNG());

        Destroy(tx); //to avoid memory leaks

        new NativeShare()
            .AddFile(path)
            .SetSubject("This is my score")
            .SetText("share your score with your friends")
            .Share();


        //Panel_share.SetActive(false); //hide the panel
    }
    public void SetUpPlayer()
    {
        _gameStateMachine.SetupPlayer(_playerStateMachines);
        UIManager.Instance.ShowModeSelectUI(false);
    }
    public void EndGame()
    {
        _gameStateMachine.EndGame();    
    }
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void GoToHome()
    {
        SceneManager.LoadScene("Home");
    }

}
