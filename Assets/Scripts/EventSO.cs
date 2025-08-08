using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Survival Game/Event")]
public class EventSO : ScriptableObject
{
    [Header("�̺�Ʈ ����")]
    public string eventTitle = "�̺�Ʈ �߻�!";
    [TextArea(3, 5)]
    public string eventDescription = "���� ���� �Ͼ���ϴ�";
    public Sprite image;

    [Header("�ڿ� ��ȭ")]
    public int foodChange = 0;
    public int candleChange = 0;
    public int medicineChange = 0;

    [Header("��� ���� ��ȭ")]
    public int trustChange = 0;
    public int hungerChange = 0;
    public int mentalChange = 0;

    [Header("�߻� Ȯ��")]
    [Range(0, 100)]
    public int probability = 30; 
}
