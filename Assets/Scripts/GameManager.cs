using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("¸â¹ö ÅÙÇÃ¸´")]
    public GroupMemberSO[] groupMembers;

    private int[] memberTrust;
    private int[] memberHunger;
    private int[] memberMental;

    void Start()
    {

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
}
