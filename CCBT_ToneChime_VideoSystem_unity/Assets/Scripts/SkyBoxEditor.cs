using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxEditor : MonoBehaviour
{
    public Material Skybox;

    public float fadeSpeed = 2.0f; // 透明度の変化速度（1秒あたりの透明度変化量）
    private Coroutine fadeCoroutine; // 現在実行中のコルーチンの参照
    private bool isCurrentlyBright = true; // 初期状態を明転として設定

    void Start()
    {
        // 初期状態を明転に設定
        if (Skybox != null)
        {
            SetSkyboxBrightness(0.0f); // 明転状態に初期化（0が明転）
            isCurrentlyBright = true;  // フラグも明転状態に設定
        }
    }

    public void SetFadeDuration(float duration)
    {
        fadeSpeed = 1.0f / duration; // フェード時間から速度を計算
    }


    public void ToggleFade()
    {
        // 実行中のフェード処理があれば停止
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // フェードの方向を切り替え
        float targetAlpha = isCurrentlyBright ? 1.0f : 0.0f; // 明転なら暗転へ（1.0f）、暗転なら明転へ（0.0f）
        isCurrentlyBright = !isCurrentlyBright; // 状態を反転

        // 新しいフェード処理を開始
        fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        // 現在の明るさを取得
        float startAlpha = Skybox.GetFloat("_TotalBlightness");
        float alphaChange = Mathf.Abs(targetAlpha - startAlpha);
        float duration = alphaChange / fadeSpeed; // 透明度の変化速度に基づいて継続時間を計算

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            SetSkyboxBrightness(newAlpha);
            yield return null;
        }

        SetSkyboxBrightness(targetAlpha); // 最終目標値を設定
    }

    private void SetSkyboxBrightness(float brightness)
    {
        // Skybox の明るさを設定
        if (Skybox != null)
        {
            Skybox.SetFloat("_TotalBlightness", brightness);
        }
    }

    public void _SizeChangeSpeed(float value)
    {
        // Skybox のサイズ変更速度を調整
        if (Skybox != null)
        {
            Skybox.SetFloat("_SizeChangeSpeed", Mathf.Lerp(0, 10, value));
        }
    }
}
