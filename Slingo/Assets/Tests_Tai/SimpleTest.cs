using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests_Tai
{
    public class SimpleTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SimpleTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SimpleTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [Test]
        public void SpinTest()
        {
            //Arrange
            GameObject spinTestGO = new GameObject();

            GameObject slot1 = new GameObject();
            GameObject wildImg1 = new GameObject();
            GameObject text1 = new GameObject();
            wildImg1.AddComponent<Image>();
            text1.AddComponent<TextMeshProUGUI>();


            GameObject slot2 = new GameObject();
            GameObject wildImg2 = new GameObject();
            GameObject text2 = new GameObject();
            wildImg2.AddComponent<Image>();
            text2.AddComponent<TextMeshProUGUI>();

            slot1.transform.parent = spinTestGO.transform;
            slot2.transform.parent = spinTestGO.transform;

            wildImg1.transform.parent = slot1.transform;
            wildImg2.transform.parent = slot2.transform;
            
            text1.transform.parent = slot1.transform;
            text2.transform.parent = slot2.transform;


            spinTestGO.AddComponent<spin>();
            //spinTestGO.AddComponent<GridGeneration>();

            spin spinScript = spinTestGO.GetComponent<spin>();
            spinScript.spinNumbers = new List<int>() { 1, 22, 38, 55, 76};

            spinScript.slotsList = new List<GameObject> { slot1, slot2 };


            //Act
            spinScript.Spin();

            //Assert
            Assert.NotNull(spinScript.spinNumbers);


        }
    }
}

