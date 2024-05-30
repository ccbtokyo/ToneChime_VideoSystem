using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxEditor : MonoBehaviour
{
    public Material Skybox;

    public float fadeSpeed = 2.0f; // 透明度の変化速度（1秒あたりの透明度変化量）

    private Coroutine fadeCoroutine; // 現在実行中のコルーチンの参照
    private bool isFadingToTransparent = false;

    void Awake()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {

            // 実行中のフェード処理があれば停止
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            // フェードの方向を切り替え
            isFadingToTransparent = !isFadingToTransparent;
            float targetAlpha = isFadingToTransparent ? 0.0f : 1.0f;

            // 新しいフェード処理を開始
            fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
        }
    }

    public void _SizeChangeSpeed(float value)
    {
        //Skybox.si
        Skybox.SetFloat("_SizeChangeSpeed", value.Map(0,1,0,10));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = Skybox.GetFloat("_TotalBlightness");
        float alphaChange = Mathf.Abs(targetAlpha - startAlpha);
        float duration = alphaChange / fadeSpeed; // 透明度の変化速度に基づいて継続時間を計算

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            //targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, newAlpha);
            Skybox.SetFloat("_TotalBlightness", newAlpha);
            yield return null;
        }

        Skybox.SetFloat("_TotalBlightness", targetAlpha);

        //targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, targetAlpha);
    }

    //IEnumerator Fade_Skybox(float v)
    //{

    //    float time = 0;
    //    while (time < duration)
    //    {
    //        time += Time.deltaTime;
    //        Vector2 targetRange = Mathf.Lerp(new Vector2(1,1), initialSizeRange, time / duration);
    //        targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, newAlpha);
    //        yield return null;
    //    }

    //}
}
