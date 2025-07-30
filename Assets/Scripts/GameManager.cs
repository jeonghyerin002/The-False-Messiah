using System.Collections;
using System.Collections.Generic;
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
    public Button medicineButton;             // 정신개조약
    public Button feedButton;
    public Button lightButton;
    public GameObject actingMemoPopup;

    [Header("게임 상태")]
    public int food = 20;
    public int medicine = 10;
    public int candle = 15;

    int currentDay;

    private int[] memberTrust;
    private int[] memberHunger;
    private int[] memberMental;

    void Start()
    {
        currentDay = 1;

        InitializeGroup();
        UpdateUI();

        actingMemoPopup.SetActive(false);
        actingButton.onClick.AddListener(ActingMemo);
        nextDayButton.onClick.AddListener(NextDay);
    }

    void UpdateUI()
    {
        dayText.text = $"Day {currentDay}";

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
    public void ActingMemo()
    {
        actingMemoPopup.SetActive (true);

        

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

        food--;
    }

    public void UseMedicineItem()
    {

    }

    public void UseCandleItem()
    {

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

            memberTrust[i] = Mathf.RoundToInt(doubt * trustMultipilier);
            memberHunger[i] = Mathf.RoundToInt(baseHungerLoss * hungerMultipilier);
            memberMental[i] = Mathf.RoundToInt(baseMentalLoss * member.lightEfficiency);

            if (memberHunger[i] <= 0) memberTrust[i] -= 10;
            if (memberHunger[i] <= 0) memberHunger[i] -= 15;
            if (memberMental[i] <= 0) memberTrust[i] -= 20;

            memberTrust[i] = Mathf.Max(0, memberTrust[i]);
            memberHunger[i] = Mathf.Max(0, memberHunger[i]);
            memberMental[i] = Mathf.Max(0, memberMental[i]);
        }

    }
}
