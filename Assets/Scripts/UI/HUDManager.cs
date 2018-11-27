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

    private Color coolDownOriginalColor;
    private Color textureOriginalColor;
    private Coroutine cHitEffect;


    private void Start()
    {
        textureOriginalColor = timotheTexture.color;
        coolDownOriginalColor = coolDownBar.color;
    }

    public void SetItemDisplay(Item item,int index)
    {
        if (index == 0)
        {
            ItemDisplay1.enabled = true;
            ItemDisplay1.sprite = item.sprite;
            ItemDisplay1.color = item.color;
            if (item.type == ItemType.Undefined)
                ItemDisplay1.enabled = false;
        }
        else
        {
            ItemDisplay2.enabled = true;
            ItemDisplay2.sprite = item.sprite;
            ItemDisplay2.color = item.color;
            if (item.type == ItemType.Undefined)
                ItemDisplay2.enabled = false;
        }       
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
            coolDownBar.color = coolDownOriginalColor;
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
