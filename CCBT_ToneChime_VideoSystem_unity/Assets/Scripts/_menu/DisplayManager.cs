using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    public Camera mainCamera,UI_Camera;
    public GameObject UI_Canva,UI_bg;
    public Canvas canvas_black;

    void Start()
    {
        projection();
    }

    public void projection()
    {

        // ディスプレイの数を取得します
        int displayCount = Display.displays.Length;
        Debug.Log("Connected displays: " + displayCount);

        // すべてのディスプレイをアクティブにします
        for (int i = 1; i < displayCount; i++)
        {
            Display.displays[i].Activate();
        }

        
        if(displayCount==1)
        {
            mainCamera.targetDisplay = 0;
            canvas_black.targetDisplay = 0;
            UI_bg.SetActive(false);
            Screen.fullScreen = !Screen.fullScreen;
            Screen.SetResolution(1920,1080,false);
        }
        else
        {
            mainCamera.targetDisplay = 1;
            canvas_black.targetDisplay = 1;
            Screen.fullScreen = !Screen.fullScreen;
            Screen.SetResolution(500,800,false);
        }

    }

    // public void OutputToDisplay(int displayIndex)
    // {
    //     if (displayIndex < Display.displays.Length)
    //     {
    //         // 特定のディスプレイにカメラを割り当てます
    //         Camera.main.targetDisplay = displayIndex;
    //         Debug.Log("Outputting to display: " + displayIndex);

    //         // フルスクリーンモードに設定します
    //         Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    //         Screen.SetResolution(Display.displays[displayIndex].systemWidth, Display.displays[displayIndex].systemHeight, true);
    //     }
    //     else
    //     {
    //         Debug.LogError("Invalid display index");
    //     }
    // }

    public void _shutdownApp()
    {
        Application.Quit();
    }
}