using UnityEngine;

using Molecule.Pack;

namespace Environment.Props.Pyrolysis
{
    [RequireComponent(typeof(Collider))]
    public class PyrolysisIntake : MonoBehaviour
    {
        [Header("Machine")]
        [SerializeField] private PyrolysisMachine machine = null;

        [Header("Cleanup")]
        [SerializeField] private float destroyTimeout = 1.0f;

        private void OnTriggerEnter(Collider other)
        {
            PyrolysisPlastic plastic = other.GetComponent<PyrolysisPlastic>();
            if (plastic != null && plastic.AmountOfPlastic >= 1)
            {
                machine.AddPlastic();
                Destroy(plastic.gameObject, destroyTimeout);
                return;
            }

            MoleculePack molecule = other.GetComponent<MoleculePack>();
            if (molecule != null && molecule.MoleculeAbbreviation == "CH4")
            {
                machine.AddMethane();
                Destroy(molecule.gameObject, destroyTimeout);
                return;
            }
        }
    }
}
