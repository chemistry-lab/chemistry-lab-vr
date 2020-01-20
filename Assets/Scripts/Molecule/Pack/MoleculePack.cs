using System.Collections.Generic;

using UnityEngine;

using Atom.Pack;

namespace Molecule.Pack
{
    public class MoleculePack : MonoBehaviour
    {
        [Header("Name")]
        [SerializeField] private string moleculeName = "";
        [SerializeField] private string moleculeAbbreviation = "";

        [Header("Content")]
        [SerializeField] private List<AtomPack> atoms = new List<AtomPack>();

        // IMPROVE
        // This function loops over all the atoms in the molecule
        // ignoring the ones that where already checked. This could
        // become a performance problem for very large molecules.
        public bool IsEqual(List<AtomPack> other)
        {
            if (atoms.Count != other.Count) return false;

            foreach (AtomPack atom in atoms)
            {
                int atomsCount = atoms.FindAll(a => a.AtomName == atom.AtomName).Count;
                int otherCount = other.FindAll(o => o.AtomName == atom.AtomName).Count;
                if (atomsCount != otherCount) return false;
            }

            return true;
        }

        public List<AtomPack> Atoms { get { return atoms; } }
        public string MoleculeName { get { return moleculeName; } }
        public string MoleculeAbbreviation { get { return moleculeAbbreviation; } }
    }
}
