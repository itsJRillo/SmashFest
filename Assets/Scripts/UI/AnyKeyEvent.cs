using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyEvent : MonoBehaviour {

    void Update(){
        if (Input.anyKeyDown){
            UIManager.instance.StartMenuScreen();
        }
    }
}
