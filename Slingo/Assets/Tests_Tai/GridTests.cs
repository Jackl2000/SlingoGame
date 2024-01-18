using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridTests
{
    [Test]
    public void TestGridNumberDiagonalFalse()
    {
        GameObject go = new GameObject();
        GridNumbers gridNumber = new GridNumbers(4, 1, go);
        Assert.AreEqual(false, gridNumber.diagonal);
    }

    [Test]
    public void TestGridNumberDiagonalTrue()
    {
        GameObject go = new GameObject();
        GridNumbers gridNumber = new GridNumbers(3, 3, go);
        Assert.AreEqual(true, gridNumber.diagonal);
    }

    [Test]
    public void TestGridNumberHit()
    {
        GameObject go = new GameObject();
        GridNumbers gridNumber = new GridNumbers(3, 3, go);
        gridNumber.Hit(false);
        Assert.AreEqual(true, gridNumber.hasBeenHit);
    }

    [Test]
    public void TestGridNumberReset()
    {
        GameObject go = new GameObject();
        GridNumbers gridNumber = new GridNumbers(3, 3, go);
        gridNumber.hasBeenHit = true;
        gridNumber.ResetData();
        Assert.AreEqual(false, gridNumber.hasBeenHit);
    }

    [Test]
    public void TestCheckMatchingNumb()
    {
        GameObject go = new GameObject();
        GameObject parent = new GameObject();
        parent.AddComponent<CollectReward>();
        go.transform.parent = parent.transform;
        GameObject child = new GameObject();
        child.AddComponent<Animator>();
        child.transform.parent = go.transform;
        GameObject childOffChild = new GameObject();
        childOffChild.AddComponent<Animator>();
        childOffChild.transform.parent = child.transform;
        child.AddComponent<spin>();
        spin spin = go.AddComponent<spin>();
        GameObject grid = new GameObject();
        grid.AddComponent<GridGeneration>();
        spin.gridGeneration = grid.GetComponent<GridGeneration>();
        GridNumbers number = new GridNumbers(1, 1, go);
        spin.gridGeneration.numberPositions.Add(1, number);
        spin.spinNumbers = new List<int>
        {
            1
        };
        
        spin.CheckMatchingNumb();
        Assert.IsTrue(number.hasBeenHit);
    }
}
