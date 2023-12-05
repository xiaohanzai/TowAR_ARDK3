using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class MainMenuUI : MonoBehaviour {

    private void Awake() {
        transform.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene_FactorySim);
        });

        transform.Find("QuitBtn").GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });

        transform.Find("CodeMonkeyBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Application.OpenURL("https://www.youtube.com/c/CodeMonkeyUnity");
        };
    }

}
