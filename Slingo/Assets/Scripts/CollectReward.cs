using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectReward : MonoBehaviour
{
    public GridCheck gridCheck;
    public PlayerData playerData;

    public void Collect()
    {
        playerData.balance = gridCheck.rewards[gridCheck.slingoCount];
    }

}
