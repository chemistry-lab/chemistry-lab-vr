using UnityEngine;

using Molecule.Builder;
using Atom.Pack;

namespace Molecule.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class MoleculeIntake : MonoBehaviour
    {
        [Header("Build")]
        [SerializeField] private MoleculeBuilder builder = null;

        [Header("Cleanup")]
        [SerializeField] private float destroyTimeout = 1.0f;

        private void OnTriggerEnter(Collider other)
        {
            AtomPack atom = other.GetComponent<AtomPack>();
            if (atom)
            {
                builder.AddAtom(atom);
                Destroy(atom.gameObject, destroyTimeout);
            }
        }
    }
}
