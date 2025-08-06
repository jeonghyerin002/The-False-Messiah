using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
[CreateAssetMenu (fileName = "New GroupMember", menuName = "Survival Game/Group Member")]

public class GroupMemberSO : ScriptableObject
{
    [Header("정보")]
    public string memberName = "구성원";
    public Sprite protrait;
    public Difficulty difficulty = Difficulty.easy;

    [Header("기본 스텟")]
    [Range(0, 100)]
    public int maxTrust = 100;
    [Range(0, 100)]
    public int maxHunger = 100;
    [Range(0, 100)]
    public int maxMental = 100;

    [Header("특성")]
    [Range(0f, 30.0f)]
    public float trustEfficiency = 5.0f;
    //public float doubt = 5.0f;        //의심
    [Range(1.0f, 5.0f)]
    public float foodEfficiency = 1.0f;
    [Range(0.0f, 10.0f)]
    public float lightEfficiency = 1.0f;
    
    public enum Difficulty
    {
        easy,
        namal,
        hard
    }
}
