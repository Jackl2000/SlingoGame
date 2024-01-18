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
}
