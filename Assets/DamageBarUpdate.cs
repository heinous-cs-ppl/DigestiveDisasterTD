using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBarUpdate : MonoBehaviour
{
    [Header("Game Object references")]
    public Image icon;
    public Image text;
    public Image fill;
    [Header("Damage Stuff")]
    public Sprite damageIcon;
    public Sprite damageText;
    public Sprite damageFill;
    [Header("Heal Stuff")]
    public Sprite healIcon;
    public Sprite healText;
    public Sprite healFill;
    [Header("Purify Stuff")]
    public Sprite purifyIcon;
    public Sprite purifyText;
    public Sprite purifyFill;
    [Header("Slow Stuff")]
    public Sprite slowIcon;
    public Sprite slowText;
    public Sprite slowFill;

    public void SetDamage() {
        icon.sprite = damageIcon;
        text.sprite = damageText;
        fill.sprite = damageFill;
        text.SetNativeSize();
    }

    public void SetHeal() {
        icon.sprite = healIcon;
        text.sprite = healText;
        fill.sprite = healFill;
        text.SetNativeSize();
    }

    public void SetPurify() {
        icon.sprite = purifyIcon;
        text.sprite = purifyText;
        fill.sprite = purifyFill;
        text.SetNativeSize();
    }

    public void SetSlow() {
        icon.sprite = slowIcon;
        text.sprite = slowText;
        fill.sprite = slowFill;
        text.SetNativeSize();
    }
}
