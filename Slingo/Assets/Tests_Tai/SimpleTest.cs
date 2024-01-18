using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
            spinTestGO.AddComponent<spin>();

            //Act

            //spinScript.Spin();

            //Assert
            Assert.IsNotNull(spinTestGO.GetComponent<spin>());

            //Assert.GreaterOrEqual(0, 0);

        }
    }
}

