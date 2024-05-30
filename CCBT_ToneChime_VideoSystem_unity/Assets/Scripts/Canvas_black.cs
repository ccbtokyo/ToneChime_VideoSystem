using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_black : MonoBehaviour
{
    public Image targetImage; // 透明度を変更するImageコンポーネント
    float fadeDuration = 2.0f; // 透明度が完全に変わるまでの時間（秒）

    private Coroutine fadeCoroutine; // 現在実行中のコルーチンの参照
    private bool isFadingToTransparent = false; // 現在フェードアウトしているかどうかのフラグ

    public Text buttonText, speedSliderText;
    public Slider speedSlider;

    private void Start()
    {
        targetImage = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if(targetAlpha == 1)
        {
            buttonText.text = "明転ボタン";
        }
        else
        {
            buttonText.text = "暗転ボタン";
        }

        fadeDuration = speedSlider.value;
        speedSliderText.text = speedSlider.value.ToString("F1") + "秒";
    }

    float targetAlpha;
    public void doFade()
    {
        // 実行中のフェード処理があれば停止
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // フェードの方向を切り替え
        isFadingToTransparent = !isFadingToTransparent;
        targetAlpha = isFadingToTransparent ? 1.0f : 0.0f;

        // 新しいフェード処理を開始
        fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = targetImage.color.a;
        float duration = fadeDuration; // フェードの持続時間

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, newAlpha);
            yield return null;
        }
        targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, targetAlpha);
    }
}
