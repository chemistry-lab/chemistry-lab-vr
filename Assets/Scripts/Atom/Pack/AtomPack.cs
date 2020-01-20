using UnityEngine;

namespace Atom.Pack
{
    public class AtomPack : MonoBehaviour
    {
        [SerializeField] private int protons = 0;
        [SerializeField] private int neutrons = 0;
        [SerializeField] private int electrons = 0;
        [SerializeField] private string atomName = "";

        public AtomPack(AtomPack atom)
        {
            protons = atom.protons;
            neutrons = atom.neutrons;
            electrons = atom.electrons;
            atomName = atom.atomName;
        }

        public int Protons { get { return protons; } }
        public int Neutrons { get { return neutrons; } }
        public int Electrons { get { return electrons; } }
        public string AtomName { get { return atomName; } }
    }
}
