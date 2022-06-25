using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuHUD : MonoBehaviour
{
    [SerializeField] private Text _fieldSize;
    [SerializeField] private Text _bodyLength;
    [SerializeField] private Text _bodySpeed;
    [SerializeField] private Text _baitCount;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FieldWidth"))
            PlayerPrefs.SetInt("FieldWidth", 8);

        if (!PlayerPrefs.HasKey("FieldHeight"))
            PlayerPrefs.SetInt("FieldHeight", 16);

        if (!PlayerPrefs.HasKey("BodyLength"))
            PlayerPrefs.SetInt("BodyLength", 3);

        if (!PlayerPrefs.HasKey("BodySpeed"))
            PlayerPrefs.SetInt("BodySpeed", 3);

        if (!PlayerPrefs.HasKey("BaitCount"))
            PlayerPrefs.SetInt("BaitCount", 1);

        if (!PlayerPrefs.HasKey("Score"))
            PlayerPrefs.SetInt("Score", 0);


        _fieldSize.text = PlayerPrefs.GetInt("FieldWidth") + " x " + PlayerPrefs.GetInt("FieldHeight");
         _bodyLength.text = PlayerPrefs.GetInt("BodyLength") + "";
         _bodySpeed.text = PlayerPrefs.GetInt("BodySpeed") + "";
         _baitCount.text = PlayerPrefs.GetInt("BaitCount") + "";
    }

    public void AddFieldSize(int value)
    {
        var width = PlayerPrefs.GetInt("FieldWidth") + value;
        var height = PlayerPrefs.GetInt("FieldHeight") + value + value;

        if (height > 30 || height < 16)
            return;

        PlayerPrefs.SetInt("FieldWidth", width);
        PlayerPrefs.SetInt("FieldHeight", height);

        _fieldSize.text = width + " x " + height;
    }

    public void AddBodyLength(int value)
    {
        var length = PlayerPrefs.GetInt("BodyLength") + value;

        if (length > 6 || length < 3)
            return;

        PlayerPrefs.SetInt("BodyLength", length);

        _bodyLength.text = length + "";
    }

    public void AddBodySpeed(int value)
    {
        var speed = PlayerPrefs.GetInt("BodySpeed") + value;

        if (speed > 5 || speed < 1)
            return;

        PlayerPrefs.SetInt("BodySpeed", speed);

        _bodySpeed.text = speed + "";
    }

    public void AddBaitCount(int value)
    {
        var count = PlayerPrefs.GetInt("BaitCount") + value;

        if (count > 3 || count < 1)
            return;

        PlayerPrefs.SetInt("BaitCount", count);

        _baitCount.text = count + "";
    }

    public void LoadScene(string name) => SceneManager.LoadScene(name);

    public void Quit() => Application.Quit();
}
