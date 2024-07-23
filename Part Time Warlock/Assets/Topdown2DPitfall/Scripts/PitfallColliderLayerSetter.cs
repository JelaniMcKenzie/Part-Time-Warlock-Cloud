using UnityEngine;
namespace Nevelson.Topdown2DPitfall.Assets.Scripts.Utils {
    public class PitfallColliderLayerSetter : MonoBehaviour {
        void Start() {
            if (LayerMask.GetMask(Constants.PITFALL_COLLIDER) == 0) {
                Debug.LogError("Add " + Constants.PITFALL_COLLIDER + " to " +
                    "Edit > Project Settings > Tags and Layers");
            }
            if (gameObject.layer != LayerMask.NameToLayer(Constants.PITFALL_COLLIDER))
                gameObject.layer = LayerMask.NameToLayer(Constants.PITFALL_COLLIDER);
        }
    }
}