using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayManager : MonoBehaviour
{

    void Awake()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Screen.SetResolution(500,800,false);
    }

    void Start()
    {

        // ディスプレイの数を取得します
        int displayCount = Display.displays.Length;
        Debug.Log("Connected displays: " + displayCount);

        // すべてのディスプレイをアクティブにします
        for (int i = 1; i < displayCount; i++)
        {
            Display.displays[i].Activate();
        }
    }



    public void OutputToDisplay(int displayIndex)
    {
        if (displayIndex < Display.displays.Length)
        {
            // 特定のディスプレイにカメラを割り当てます
            Camera.main.targetDisplay = displayIndex;
            Debug.Log("Outputting to display: " + displayIndex);

            // フルスクリーンモードに設定します
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.SetResolution(Display.displays[displayIndex].systemWidth, Display.displays[displayIndex].systemHeight, true);
        }
        else
        {
            Debug.LogError("Invalid display index");
        }
    }

    public void _shutdownApp()
    {
        Application.Quit();
    }
}