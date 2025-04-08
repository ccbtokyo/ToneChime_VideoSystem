using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TS.ColorPicker;

public class ColorChanger : MonoBehaviour
{
    Image myImage;
    public ColorPicker colorPicker;
    int myIndex;
    Color currentColor;
    void Start()
    {
        myImage = GetComponent<Image>();
        myIndex = transform.GetSiblingIndex();
        myImage.color = ParticlesHolder.instance.colors[myIndex];
        currentColor = myImage.color;

        if (colorPicker != null)
        {
            // colorPicker.OnChanged.AddListener(UpdateColor);
            colorPicker.OnSubmit.AddListener(UpdateColor);
            colorPicker.OnChanged.AddListener(selectingColor);
            colorPicker.OnCancel.AddListener(cancelColor);

        }
    }

    void cancelColor()
    {
        if(myImage != null)
        {
            myImage.color = currentColor;
            ParticlesHolder.instance.colors[myIndex] = currentColor;

        }
    }

    void selectingColor(Color color)
    {
        if(myImage != null)
        {
            myImage.color = color;
            ParticlesHolder.instance.colors[myIndex] = color;

        }
    }

    void UpdateColor(Color color)
    {
        if (myImage != null)
        {
            myImage.color = color;
            ParticlesHolder.instance.colors[myIndex] = color;
            currentColor = color;
        }
    }

    public void showColorPicker()
    {
        colorPicker.gameObject.SetActive(true);
    }

    public void RefreshColorFromParticles()
    {
        if (myImage != null)
        {
            myImage.color = ParticlesHolder.instance.colors[myIndex];
            currentColor = myImage.color;
        }
    }


}
