using System.Collections.Generic;

using UnityEngine;

using Utilities;

namespace Atom.Visualization
{
    public class Orbital : MonoBehaviour
    {
        [Header("Effects")]
        [SerializeField] private bool rotate = true;

        [Header("Content")]
        [SerializeField] private List<GameObject> electrons = new List<GameObject>();

        private void Update()
        {
            if (rotate) transform.Rotate(Time.deltaTime * 100 * Vector3.forward, Space.Self);
        }

        public GameObject AddElectron()
        {
            GameObject electron = electrons.Find(e => !e.activeInHierarchy);

            if (electron)
            {
                electron.SetActive(true);
                electron.GetComponent<ParticleManager>().Activate();
                return electron;
            }
            return null;
        }

        public GameObject SubtractElectron()
        {
            GameObject electron = electrons.Find(e => e.activeInHierarchy);

            if (electron)
            {
                electron.SetActive(false);
                return electron;
            }
            return null;
        }

        public void Reset()
        {
            foreach (var electron in electrons)
            {
                electron.SetActive(false);
            }
        }
    }
}
