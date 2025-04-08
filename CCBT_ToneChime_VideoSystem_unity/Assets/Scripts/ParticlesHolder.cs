using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParticlesHolder : MonoBehaviour
{
    public Color[] colors;
    private Color[] initialColors;
    private ParticleSystem.MainModule[] modules;

    public static ParticlesHolder instance;

    private string savePath;
    private string initialPath;


    [System.Serializable]
    private class ColorArrayWrapper
    {
        public Color[] colors;
    }

    private void Awake()
    {
        instance = this;
        modules = new ParticleSystem.MainModule[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            modules[i] = transform.GetChild(i).GetComponent<ParticleSystem>().main;
        }

        savePath = Path.Combine(Application.persistentDataPath, "saved_colors.json");
        initialPath = Path.Combine(Application.persistentDataPath, "initial_colors.json");

        if (!File.Exists(initialPath))
        {
            SaveInitialColors();
        }

        LoadInitialColors();
        LoadColors();
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            modules[i].startColor = colors[i];
        }
    }

    private void OnApplicationQuit()
    {
        SaveColors();  // アプリ終了時に必ず保存
    }

    public void SaveColors()
    {
        if (colors == null || colors.Length == 0)
        {
            Debug.LogWarning("[ParticlesHolder] No colors to save.");
            return;
        }

        ColorArrayWrapper wrapper = new ColorArrayWrapper { colors = colors };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[ParticlesHolder] Current colors saved to: " + savePath);
    }

    public void LoadColors()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            ColorArrayWrapper wrapper = JsonUtility.FromJson<ColorArrayWrapper>(json);

            if (wrapper.colors.Length == colors.Length)
            {
                colors = wrapper.colors;
                Debug.Log("[ParticlesHolder] Colors loaded from saved file.");
            }
            else
            {
                Debug.LogWarning("[ParticlesHolder] Saved colors array size mismatch.");
            }
        }
    }

    public void SaveInitialColors()
    {
        ColorArrayWrapper wrapper = new ColorArrayWrapper { colors = colors };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(initialPath, json);
        Debug.Log("[ParticlesHolder] Initial colors saved to: " + initialPath);
    }

    public void LoadInitialColors()
    {
        if (File.Exists(initialPath))
        {
            string json = File.ReadAllText(initialPath);
            ColorArrayWrapper wrapper = JsonUtility.FromJson<ColorArrayWrapper>(json);
            initialColors = wrapper.colors;
            Debug.Log("[ParticlesHolder] Initial colors loaded.");
        }
    }

    public ColorChanger[] colorChangers; // ← Inspector ですべての ColorChanger を登録

    public void ResetToInitialColors()
    {
        if (initialColors != null && initialColors.Length == colors.Length)
        {
            initialColors.CopyTo(colors, 0);
            Debug.Log("[ParticlesHolder] Colors reset to initial state.");

            // ColorChangerの表示も更新
            foreach (var changer in colorChangers)
            {
                if (changer != null)
                    changer.RefreshColorFromParticles();
            }
        }
        else
        {
            Debug.LogWarning("[ParticlesHolder] Cannot reset colors: initial data invalid.");
        }
    }
}
