using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using System.Collections;

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

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            windowAdjust_forWindows();
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rect rect);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern long GetWindowLong(IntPtr hWnd, int nIndex);

    private const int GWL_STYLE = -16;
    private const long WS_CAPTION = 0x00C00000L;

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }


    void windowAdjust_forWindows()
    {
        // 現在のウィンドウハンドルを取得
        IntPtr windowHandle = GetForegroundWindow();

        // ウィンドウスタイルを設定してタイトルバーを表示
        long style = GetWindowLong(windowHandle, GWL_STYLE);
        SetWindowLong(windowHandle, GWL_STYLE, style | WS_CAPTION);

        // 画面の右上にウィンドウを移動
        StartCoroutine(MoveWindowToPosition());
    }

    IEnumerator MoveWindowToPosition()
    {
        yield return new WaitForSeconds(0.1f); // 少し待ってから位置を設定

        while (true)
        {
            // 現在のウィンドウハンドルを取得
            IntPtr windowHandle = GetForegroundWindow();

            // ウィンドウの位置とサイズを取得
            Rect rect = new Rect();
            GetWindowRect(windowHandle, ref rect);

            // 画面の右上にウィンドウを移動
            int screenWidth = Screen.currentResolution.width;
            int newX = screenWidth - (rect.Right - rect.Left);
            int newY = 0;  // 上端

            // ウィンドウを移動
            MoveWindow(windowHandle, newX, newY, rect.Right - rect.Left, rect.Bottom - rect.Top, true);

            yield return new WaitForSeconds(0.5f); // 定期的に位置を設定
        }
    }

    public void _shutdownApp()
    {
        Application.Quit();
    }
}