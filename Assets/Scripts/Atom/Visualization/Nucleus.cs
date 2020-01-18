using System.Collections.Generic;

using UnityEngine;

using Utilities;

namespace Atom.Visualization
{
    public class Nucleus : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private List<GameObject> neutrons = new List<GameObject>();
        [SerializeField] private List<GameObject> protons = new List<GameObject>();

        public GameObject AddProton()
        {
            GameObject proton = protons.Find(p => !p.activeInHierarchy);

            if (proton)
            {
                proton.SetActive(true);
                proton.GetComponent<ParticleManager>().Activate();
                return proton;
            }
            return null;
        }

        public GameObject AddNeutron()
        {
            GameObject neutron = neutrons.Find(n => !n.activeInHierarchy);

            if (neutron)
            {
                neutron.SetActive(true);
                neutron.GetComponent<ParticleManager>().Activate();
                return neutron;
            }
            return null;
        }

        public GameObject SubtractNeutron()
        {
            GameObject neutron = neutrons.Find(n => n.activeInHierarchy);

            if (neutron)
            {
                neutron.SetActive(false);
                return neutron;
            }
            return null;
        }

        public GameObject SubtractProton()
        {
            GameObject proton = protons.Find(p => p.activeInHierarchy);

            if (proton)
            {
                proton.SetActive(false);
                return proton;
            }
            return null;
        }
    }
}
