using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Collections;
using Firebase.Extensions; // BUNU EKLE

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public TextMeshProUGUI feedbackText;

    private FirebaseAuth auth;
    private bool firebaseReady = false;

    void Start()
    {
        feedbackText.text = "Initializing Firebase...";

        // Başta butonları kapatalım, hazır olunca açacağız
        loginButton.interactable = false;
        registerButton.interactable = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                firebaseReady = true;

                feedbackText.text = ""; // Temizle
                Debug.Log("Firebase Ready");

                // Artık butonlara izin verebiliriz
                loginButton.interactable = true;
                registerButton.interactable = true;
            }
            else
            {
                firebaseReady = false;
                feedbackText.text = "Firebase Error: " + status;
                Debug.LogError("Firebase dependencies error: " + status);
            }
        });

        // Listener'lar Start'ta kalabilir
        loginButton.onClick.AddListener(LoginUser);
        registerButton.onClick.AddListener(RegisterUser);
    }

    public void RegisterUser()
    {
        if (!firebaseReady)
        {
            feedbackText.text = "Firebase not ready yet!";
            return;
        }

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please fill all fields!";
            return;
        }

        if (password.Length < 6)
        {
            feedbackText.text = "Password must be at least 6 characters!";
            return;
        }

        feedbackText.text = "Creating account...";
        loginButton.interactable = false;
        registerButton.interactable = false;

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                loginButton.interactable = true;
                registerButton.interactable = true;

                if (task.IsCanceled)
                {
                    feedbackText.text = "Registration canceled!";
                    return;
                }

                if (task.IsFaulted)
                {
                    feedbackText.text = "Registration failed!";
                    Debug.LogError(task.Exception);
                    return;
                }

                feedbackText.text = "Account created! You can login now.";
                feedbackText.color = Color.green;
                StartCoroutine(ResetFeedbackColor());
            });
    }

    public void LoginUser()
    {
        if (!firebaseReady)
        {
            feedbackText.text = "Firebase not ready yet!";
            return;
        }

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please fill all fields!";
            return;
        }

        feedbackText.text = "Logging in...";
        loginButton.interactable = false;
        registerButton.interactable = false;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                loginButton.interactable = true;
                registerButton.interactable = true;

                if (task.IsCanceled)
                {
                    feedbackText.text = "Login canceled!";
                    return;
                }

                if (task.IsFaulted)
                {
                    feedbackText.text = "Login failed! Check credentials.";
                    Debug.LogError(task.Exception);
                    return;
                }

                feedbackText.text = "Login successful!";
                feedbackText.color = Color.green;

                // Oyun sahnesine geç
                SceneManager.LoadScene("GameScene");
            });
    }

    IEnumerator ResetFeedbackColor()
    {
        yield return new WaitForSeconds(2f);
        feedbackText.color = new Color(1f, 0.32f, 0.32f);
    }
}
