using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectReward : MonoBehaviour
{
    [Header("References")]
    public GridGeneration gridGeneration;
    public GridCheck gridCheck;
    public PlayerData playerData;
    public spin spin;

    public void Collect()
    {
        playerData.balance += gridCheck.rewards[gridCheck.slingoCount];
        gridGeneration.ReGenerateGrid();
        spin.spinLeft = 10;
        spin.spinPrice = 0;
    }

}
