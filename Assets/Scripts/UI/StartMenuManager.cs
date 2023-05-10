using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class StartMenuManager : MonoBehaviour {
    
    private DependencyStatus status;
    private DatabaseReference db;
    FirebaseAuth auth;
    FirebaseUser user;
    public TMP_Text username;

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

    void Start() {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        if (user != null) {
            string displayName = user.DisplayName;
            string uid = user.UserId;

            username.text = displayName;
        } else {
            Debug.Log("Not");
        }
    }

    public void Play() {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void Training() {
        
        SceneManager.LoadScene("CharacterSelection");
    }

    public void Multiplayer() {
        
        SceneManager.LoadScene("LoadingScene");
    }

}
