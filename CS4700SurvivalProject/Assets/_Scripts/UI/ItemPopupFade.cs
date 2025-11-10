using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemPopupFade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image itemImage, backgroundImage;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private Ease fadeEase;
    [SerializeField] private float moveSpeed;
    void Start()
    {
        itemImage.DOFade(0f, lifetime).SetEase(fadeEase);
        backgroundImage.DOFade(0f, lifetime).SetEase(fadeEase);
        nameText.DOFade(0f, lifetime).SetEase(fadeEase);
        Destroy(gameObject, lifetime);
    }

    public void DisplayItem(ItemSO item)
    {
        nameText.text = "1x " + item.name;
        itemImage.sprite = item.worldSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.up * (Time.deltaTime * moveSpeed * 100);
    }
}
