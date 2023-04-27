using UnityEngine;
using System.Collections;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour {

    private DependencyStatus status;
    private FirebaseAuth auth;
    private FirebaseUser Usuari;
    private DatabaseReference db;

    //Login variables
    [Header("Inicio de sesión")]
    public TMP_InputField correuLoginInput;
    public TMP_InputField contrasenaLoginInput;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Registro de sesión")]
    public TMP_InputField nomRegisterInput;
    public TMP_InputField correuRegisterInput;
    public TMP_InputField contrasenaRegisterInput;
    public TMP_InputField contrasenaRegisterReInput;
    public TMP_Text warningRegisterText;

    [Header("Leaderboard")]
    public GameObject leaderboard;
    public Transform leaderboardContent;

    void Awake() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            status = task.Result;
            if (status == DependencyStatus.Available) {
                InitializeFirebase();
            } 
        });
    }

    private void InitializeFirebase() {
        auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        if (Application.isEditor) {
            database.SetPersistenceEnabled(false);
        }

        db = database.RootReference;
    }

    public void ClearLoginFields() {
        correuLoginInput.text = "";
        contrasenaLoginInput.text = "";
    }

    public void ClearRegisterFields() {
        nomRegisterInput.text = "";
        correuRegisterInput.text = "";
        contrasenaRegisterInput.text = "";
        contrasenaRegisterReInput.text = "";
    }

    public void LoginButton() {
        StartCoroutine(Login(correuLoginInput.text, contrasenaLoginInput.text));
    }
    
    public void RegisterButton() {
        StartCoroutine(Register(correuRegisterInput.text, contrasenaRegisterInput.text, nomRegisterInput.text));
    }

    private IEnumerator Login(string correu, string contrasena) {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(correu, contrasena);
    
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null) {
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Inicio de sesión fallido!";
            switch (errorCode) {
                case AuthError.MissingEmail:
                    message = "Falta el correo";
                    break;
                case AuthError.MissingPassword:
                    message = "Falta la contraseña";
                    break;
                case AuthError.WrongPassword:
                    message = "Contraseña errónea";
                    break;
                case AuthError.InvalidEmail:
                    message = "Correo inválido";
                    break;
                case AuthError.UserNotFound:
                    message = "La cuenta no existe";
                    break;
            }
            warningLoginText.text = message;

        } else {
            Usuari = LoginTask.Result;
            warningLoginText.text = "";
            confirmLoginText.text = "Sesión iniciada";

            yield return new WaitForSeconds(2);

            SceneManager.LoadScene("MainScene");
            confirmLoginText.text = "";
        }
    }

    private IEnumerator Register(string correu, string contrasena, string nom) {
        if (nom == "") {
            warningRegisterText.text = "Falta el nombre de usuario";
            
        } else if (contrasenaRegisterInput.text != contrasenaRegisterReInput.text) {
            
            warningRegisterText.text = "La contraseña no coincide!";
        } else {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(correu, contrasena);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null) {
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Registro de usuario fallido!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Falta el correo";
                        break;
                    case AuthError.MissingPassword:
                        message = "Falta la contraseña";
                        break;
                    case AuthError.WeakPassword:
                        message = "Contraseña débil";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Correo ya existente";
                        break;
                }
                warningRegisterText.text = message;

            } else {
                Usuari = RegisterTask.Result;

                if (Usuari != null) {
                    UserProfile profile = new UserProfile { DisplayName = nom };
                    var ProfileTask = Usuari.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null) {
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "El proceso ha fallado";

                    } else {
                        StartCoroutine(UpdateUsernameAuth(nomRegisterInput.text));
                        StartCoroutine(UpdateUsernameDatabase(nomRegisterInput.text));
                        StartCoroutine(UpdateEmailDatabase(correuRegisterInput.text));
                        StartCoroutine(UpdateWinsDatabase(0));
                        
                        IngressManager.instance.LoginScreen();
                    }
                }
            }
        }
    }
    
    public void SignOutButton() {
        auth.SignOut();
        IngressManager.instance.LoginScreen();
        ClearLoginFields();
        ClearRegisterFields();
    }

    public void leaderboardContentLoaded() {
        StartCoroutine(LoadLeaderboardData());
    }

    private IEnumerator UpdateUsernameAuth(string nom) {
        UserProfile profile = new UserProfile { DisplayName = nom };
        var ProfileTask = Usuari.UpdateUserProfileAsync(profile);

        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);   
    }

    private IEnumerator UpdateUsernameDatabase(string nom) {
        var DBTask = db.Child("usuaris").Child(Usuari.UserId).Child("nom").SetValueAsync(nom);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    private IEnumerator UpdateEmailDatabase(string correu) {
        var DBTask = db.Child("usuaris").Child(Usuari.UserId).Child("correu").SetValueAsync(correu);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    private IEnumerator UpdateWinsDatabase(int victorias) {
        var DBTask = db.Child("usuaris").Child(Usuari.UserId).Child("victorias").SetValueAsync(victorias);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    public IEnumerator LoadLeaderboardData() {
        var DBTask = db.Child("usuaris").OrderByChild("victorias").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in leaderboardContent.transform) {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>()) {
                string nom = childSnapshot.Child("nom").Value.ToString();
                int victorias = int.Parse(childSnapshot.Child("victorias").Value.ToString());

                GameObject leaderboardElement = Instantiate(leaderboard, leaderboardContent);
                leaderboardElement.GetComponent<Leaderboard>().SetLeaderboardUser(nom.ToLower(), victorias);                
            }

            UIManager.instance.LeaderboardScreen();
        }
    }
}
