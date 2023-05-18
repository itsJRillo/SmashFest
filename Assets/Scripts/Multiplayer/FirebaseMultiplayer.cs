using UnityEngine;
using System.Collections;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class FirebaseMultiplayer : MonoBehaviourPunCallbacks {

    private DependencyStatus status;
    private FirebaseAuth auth;
    private FirebaseUser Usuari;
    private DatabaseReference db;
    private string gameSessionId;

    void Awake() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            status = task.Result;
            if (status == DependencyStatus.Available) {
                InitializeFirebase();
                gameSessionId = GenerateGameSessionId();
            } 
        });
    }

    private string GenerateGameSessionId() {
        string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string photonId = PhotonNetwork.LocalPlayer.UserId;
        return timestamp + "_" + photonId;
    }


    private void InitializeFirebase() {
        auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        if (Application.isEditor) {
            database.SetPersistenceEnabled(false);
        }

        db = database.RootReference;
    }

    private IEnumerator UpdateEmailDatabase(string correu) {
        var DBTask = db.Child("usuaris").Child(Usuari.UserId).Child("correu").SetValueAsync(correu);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    private IEnumerator UpdateWinsDatabase(int victorias) {
        var DBTask = db.Child("usuaris").Child(Usuari.UserId).Child("victorias").SetValueAsync(victorias);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
}

