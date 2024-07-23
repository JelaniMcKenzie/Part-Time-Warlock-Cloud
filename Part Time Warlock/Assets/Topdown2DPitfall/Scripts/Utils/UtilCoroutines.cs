using System;
using System.Collections;
using UnityEngine;

namespace Nevelson.Topdown2DPitfall.Assets.Scripts.Utils {
    public static class UtilCoroutines {
        public static IEnumerator FallingCo(GameObject fallingObj, Action ActionsAfterPitfall,
            float pitfallAnimSpeed, Vector2 respawnLocation) {
            Vector2 scaleReduction = new Vector2(pitfallAnimSpeed, pitfallAnimSpeed);
            while (fallingObj.transform.GetLocalScale2D().x > 0) {
                fallingObj.transform.SetLocalScale2D(fallingObj.transform.GetLocalScale2D() - scaleReduction);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(.1f);
            ActionsAfterPitfall();
            fallingObj.transform.SetPosition2D(respawnLocation);
            yield return new WaitForSecondsRealtime(.1f);
            fallingObj.transform.SetLocalScale2D(Vector2.one);
        }
    }
}