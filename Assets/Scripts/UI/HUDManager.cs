using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ItemSpriteDictionnaryElement
{
    public ItemType type;
    public Sprite sprite;
}

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

    [SerializeField] private ItemSpriteDictionnaryElement[] InspectorDictionnary;
    private Dictionary<ItemType, Sprite> itemsSpritesDictionnary;

    private void Start()
    {
        textureOriginalColor = timotheTexture.color;
        itemsSpritesDictionnary = new Dictionary<ItemType, Sprite>();
        for(int i = 0; i < InspectorDictionnary.Length; i++)
        {
            itemsSpritesDictionnary.Add(InspectorDictionnary[i].type, InspectorDictionnary[i].sprite);
        }
    }

    public void SetItemDisplay(ItemType type,int index)
    {
        Sprite sprite;
        if (itemsSpritesDictionnary.TryGetValue(type, out sprite))
        {
            if (index == 0)
                ItemDisplay1.sprite = sprite;
            else
                ItemDisplay2.sprite = sprite;
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
