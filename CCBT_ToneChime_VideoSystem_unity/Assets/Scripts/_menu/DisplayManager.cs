using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics; // 追加

public class DisplayManager : MonoBehaviour
{
    public Camera mainCamera, UI_Camera;
    // public GameObject UI_Canva, UI_bg;
    public Canvas canvas_black;

    private IntPtr unityWindowHandle; // Unityのウィンドウハンドル

    void Start()
    {
        // Unityウィンドウのハンドルを取得
        unityWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
        UnityEngine.Debug.Log("Unity Window Handle: " + unityWindowHandle); // 明示的に指定
        projection();
    }

    public void projection()
    {
        int displayCount = Display.displays.Length;
        UnityEngine.Debug.Log("Connected displays: " + displayCount); // 修正

        for (int i = 1; i < displayCount; i++)
        {
            Display.displays[i].Activate();
        }

        if (displayCount == 1)
        {
            mainCamera.targetDisplay = 0;
            canvas_black.targetDisplay = 0;
            // UI_bg.SetActive(false);
            Screen.fullScreen = !Screen.fullScreen;
            Screen.SetResolution(1920, 1080, false);
        }
        else
        {
            mainCamera.targetDisplay = 1;
            canvas_black.targetDisplay = 1;
            Screen.fullScreen = !Screen.fullScreen;
            Screen.SetResolution(500, 800, false);
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            windowAdjust_forWindows();
        }
    }

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
        if (unityWindowHandle == IntPtr.Zero) return;

        long style = GetWindowLong(unityWindowHandle, GWL_STYLE);
        SetWindowLong(unityWindowHandle, GWL_STYLE, style | WS_CAPTION);

        UnityEngine.Debug.Log("Adjusting window position"); // 修正
        StartCoroutine(MoveWindowToPosition());
    }

    IEnumerator MoveWindowToPosition()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (unityWindowHandle == IntPtr.Zero) yield break;

            Rect rect = new Rect();
            GetWindowRect(unityWindowHandle, ref rect);

            int screenWidth = Screen.currentResolution.width;
            int newX = screenWidth - (rect.Right - rect.Left);
            int newY = 0;

            MoveWindow(unityWindowHandle, newX, newY, rect.Right - rect.Left, rect.Bottom - rect.Top, true);

            UnityEngine.Debug.Log("Window moved to: " + newX + ", " + newY); // 修正
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void _shutdownApp()
    {
        Application.Quit();
    }
}
