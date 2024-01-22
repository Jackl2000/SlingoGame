using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
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
    public void TestUIManagerGetMoneyValue()
    {
        string value = "195,45 kr";
        float expected = 195.45f;
        float actual = UIManager.Instance.GetMoneyValue(value);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestUpdateRewardsFirstValue()
    {
        int multipliere = 5;
        GameObject go = new GameObject();
        GridCheck gridCheck = go.AddComponent<GridCheck>();
        float expected = 10 * multipliere;

        gridCheck.UpdateRewards(multipliere);

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

    [Test]
    public void TestSoundUISwitchOff()
    {
        GameObject go = new GameObject();
        SettingsMenu settings = go.AddComponent<SettingsMenu>();
        TextMeshProUGUI soundText = go.AddComponent<TextMeshProUGUI>();
        soundText.text = "Sounds: On";
        settings.soundText = soundText;
        string expected = "Sounds: Off";

        settings.Sound();

        Assert.AreEqual(expected, settings.soundText.text);
    }

    [Test]
    public void TestSoundUISwitchOn()
    {
        GameObject go = new GameObject();
        SettingsMenu settings = go.AddComponent<SettingsMenu>();
        TextMeshProUGUI soundText = go.AddComponent<TextMeshProUGUI>();
        soundText.text = "Sounds: Off";
        settings.soundText = soundText;

        string expected = "Sounds: On";

        settings.Sound();

        Assert.AreEqual(expected, settings.soundText.text);
    }

    [Test]
    public void TestSettingsViewer()
    {
        GameObject go = new GameObject();
        SettingsMenu settings = go.AddComponent<SettingsMenu>();
        GameObject settingsPanel = new GameObject();
        settings.settingsMenuPanel = settingsPanel;
        GameObject betsPanel = new GameObject();
        settings.spinsBetPanel = betsPanel;
        bool expected = true;
        bool expected2 = false;

        settings.settingsMenuPanel.SetActive(false);
        settings.ViewSettingsPanel();

        Assert.AreEqual(expected, settings.settingsMenuPanel.activeSelf);
        Assert.AreEqual(expected2, settings.spinsBetPanel.activeSelf);
    }

    [Test]
    public void TestSpinBetsViewer()
    {
        GameObject go = new GameObject();
        SpinsValue bets = go.AddComponent<SpinsValue>();
        go.AddComponent<spin>();
        GameObject betsPanel = new GameObject();
        bets.spinsBetPanel = betsPanel;
        GameObject settingsPanel = new GameObject();
        bets.settingsPanel = settingsPanel;
        bool expected = true;
        bool expected2 = false;
        
        bets.spinsBetPanel.SetActive(false);
        bets.ViewSpinsBets();

        Assert.AreEqual(expected, bets.spinsBetPanel.activeSelf);
        Assert.AreEqual(expected2, bets.settingsPanel.activeSelf);
    }

    [Test]
    public void TestSetSpinBets()
    {
        GameObject go = new GameObject();
        spin spin = go.AddComponent<spin>();
        go.AddComponent<GridCheck>();
        go.AddComponent<GridGeneration>();
        SpinsValue spinValue = go.AddComponent<SpinsValue>();
        GameObject betsPanel = new GameObject();
        spinValue.spinsBetPanel = betsPanel;
        float bet = 10f;

        spinValue.SetSpinBets(bet);

        Assert.AreEqual(10, spin.spinBets);
    }
}
