using System.Collections;
using System.Collections.Generic;
using OscCore;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OSC_portNumber : MonoBehaviour
{
    public static OSC_portNumber instance;
    OscReceiver[] oscReceivers;
    int portNum;
    public InputField inputField;

    void Awake()
    {
        instance = this;
        oscReceivers = GetComponentsInChildren<OscReceiver>();


        if(PlayerPrefs.GetInt("port")!=0)
        {
            Debug.Log("current port num = " + PlayerPrefs.GetInt("port"));
            portNum = PlayerPrefs.GetInt("port");

        }
        else
        {
            PlayerPrefs.SetInt("port",8888);
            portNum = PlayerPrefs.GetInt("port");
            Debug.Log("set port num 8888 ");

        }

        // portNum=7777;
        setPortNum(portNum);

    }

    void setPortNum(int num)
    {
        inputField.text = num.ToString();
        for(int i =0;i<transform.childCount;i++)
        {
            oscReceivers[i].Port = num;
        }

        // PlayerPrefs.SetInt("port",num);

    }

    public void _restart()
    {
        PlayerPrefs.SetInt("port",int.Parse(inputField.text));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
