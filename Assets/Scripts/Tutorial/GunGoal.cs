using UnityEngine;

using Atom.Builder;
using Atom.Enum;
using Utilities;

namespace Tutorial
{
    public class GunGoal : MonoBehaviour
    {
        [Header("Builder")]
        [SerializeField] private AtomBuilder builder = null;

        [Header("Scene")]
        [SerializeField] private SceneLoader loader = null;
        [SerializeField] private UnityScene scene = null;

        void Update()
        {
            if (builder.GetCount(AtomPart.Proton) == 4 && builder.GetCount(AtomPart.Neutron) == 4 && builder.GetCount(AtomPart.Electron) == 4)
            {
                loader.LoadScene(scene);
            }
        }
    }
}
