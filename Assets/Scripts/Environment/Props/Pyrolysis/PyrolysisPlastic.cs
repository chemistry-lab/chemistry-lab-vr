using UnityEngine;

namespace Environment.Props.Pyrolysis
{
    public class PyrolysisPlastic : MonoBehaviour
    {
        [SerializeField] private float amountOfPlastic = 1.0f;

        public float AmountOfPlastic { get { return amountOfPlastic; } }
    }
}
