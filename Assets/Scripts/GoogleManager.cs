using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Networking;



#if !UNITY_WEBGL
using Google;
#endif
using System.Threading.Tasks;

public class GoogleManager : Singleton<GoogleManager>
{
    public string Firstname, Lastname, Birthday, Email, Gender, ProfileImageLink;
    public Texture ProfilePicture;
    public bool OnProcess;
    public bool LoginSuccess;

    public string webClientId = "<your client id here>";
#if !UNITY_WEBGL
    private GoogleSignInConfiguration configuration;

    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true // Ensure email is requested
        };

        // Attempt silent sign-in when the app starts
        SignInSilently();
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        ClearUserData();
        Debug.Log("User signed out.");
        //MenuManager.Instance.ShowLoginPage(); // Show login page after logout
    }

    public void OnDisconnect()
    {
        GoogleSignIn.DefaultInstance.Disconnect();
        ClearUserData();
        Debug.Log("User disconnected.");
        //MenuManager.Instance.ShowLoginPage(); // Show login page after disconnect
    }
#if PLATFORM_ANDROID
    private void SignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }
#else
    private void SignInSilently()
    {
        // ตั้งค่า Google Sign-In ตามที่ต้องการ
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        // พยายาม sign-in แบบเงียบ ซึ่งเทียบเท่ากับ restorePreviousSignIn ใน iOS ดั้งเดิม
        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // หากการ sign-in แบบเงียบล้มเหลวหรือถูกยกเลิก ให้แสดงหน้าเข้าสู่ระบบ
                Debug.LogWarning("Silent sign-in failed or was canceled.");
                MenuManager.Instance.ShowLoginPage();
            }
            else
            {
                // การ sign-in แบบเงียบสำเร็จ
                Debug.Log("Silent sign-in successful. Welcome: " + task.Result.DisplayName);
                Firstname = task.Result.GivenName;
                Lastname = task.Result.FamilyName;
                Email = task.Result.Email;
                ProfileImageLink = task.Result.ImageUrl.ToString();

                // โหลดรูปโปรไฟล์แบบ asynchronous หากจำเป็น
                StartCoroutine(LoadProfilePicture(ProfileImageLink));
                MenuManager.Instance.AutoLoginGoogle(); // เข้าสู่ระบบอัตโนมัติหากสำเร็จ
            }
        });
    }
#endif
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception: " + task.Exception);
                }
            }
            LoginSuccess = false;
            //MenuManager.Instance.ShowLoginPage(); // Show login page if sign-in failed
        }
        else if (task.IsCanceled)
        {
            Debug.LogWarning("Sign-in canceled.");
            LoginSuccess = false;
            //MenuManager.Instance.ShowLoginPage(); // Show login page if sign-in canceled
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Firstname = task.Result.GivenName;
            Lastname = task.Result.FamilyName;
            Email = task.Result.Email;
            ProfileImageLink = task.Result.ImageUrl.ToString();

            // Load the profile picture asynchronously if needed
            StartCoroutine(LoadProfilePicture(ProfileImageLink));
            //MenuManager.Instance.AutoLoginGoogle(); // Automatically login if successful

            LoginSuccess = true;
        }
        OnProcess = false;
    }

    private IEnumerator LoadProfilePicture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest(); // ส่งคำขอและรอจนกว่าจะเสร็จสิ้น

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // ตรวจสอบว่ามีข้อผิดพลาดในการเชื่อมต่อหรือโปรโตคอล
            Debug.LogError("Failed to load profile picture: " + request.error);
        }
        else
        {
            // สำเร็จ ใช้ texture ที่ได้รับจากเว็บ
            ProfilePicture = DownloadHandlerTexture.GetContent(request);
            Debug.Log("Profile Picture Loaded.");
        }
        HomeManager.Instance.LoginSuccess();

    }

    private void ClearUserData()
    {
        Firstname = "";
        Lastname = "";
        Email = "";
        ProfileImageLink = "";
        ProfilePicture = null;
        LoginSuccess = false;
        OnProcess = false;
    }
#endif
}
