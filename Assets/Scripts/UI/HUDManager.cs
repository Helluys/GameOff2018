using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    [SerializeField] private Image healthBar;
    [SerializeField] private Image coolDownBar;
    [SerializeField] private RawImage timotheTexture;

    [SerializeField] private Image ItemDisplay1;
    [SerializeField] private Image ItemDisplay2;

    [SerializeField] private Color hitEffectColor;
    [SerializeField] private float hitEffectTime = 1;
    private Color textureOriginalColor;
    private Coroutine cHitEffect;


    private void Start()
    {
        textureOriginalColor = timotheTexture.color;
    }

    public void SetItemDisplay(Item item,int index)
    {
        if (index == 0)
            ItemDisplay1.sprite = item.sprite;
        else
            ItemDisplay2.sprite = item.sprite;     
    }

    public void UpdateHealtBar(float value)
    {
        healthBar.fillAmount = value;
    }

    public void UpdateCoolDownBar(float value)
    {
        coolDownBar.fillAmount = value;
        if (value < 0.25)
            coolDownBar.color = Color.grey;
        else
            coolDownBar.color = Color.yellow;
    }

    public void HitEffect()
    {
        if (cHitEffect != null)
            StopCoroutine(cHitEffect);

        cHitEffect = StartCoroutine(EHitEffect());
    }

    private IEnumerator EHitEffect()
    {
        timotheTexture.color = hitEffectColor;
        float elapsedTime = 0;
        while(elapsedTime < hitEffectTime)
        {
            timotheTexture.color = Color.Lerp(hitEffectColor, textureOriginalColor, elapsedTime / hitEffectTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timotheTexture.color = textureOriginalColor;
    }
}
