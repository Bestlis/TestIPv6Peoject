using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text statusText;

    void Start()
    {
        StartConnection.Instance.OnStatusChanged += Instance_OnStatusChanged;        
    }

    private void Instance_OnStatusChanged(object sender, string e)
    {
        statusText.text += e + System.Environment.NewLine;
    }
}
