using UnityEngine;

using Utilities;

namespace Tutorial
{
    public class WalkGoal : MonoBehaviour
    {
        [SerializeField] private SceneLoader loader = null;
        [SerializeField] private UnityScene scene = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") loader.LoadScene(scene);
        }
    }
}
