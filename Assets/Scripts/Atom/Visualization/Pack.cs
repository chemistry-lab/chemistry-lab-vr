using UnityEngine;

namespace Atom.Visualization
{
    public class Pack : MonoBehaviour
    {
        [SerializeField] private int protons = 0;
        [SerializeField] private int neutrons = 0;
        [SerializeField] private int electrons = 0;
        [SerializeField] private string atomName = "";

        public int Protons { get { return protons; } }
        public int Neutrons { get { return neutrons; } }
        public int Electrons { get { return electrons; } }
        public string AtomName { get { return atomName; } }
    }
}
