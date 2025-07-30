using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("멤버 템플릿")]
    public GroupMemberSO[] groupMembers;

    [Header("아이템 템플릿")]
    public ItemSO foodItem;
    public ItemSO medicineItem;
    public ItemSO candleItem;


    [Header("참조 UI")]
    public Text dayText;
    public Text[] memberStatusText;
    public Button nextDayButton;
    

    [Header("메모 UI")]
    public Button actingButton;
    public Button[] medicineButton;             // 정신개조약
    public Button[] feedButton;
    public Button[] lightButton;
    public GameObject memberActingMemoPopup;

    [Header("이벤트 시스템")]
    public EventSO[] events;
    public GameObject eventPopup;
    public Text eventTitleText;
    public Text eventDescriptionText;
    public Button eventCloseButton;

    [Header("게임 상태")]
    int currentDay;
    public int food = 20;
    public int medicine = 10;
    public int candle = 15;

    private int[] memberTrust;
    private int[] memberHunger;
    private int[] memberMental;

    void Start()
    {
        currentDay = 1;

        InitializeGroup();
        UpdateUI();

        memberActingMemoPopup.SetActive(false);
        eventPopup.SetActive(false);
        actingButton.onClick.AddListener(MemberActingMemo);
        nextDayButton.onClick.AddListener(NextDay);
    }

    void UpdateUI()
    {
        dayText.text = $"Day {currentDay}";

        memberActingMemoPopup.SetActive(false);

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberStatusText[i] != null)
            {
                GroupMemberSO member = groupMembers[i];

                memberStatusText[i].text =
                    $"{member.memberName} \n" +
                    $"믿음 : {memberTrust[i]} \n" +
                    $"허기 : {memberHunger[i]} \n" +
                    $"멘탈 : {memberMental[i]} \n";
            }
        }

    }
    public void MemberActingMemo()
    {
        memberActingMemoPopup.SetActive (true);

        for(int i = 0; i < feedButton.Length && i < groupMembers.Length; i++)
        {
            int memberIndex = i;
            feedButton[i].onClick.AddListener(() => UseFoodItem(memberIndex));
        }
        for (int i = 0; i < medicineButton.Length && i < groupMembers.Length; i++)
        {
            int memberIndex = i;
            medicineButton[i].onClick.AddListener(() => UseMedicineItem(memberIndex));
        }
        for (int i = 0; i < lightButton.Length && i < groupMembers.Length; i++)
        {
            int memberIndex = i;
            lightButton[i].onClick.AddListener(() => UseCandleItem(memberIndex));
        }



    }

    void ApplyItemEffect(int memberIndex, ItemSO item)
    {
        GroupMemberSO member = groupMembers[memberIndex];

        int actualTrust = Mathf.RoundToInt(item.medicineEffect * member.trustEfficiency);
        int actualHunger = Mathf.RoundToInt(item.foodEffect * member.foodEfficiency);
        int actualMental = item.candleEffect;

        memberTrust[memberIndex] += actualTrust;
        memberHunger[memberIndex] += actualHunger;
        memberMental[memberIndex] += actualMental;

        memberTrust[memberIndex] = Mathf.Min(memberTrust[memberIndex], member.maxTrust);
        memberHunger[memberIndex] = Mathf.Min(memberHunger[memberIndex], member.maxHunger);
        memberMental[memberIndex] = Mathf.Min(memberMental[memberIndex], member.maxMental);
    }
  
    public void UseFoodItem(int memberIndex)
    {
        if (food <= 0 || foodItem == null) return;
        if (memberHunger[memberIndex] <= 0) return;

        food--;
        ApplyItemEffect(memberIndex, foodItem);

    }

    public void UseMedicineItem(int memberIndex)
    {
        if (medicine <= 0 || medicineItem == null) return;
        if (memberTrust[memberIndex] <= 0) return;

        medicine--;
        ApplyItemEffect(memberIndex, medicineItem);
    }

    public void UseCandleItem(int memberIndex)
    {
        if (candle <= 0 || candleItem == null) return;
        if (memberMental[memberIndex] <= 0) return;

        candle--;
        ApplyItemEffect(memberIndex, candleItem);
    }

    public void NextDay()
    {
        currentDay += 1;
        ProcessDailyChange();
        UpdateUI();
    }
    void InitializeGroup()
    {
        int memberCount = groupMembers.Length;

        memberTrust = new int[memberCount];
        memberHunger = new int[memberCount];
        memberMental = new int[memberCount];

        for (int i = 0; i < memberCount; i++)
        {
            if (groupMembers[i] != null)
            {
                memberTrust[i] = groupMembers[i].maxTrust;
                memberHunger[i] = groupMembers[i].maxHunger;
                memberMental[i] = groupMembers[i].maxMental;
            }
        }
    }

    void ProcessDailyChange()
    {
        int doubt = 5;
        int baseHungerLoss = 15;
        int baseMentalLoss = 10;

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] == null) continue;

            GroupMemberSO member = groupMembers[i];

            float trustMultipilier = member.difficulty == GroupMemberSO.Difficulty.easy ? 0.5f : 1.0f;
            float hungerMultipilier = member.difficulty == GroupMemberSO.Difficulty.easy ? 0.8f : 1.0f;

            memberTrust[i] -= Mathf.RoundToInt(doubt * trustMultipilier);
            memberHunger[i] -= Mathf.RoundToInt(baseHungerLoss * hungerMultipilier);
            memberMental[i] -= Mathf.RoundToInt(baseMentalLoss * member.lightEfficiency);


            if (memberHunger[i] <= 0)
            {
                memberTrust[i] -= 10;
                memberHunger[i] -= 15;
            }

            if (memberMental[i] <= 0) memberTrust[i] -= 20;

            memberTrust[i] = Mathf.Max(0, memberTrust[i]);
            memberHunger[i] = Mathf.Max(0, memberHunger[i]);
            memberMental[i] = Mathf.Max(0, memberMental[i]);
        }

    }

    void ApplyEventEffect(EventSO eventSO)
    {
        food += eventSO.foodChange;
        medicine += eventSO.medicineChange;
        candle += eventSO.candleChange;

        food = Mathf.Max(0, food);
        medicine = Mathf.Max(0, medicine);
        candle = Mathf.Max(0, candle);

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberTrust[i] > 0)
            {
                memberTrust[i] += eventSO.trustChange;
                memberHunger[i] += eventSO.hungerChange;
                memberMental[i] += eventSO.mentalChange;

                GroupMemberSO member = groupMembers[i];
                memberTrust[i] = Mathf.Clamp(memberTrust[i], 0, member.maxTrust);
                memberHunger[i] = Mathf.Clamp(memberHunger[i], 0, member.maxHunger);
                memberMental[i] = Mathf.Clamp(memberMental[i], 0, member.maxMental);
            }
        }
    }

    void ShowEventPopup(EventSO eventSO)
    {
        eventPopup.SetActive(true);

        eventTitleText.text = eventSO.eventTitle;
        eventDescriptionText.text = eventSO.eventDescription;

        ApplyEventEffect(eventSO);

        nextDayButton.interactable = false;
    }

   
}
