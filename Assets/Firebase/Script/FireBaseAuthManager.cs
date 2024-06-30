using System;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.SceneManagement;

public class FireBaseAuthManager : MonoBehaviour
{

    private static FireBaseAuthManager instance = null;
    public static FireBaseAuthManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private FirebaseAuth auth;
    private FirebaseUser user;
    private string displayName;
    private string emailAddress;
    private Uri photoUrl;

    public string UserId => user?.UserId ?? string.Empty;
    public string DisplayName => displayName;
    public string EmailAddress => emailAddress;
    public Uri PhotoUrl => photoUrl;
    public Action<bool> OnChangedLoginState;

    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;
        OnAuthStateChanged(this, null);
        auth.SignOut();
    }

    private int GetFirebaseErrorCode(AggregateException exception)
    {
        FirebaseException firebaseException = null;
        foreach (Exception e in exception.Flatten().InnerExceptions)
        {
            firebaseException = e as FirebaseException;
            if (firebaseException != null)
            {
                break;
            }
        }
        return firebaseException?.ErrorCode ?? 0;
    }

    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                int errorCode = GetFirebaseErrorCode(task.Exception);
                switch (errorCode)
                {
                    case (int)AuthError.EmailAlreadyInUse:
                        Debug.LogError("Email Already In Use");
                        break;
                    case (int)AuthError.InvalidEmail:
                        Debug.LogError("Invalid Email");
                        break;
                    case (int)AuthError.WeakPassword:
                        Debug.LogError("Weak Password");
                        break;
                }
                return;
            }
            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
            Debug.LogError("Create Success!");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                int errorCode = GetFirebaseErrorCode(task.Exception);
                switch (errorCode)
                {
                    case (int)AuthError.WrongPassword:
                        Debug.LogError("Wrong Password");
                        break;
                    case (int)AuthError.UnverifiedEmail:
                        Debug.LogError("Unverified Email");
                        break;
                    case (int)AuthError.InvalidEmail:
                        Debug.LogError("Invalid Email");
                        break;
                }
                return;
            }
            AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;
            Debug.LogError("Login Success!");
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.LogError("LogOut Success!");
    }

    private void OnAuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out: " + user.UserId);
                OnChangedLoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in: " + user.UserId);
                displayName = user.DisplayName ?? "";
                emailAddress = user.Email ?? "";
                photoUrl = user.PhotoUrl ?? null;
                OnChangedLoginState?.Invoke(true);
                SceneManager.LoadScene(1);
            }
        }
    }
}
