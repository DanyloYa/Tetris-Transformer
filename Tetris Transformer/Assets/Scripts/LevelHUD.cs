using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelHUD : MonoBehaviour
{
    [SerializeField] private Text _scoreCounter;

    private void Start() => PlayerPrefs.SetInt("Score", 0);

    private void Update() => _scoreCounter.text = "Score: " + PlayerPrefs.GetInt("Score");

    public void LoadScene(string name) => SceneManager.LoadScene(name);
}
