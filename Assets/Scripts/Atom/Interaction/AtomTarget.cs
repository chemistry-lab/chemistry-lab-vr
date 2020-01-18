using System.Text.RegularExpressions;

using UnityEngine;

using Atom.Builder;
using Atom.Enum;

namespace Experiments.Atom.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class AtomTarget : MonoBehaviour
    {
        [Header("Build")]
        [SerializeField] private AtomBuilder builder = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Undo")) builder.Undo();
            if (other.CompareTag("Proton")) builder.AddProton();
            if (other.CompareTag("Neutron")) builder.AddNeutron();
            if (other.CompareTag("Electron")) builder.AddElectron();

            string pattern = @"\b(?:Proton|Neutron|Electron|Undo)";
            if (Regex.IsMatch(other.tag, pattern)) Destroy(other.gameObject);
        }
    }
}
