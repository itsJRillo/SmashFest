using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject menuUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void LoginScreen(){
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }

    public void RegisterScreen(){
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }

    public void menuScreen(){
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }
}
