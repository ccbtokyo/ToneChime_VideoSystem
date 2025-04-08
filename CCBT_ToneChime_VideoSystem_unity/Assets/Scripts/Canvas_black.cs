using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_black : MonoBehaviour
{
    public Image targetImage;
    public Material particleMaterial;
    public Text buttonText_particle, buttonText_skybox, speedSliderText;
    public Slider speedSlider;

    float fadeDuration = 2.0f; // フェード時間
    private Coroutine fadeCoroutine_particle; // Particle用のフェードコルーチン
    private Coroutine fadeCoroutine_skybox;   // Skybox用のフェードコルーチン
    private bool isParticleTransparent = false; // Particleの初期状態（見える状態：false）
    private bool isSkyboxTransparent = false;   // Skyboxの初期状態（明転状態：false）

    
    private void Start()
    {
        // 初期化
        if (particleMaterial != null)
        {
            SetParticleAlpha(0.2f); // Particleを初期状態で「見える」ように設定
        }
    }

    void Update()
    {
        // フェード時間をスライダーから反映
        fadeDuration = speedSlider.value;
        speedSliderText.text = fadeDuration.ToString("F1") + "秒";

        // ボタンテキストの更新
        buttonText_particle.text = isParticleTransparent ? "表示する・ライト" : "非表示にする・ライト";
        buttonText_skybox.text = isSkyboxTransparent ? "表示する・背景" : "非表示にする・背景";
    }

    public void FadeParticlesButton()
    {
        if (fadeCoroutine_particle != null)
        {
            StopCoroutine(fadeCoroutine_particle);
        }

        // 次の目標透明度を設定
        float targetAlpha = isParticleTransparent ? 0.2f : 0.0f;
        isParticleTransparent = !isParticleTransparent;

        // フェード処理を開始
        fadeCoroutine_particle = StartCoroutine(FadeParticleMaterial(targetAlpha));
    }

    IEnumerator FadeParticleMaterial(float targetAlpha)
    {
        if (particleMaterial == null)
            yield break;

        float startAlpha = particleMaterial.color.a;

        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetParticleAlpha(newAlpha);
            yield return null;
        }
        SetParticleAlpha(targetAlpha);
    }

    private void SetParticleAlpha(float alpha)
    {
        if (particleMaterial != null)
        {
            Color color = particleMaterial.color;
            color.a = alpha;
            particleMaterial.color = color;
        }
    }

    public void FadeSkybox(SkyBoxEditor skyboxEditor)
    {
        if (skyboxEditor != null)
        {
            skyboxEditor.SetFadeDuration(fadeDuration); // フェード時間をSkyBoxに反映
            skyboxEditor.ToggleFade(); // SkyBoxのフェード切り替え

            // Skyboxの透明状態を更新
            isSkyboxTransparent = !isSkyboxTransparent;
        }
    }
}
