using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public interface IFadeService
{
    void FadeIn(Image image, float duration);
    void FadeOut(Image image, float duration);
}

public class FadeService : IFadeService
{
    public void FadeIn(Image image, float duration)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.DOFade(1, duration);
    }

    public void FadeOut(Image image, float duration)
    {
        image.DOFade(0, duration);
    }
}