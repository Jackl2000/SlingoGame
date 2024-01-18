using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UITests
{
    [Test]
    public void TestUIManagerMoneyDisplay()
    {
        float value = 100.05f;
        string expected = value.ToString("n2") + " kr";
        string actual = UIManager.Instance.DisplayMoney(value);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestUpdateRewardsFirstValue()
    {
        int multipliere = 5;
        GameObject go = new GameObject();
        GridCheck gridCheck = go.AddComponent<GridCheck>();
        gridCheck.UpdateRewards(multipliere);
        float expected = 10 * multipliere;

        Assert.AreEqual(expected, gridCheck.rewards[3]);
    }

    [Test]
    public void TestUpdateRewardsListCount()
    {
        GameObject go = new GameObject();
        go.AddComponent<GridCheck>();
        int actual = go.GetComponent<GridCheck>().rewards.Count;

        Assert.AreEqual(10, actual);
    }
}
