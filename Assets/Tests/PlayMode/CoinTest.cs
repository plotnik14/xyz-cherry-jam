using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class CoinTest
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CoinTestWithEnumeratorPasses()
        {
            SceneManager.LoadScene("SceneForTesting");

            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            var test = new MonoBehaviourTest<CoinTestBehaviour>();
            yield return test;

            var coin = GameObject.FindWithTag("SilverCoin");

            Assert.IsTrue(coin == null, "coin is still here!");
            LogAssert.Expect(LogType.Log, "Coins count:1");
        }
    }
}
