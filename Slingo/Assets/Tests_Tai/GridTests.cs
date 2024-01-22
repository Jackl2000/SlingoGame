using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
    public void TestGridClearing()
    {
        GameObject go = new GameObject();
        GridGeneration grid = go.AddComponent<GridGeneration>();
        go.AddComponent<GridCheck>();

        for (int i = 0; i < 10; i++)
        {
            GameObject newGameObject = new GameObject();
            grid.numberPositions.Add(i, new GridNumbers(i, i, newGameObject));
            grid.numberPositions[i].Hit(false);
        }

        Assert.AreEqual(10, grid.numberPositions.Count);

        grid.ReGenerateGrid();

        Assert.AreEqual(0, grid.numberPositions.Count);
    }

    [Test]
    public void TestGenerateNumberUniqueRangeOne()
    {
        Type type = typeof(GridGeneration);
        var grid = Activator.CreateInstance(type);
        MethodInfo method = type.GetMethod("GenerateNumber", BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters =
        {
            1,
            new List<int>(){5, 12, 3, 4, 11, 7, 8, 6, 9, 15, 13, 14, 10, 1}
        };
        
        int result = (int)method.Invoke(grid, parameters);
        Assert.AreEqual(2, result);
    }
}
