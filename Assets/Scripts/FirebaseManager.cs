using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference db;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    void Awake() {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {

                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    //Function for the login button
    public void LoginButton() {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    
    //Function for the register button
    public void RegisterButton() {
        StartCoroutine(UpdateUsernameDatabase(usernameRegisterField.text));
        StartCoroutine(UpdateEmailDatabase(emailRegisterField.text));
        StartCoroutine(UpdateWinsDatabase(0));

        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password) {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null) {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
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
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("Usuario ha iniciado sesión: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logeado";
            UIManager.instance.MenuScreen();
        }
    }

    private void SignOutButton(){
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearLoginFields();
        ClearRegisterFields();
    }

    private IEnumerator Register(string _email, string _password, string _username) {
        if (_username == "") {
            warningRegisterText.text = "Falta el nombre de usuario";
        } else if (passwordRegisterField.text != passwordRegisterVerifyField.text) {
            
            warningRegisterText.text = "La contraseña no coincide!";
        } else {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null) {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
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
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.MenuScreen();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username){
        var DBTask = db.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            // Database username is now updated
        }
    }

    private IEnumerator UpdateEmailDatabase(string _email){
        var DBTask = db.Child("users").Child(User.UserId).Child("email").SetValueAsync(_email);
        
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            // Database username is now updated
        }
    }

    private IEnumerator UpdateWinsDatabase(int _wins){
        var DBTask = db.Child("users").Child(User.UserId).Child("wins").SetValueAsync(_wins);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            // Database email is now updated
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }        
    }

/*
    private IEnumerator LoadUserData(){
        //Get the currently logged in user data
        var DBTask = db.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            // winsField.text = snapshot.Child("wins").Value.ToString();
        }
    }

    private IEnumerator LoadScoreboardData(){
        //Get all the users data ordered by kills amount
        var DBTask = db.Child("users").OrderByChild("kills").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());
                int deaths = int.Parse(childSnapshot.Child("deaths").Value.ToString());
                int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
            }

            //Go to scoareboard screen
            UIManager.instance.ScoreboardScreen();
        }
    }
*/


}
