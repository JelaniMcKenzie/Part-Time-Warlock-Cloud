using UnityEngine;

namespace Nevelson.Topdown2DPitfall.Assets.Scripts.Utils {
    public static class ExtTransform {
        /// <summary>
        /// Destroys all children of supplied GO transform, by default destroys after 1 frame delay
        /// </summary>
        /// <param name="t"></param>
        /// <param name="destroyImmediately"></param>
        public static void DestroyChildren(this Transform t, bool destroyImmediately = false) {
            foreach (Transform child in t) {
                if (destroyImmediately) MonoBehaviour.DestroyImmediate(child.gameObject);
                else MonoBehaviour.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Gets the local SCALE of the gameobject in 2D.  The Zed axis is assumed to be 1.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 GetLocalScale2D(this Transform t) {
            return new Vector3(t.localScale.x, t.localScale.y, 1);
        }

        /// <summary>
        /// Sets the local SCALE of the gameobject in 2D.  The Zed axis is assumed to be 1.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="localScale2D"></param>
        public static void SetLocalScale2D(this Transform t, Vector2 localScale2D) {
            t.localScale = new Vector3(localScale2D.x, localScale2D.y, 1);
        }

        /// <summary>
        /// Gets the world POSITION of the gameobject in 2D.  The Zed axis is assumed to be 0.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 GetPosition2D(this Transform t) {
            return t.position;
        }

        /// <summary>
        /// Sets the world POSITION of the gameobject in 2D.  The Zed axis is assumed to be 0.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="position2D"></param>
        public static void SetPosition2D(this Transform t, Vector2 position2D) {
            t.position = new Vector3(position2D.x, position2D.y, 0);
        }

        /// <summary>
        /// Gets the local POSITION of the gameobject in 2D.  The Zed axis is assumed to be 0.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 GetLocalPosition2D(this Transform t) {
            return t.localPosition;
        }

        /// <summary>
        /// Sets the local POSITION of the gameobject in 2D.  The Zed axis is assumed to be 0.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="localPosition2D"></param>
        public static void SetLocalPosition2D(this Transform t, Vector2 localPosition2D) {
            t.localPosition = new Vector3(localPosition2D.x, localPosition2D.y, 0);
        }
    }
}