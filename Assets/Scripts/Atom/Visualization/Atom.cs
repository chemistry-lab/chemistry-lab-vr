using UnityEngine;

namespace Atom.Visualization
{
    public class Atom : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private Orbital firstOrbital = null;
        [SerializeField] private Orbital secondOrbital = null;
        [SerializeField] private Orbital thirdOrbital = null;
        [SerializeField] private Nucleus nucleus = null;

        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip addSound = null;
        [SerializeField] private AudioClip removeSound = null;

        public GameObject AddProton()
        {
            PlayAddSound();
            return nucleus.AddProton();
        }

        public GameObject AddNeutron()
        {
            PlayAddSound();
            return nucleus.AddNeutron();
        }

        public GameObject AddElectron(int count)
        {
            PlayAddSound();
            if (count >= 2 && count < 10) return secondOrbital.AddElectron();
            else if (count < 2) return firstOrbital.AddElectron();
            return null;
        }

        public GameObject SubtractProton()
        {
            PlayRemoveSound();
            return nucleus.SubtractProton();
        }

        public GameObject SubtractNeutron()
        {
            PlayRemoveSound();
            return nucleus.SubtractNeutron();
        }

        public GameObject SubtractElectron(int count)
        {
            PlayRemoveSound();
            if (count > 2) return secondOrbital.SubtractElectron();
            else if (count <= 2) return firstOrbital.SubtractElectron();
            return null;
        }

        private void PlayAddSound()
        {
            audioSource.PlayOneShot(addSound, 0.5f);
        }

        private void PlayRemoveSound()
        {
            audioSource.PlayOneShot(removeSound, 1f);
        }
    }
}
