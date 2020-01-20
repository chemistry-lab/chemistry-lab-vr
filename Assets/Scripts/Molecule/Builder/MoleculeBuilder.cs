using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using TMPro;

using Molecule.Pack;
using Atom.Pack;

namespace Molecule.Builder
{
    public class MoleculeBuilder : MonoBehaviour
    {
        [Header("Molecules")]
        [SerializeField] private List<MoleculePack> molecules = new List<MoleculePack>();

        [Header("Visualization")]
        [SerializeField] private string defaultText = "";
        [SerializeField] private TextMeshPro screen = null;

        [Header("Spawn")]
        [SerializeField] private Transform spawnPoint = null;

        private List<AtomPack> contents = new List<AtomPack>();

        public void AddAtom(AtomPack atom)
        {
            contents.Add(new AtomPack(atom));
            RefreshScreen();
        }

        public void Reset()
        {
            contents.Clear();
            RefreshScreen();
        }

        public void CreateMolecule()
        {
            if (contents.Count == 0) return;

            foreach (MoleculePack molecule in molecules)
            {
                if (molecule.IsEqual(contents))
                {
                    Instantiate(molecule, spawnPoint.position, Quaternion.identity);
                    Reset();
                    break;
                }
            }
        }

        public void RefreshScreen()
        {
            if (contents.Count == 0)
            {
                screen.text = defaultText;
                return;
            }

            IEnumerable<IGrouping<string, AtomPack>> groups = contents.GroupBy(c => c.AtomName);
            screen.text = string.Join("\n", groups.Select(g => $"{g.Key}: {g.Count()}"));
        }
    }
}
