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
        outputText.text = "���������� ������ �����Ǿ����ϴ�!";
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
            outputText.text = "���������� �α��� �Ǿ����ϴ�";
            LoadLobbyScene();
        }
        else
        {
            outputText.text = "�̸���/��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
        }
    }
}
