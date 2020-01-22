using UnityEngine;

using Environment.Props.Pyrolysis;
using Utilities;

namespace Scenarios
{
    public class PyrolysisGoal : MonoBehaviour
    {
        [Header("Environment")]
        [SerializeField] private GameObject door = null;

        [Header("Scene")]
        [SerializeField] private SceneLoader loader = null;
        [SerializeField] private UnityScene scene = null;
        [SerializeField] private float timeout = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            PyrolysisJerrycan jerrycan = other.GetComponent<PyrolysisJerrycan>();
            if (jerrycan != null && jerrycan.AmountOfFuel > 0)
            {
                Destroy(door);
                loader.LoadScene(scene, timeout);
            }
        }
    }
}
