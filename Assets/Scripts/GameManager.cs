using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Text inventoryText;
    

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
    public Image eventImage;

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
        MemberActingMemo();

        memberActingMemoPopup.SetActive(false);
        eventPopup.SetActive(false);
        actingButton.onClick.AddListener(MemberActingMemoPopup);
        nextDayButton.onClick.AddListener(NextDay);
        eventCloseButton.onClick.AddListener(EventCloseButton);
    }

    void UpdateUI()
    {
        dayText.text = $"Day {currentDay}";

        inventoryText.text = $"사과  : {food}개       " +
            $"믿음의 약  : {medicine}개       " +
                             $"양초  : {candle}개";



        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberStatusText[i] != null)
            {
                GroupMemberSO member = groupMembers[i];

                memberStatusText[i].text =
                    $"{member.memberName} \n" +
                    $"허기 : {memberHunger[i]} \n" +
                    $"믿음 : {memberTrust[i]} \n" +
                    $"멘탈 : {memberMental[i]} \n";

            }

        }

    }

    public void MemberActingMemoPopup()
    {
        memberActingMemoPopup.SetActive(true);
    }

    public void MemberActingMemo()
    {
        //memberActingMemoPopup.SetActive(true);


        for (int i = 0; i < feedButton.Length && i < groupMembers.Length; i++)
        {
            int memberIndex = i;
            feedButton[i].onClick.RemoveAllListeners();
            feedButton[i].onClick.AddListener(() => UseFoodItem(memberIndex));
        }
        for (int i = 0; i < medicineButton.Length && i < groupMembers.Length; i++)
        {
            int memberIndex = i;
            medicineButton[i].onClick.RemoveAllListeners();
            medicineButton[i].onClick.AddListener(() => UseMedicineItem(memberIndex));
        }
        for (int i = 0; i < lightButton.Length && i < groupMembers.Length; i++)
        {
            int memberIndex = i;
            lightButton[i].onClick.RemoveAllListeners();
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

        Debug.Log($"[{member.memberName}] 믿음: {memberTrust[memberIndex]}, 허기: {memberHunger[memberIndex]}, 멘탈: {memberMental[memberIndex]}");

    }

    public void UseFoodItem(int memberIndex)
    {
        Debug.Log("버튼 클릭 됨");
        if (food <= 0 || foodItem == null)
        {
            Debug.LogWarning("음식 없음");
            return;
        }
        if (memberHunger[memberIndex] <= 0)
        {
            Debug.LogWarning("이미 손쓸 수 없음 죽음을 받아드려야함 살 수 있는 방법 같은 건 없음 언제 올지 모를 공포에 덜덜 떨 수 밖에 없음 미리 제사상에 뭐 올리고 싶은지 남아있는 힘으로 유언이라도 작성하는 걸 추천함");
            return;
        }

        food--;
        ApplyItemEffect(memberIndex, foodItem);
        UpdateUI();

        Debug.Log("음식 사용 성공적 문제 없음");

    }

    public void UseMedicineItem(int memberIndex)
    {
        if (medicine <= 0 || medicineItem == null) return;
        if (memberTrust[memberIndex] <= 0) return;

        medicine--;
        ApplyItemEffect(memberIndex, medicineItem);
        UpdateUI();
    }

    public void UseCandleItem(int memberIndex)
    {
        if (candle <= 0 || candleItem == null) return;
        if (memberMental[memberIndex] <= 0) return;

        candle--;
        ApplyItemEffect(memberIndex, candleItem);
        UpdateUI();
    }

    public void NextDay()
    {
        currentDay += 1;
        eventPopup.SetActive(true);
        memberActingMemoPopup.SetActive(false);
        ProcessDailyChange();
        CheckRandomEvent();
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
        int baseHungerLoss = 5;
        int baseMentalLoss = 5;

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

        if (eventImage != null && eventSO.image != null)
        {
            eventImage.sprite = eventSO.image;
            eventImage.gameObject.SetActive(true);
        }
        else if (eventImage == null)
        {
            eventImage.gameObject.SetActive(false);
        }

        ApplyEventEffect(eventSO);

        nextDayButton.interactable = false;
        actingButton.interactable = false;


    }
    void CheckRandomEvent()
    {
        int totalProbability = 0;

        //전체 확률 합 구하기
        for(int i = 0; i < events.Length; i++)
        {
            totalProbability += events[i].probability;
        }

        if (totalProbability == 0)
            return;

        int roll = Random.Range(1, totalProbability + 1 + 50);
        int cumualtive = 0;

        for(int i = 0; i < events.Length ; i++)
        {
            cumualtive += events[i].probability;
            if (roll <= cumualtive)
            {
                ShowEventPopup(events[i]);
                return;
            }
        }
    }

    void EventCloseButton()
    {
        eventPopup.SetActive(false);
        nextDayButton.interactable = true;
        actingButton.interactable = true;
        UpdateUI();
    }

   void NormalEnding()
    {
        //멤버가 하나라도 남아있는 상황에서
        //일정 날짜가 지나면 클리어 이벤트가 발생
        //이벤트 발생 시 노멀엔딩으로 끌려감
    }

    void BadEnding(GroupMemberSO groupMember)
    {
        //이건 아닌데 그 멤버들의 믿음이 모두 0되면 그대로 배드엔딩씬에 끌고가야함.
        if (groupMember.maxTrust == 0)
        {
            SceneManager.LoadScene("EndScene_Bad");
        }
    }
}
