using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FireBaseAuthUIController : MonoBehaviour
{
    public InputField emailInputField;
    public InputField passwordInputField;
    public TextMeshProUGUI outputText;

    void Start()
    {
        FireBaseAuthManager.Instance.OnChangedLoginState += OnChangedLoginState;
        FireBaseAuthManager.Instance.InitializeFirebase();
    }

    public void CreateUser()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        FireBaseAuthManager.Instance.Create(email, password);
        outputText.text = "성공적으로 계정이 생성되었습니다!";
    }

    public void SigedIn()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        FireBaseAuthManager.Instance.Login(email, password);
    }

    void LoadLobbyScene()
    {
        SceneManager.LoadScene(1);
    }

    public void SigedOut()
    {
        FireBaseAuthManager.Instance.LogOut();
    }

    private void OnChangedLoginState(bool signedIn)
    {
        if (signedIn)
        {
            outputText.text = "성공적으로 로그인 되었습니다";
            LoadLobbyScene();
        }
        else
        {
            outputText.text = "이메일/비밀번호가 일치하지 않습니다.";
        }
    }
}
