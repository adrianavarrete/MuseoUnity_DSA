using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBackScript : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                androidAtras();
                Application.Quit();
            }
        }
    }

    public void androidAtras()
    {
#if UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaObject("com.example.dsaproyectofront.MenuActivity");

        activity.Call("botonAtras", new object[] { GameManager.instance.idUser });
#endif


    }
}
