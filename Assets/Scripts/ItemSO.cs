using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Survival Game/Item")]
public class ItemSO : ScriptableObject
{
    [Header("기본 정보")]
    public string ItemName = "아이템";
    public Sprite ItemIcon;
    public ItemType itemType = ItemType.food;
    
    [Header("아이템 효과")]
    public int foodEffect = 0;
    public int candleEffect = 0;
    public int medicineEffect = 0;

    public enum ItemType
    {
        food,
        medicine,
        candle
    }
}
