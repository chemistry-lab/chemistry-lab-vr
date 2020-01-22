using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using Atom.Pack;
using Atom.Enum;

namespace Atom.Builder
{
    public class AtomBuilder : MonoBehaviour
    {
        [Header("Atoms")] [SerializeField] private List<AtomPack> atoms = new List<AtomPack>();

        [Header("Visualization")]
        [SerializeField] private TextMeshPro protonsLabel = null;
        [SerializeField] private TextMeshPro neutronsLabel = null;
        [SerializeField] private TextMeshPro electronsLabel = null;
        [SerializeField] private Visualization.Atom visualization = null;

        [Header("Validation")] [SerializeField]
        private Renderer validationLight = null;

        [SerializeField] private Renderer atomPreview = null;
        [SerializeField] private Transform spawnPoint = null;

        private List<AtomPart> contents = new List<AtomPart>();
        private Color invalidColor = new Color(0, 0, 0, 0.7f);
        private Color validColor = Color.white;
        private AtomPack current = null;

        private void Awake()
        {
            if (atomPreview != null)
            {
                validColor = atomPreview.material.color;
                atomPreview.material.color = invalidColor;
            }
        }

        public void AddProton()
        {
            int protons = GetCount(AtomPart.Proton);
            if (protons >= 8) return;

            if (visualization != null) visualization.AddProton();
            if (protonsLabel != null) protonsLabel.text = $"Protons: {protons + 1}";
            contents.Add(AtomPart.Proton);

            Validate();
        }

        public void AddNeutron()
        {
            int neutrons = GetCount(AtomPart.Neutron);
            if (neutrons >= 8) return;

            if (visualization != null) visualization.AddNeutron();
            if (neutronsLabel != null) neutronsLabel.text = $"Neutrons: {neutrons + 1}";
            contents.Add(AtomPart.Neutron);

            Validate();
        }

        public void AddElectron()
        {
            int electrons = GetCount(AtomPart.Electron);
            if (electrons >= 8) return;

            if (visualization != null) visualization.AddElectron(electrons);
            if (electronsLabel != null) electronsLabel.text = $"Electrons: {electrons + 1}";
            contents.Add(AtomPart.Electron);

            Validate();
        }

        public void Undo()
        {
            if (contents.Count == 0) return;

            AtomPart last = contents[contents.Count - 1];
            if (last == AtomPart.Proton) SubtractProton();
            else if (last == AtomPart.Neutron) SubtractNeutron();
            else SubtractElectron();

            Validate();
        }

        public bool CreateAtom()
        {
            if (current == null) return false;

            Instantiate(current.gameObject, spawnPoint.position, Quaternion.identity);

            return true;
        }

        public int GetCount(AtomPart part)
        {
            if (contents == null || contents.Count == 0) return 0;
            return contents.FindAll(p => p == part).Count;
        }

        private void SubtractProton()
        {
            int protons = GetCount(AtomPart.Proton);
            if (protons == 0) return;

            if (visualization != null) visualization.SubtractProton();
            if (protonsLabel != null) protonsLabel.text = $"Protons: {protons - 1}";
            contents.RemoveAt(contents.Count - 1);
        }

        private void SubtractNeutron()
        {
            int neutrons = GetCount(AtomPart.Neutron);
            if (neutrons == 0) return;

            if (visualization != null) visualization.SubtractNeutron();
            if (neutronsLabel != null) neutronsLabel.text = $"Neutrons: {neutrons - 1}";
            contents.RemoveAt(contents.Count - 1);
        }

        private void SubtractElectron()
        {
            int electrons = GetCount(AtomPart.Electron);
            if (electrons == 0) return;

            if (visualization != null) visualization.SubtractElectron(electrons);
            if (electronsLabel != null) electronsLabel.text = $"Electrons: {electrons - 1}";
            contents.RemoveAt(contents.Count - 1);
        }

        private void Validate()
        {
            if (atomPreview == null || validationLight == null || spawnPoint == null) return;

            foreach (AtomPack atom in atoms)
            {
                if (atom.Neutrons == GetCount(AtomPart.Neutron) && atom.Protons == GetCount(AtomPart.Proton) &&
                    atom.Electrons == GetCount(AtomPart.Electron))
                {
                    validationLight.material.SetColor("_EmissionColor", Color.green);
                    atomPreview.material.color = validColor;
                    atomPreview.material.SetTexture("_MainTex",
                        atom.gameObject.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex"));
                    current = atom;
                    return;
                }
            }

            atomPreview.material.color = invalidColor;
            validationLight.material.SetColor("_EmissionColor", Color.red);
            current = null;
        }


        public void ResetAtom()
        {
            int amountOfElectron = GetCount(AtomPart.Electron);
            int amountOfProton = GetCount(AtomPart.Electron);
            int amountOfNeutron = GetCount(AtomPart.Electron);
            StartCoroutine(SubtractAllAtomicParts(amountOfElectron, amountOfProton, amountOfNeutron));
        }

        private IEnumerator SubtractAllAtomicParts(int electrons, int protons, int neutrons)
        {
            for (int i = 0; i < electrons; i++)
            {
                SubtractElectron();
            }
            for (int i = 0; i < protons; i++)
            {
                SubtractProton();
            }
            for (int i = 0; i < neutrons; i++)
            { 
                SubtractNeutron();
            }

            return null;
        }
    }
}
