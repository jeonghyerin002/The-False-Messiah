using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Survival Game/Item")]
public class ItemSO : ScriptableObject
{
    [Header("�⺻ ����")]
    public string ItemName = "������";
    public Sprite ItemIcon;
    public ItemType itemType = ItemType.food;
    
    [Header("������ ȿ��")]
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
