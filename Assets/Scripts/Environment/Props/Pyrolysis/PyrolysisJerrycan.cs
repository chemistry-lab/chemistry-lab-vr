using UnityEngine;

namespace Environment.Props.Pyrolysis
{
    public class PyrolysisJerrycan : MonoBehaviour
    {
        [SerializeField] private float amountOfFuel = 100.0f;

        public float AmountOfFuel { get { return amountOfFuel; } }
    }
}
