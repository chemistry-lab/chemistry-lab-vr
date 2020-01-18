using UnityEngine;

using Utilities;

namespace Tutorial
{
    public class GrabGoal : MonoBehaviour
    {
        [SerializeField] private SceneLoader loader = null;
        [SerializeField] private UnityScene scene = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Grabbable") loader.LoadScene(scene);
        }
    }
}
