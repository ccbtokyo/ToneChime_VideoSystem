using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class text_displayLength : MonoBehaviour
{
    TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Display.displays.Length.ToString();
    }
}
