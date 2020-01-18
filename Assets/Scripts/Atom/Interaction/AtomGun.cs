using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using TMPro;

using Interactables;
using Atom.Enum;

namespace Atom.Interaction
{
    public class AtomGun : MonoBehaviour, IMixedRealityInputHandler
    {
        [Header("Interaction")]
        [SerializeField] private MixedRealityInputAction shootAction = MixedRealityInputAction.None;
        [SerializeField] private MixedRealityInputAction toggleAction = MixedRealityInputAction.None;

        [Header("Bullet")]
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private GameObject protonBullet = null;
        [SerializeField] private GameObject neutronBullet = null;
        [SerializeField] private GameObject electronBullet = null;
        [SerializeField] private GameObject undoBullet = null;
        [SerializeField] private float speed = 20.0f;
        [SerializeField] private int destroy = 5;

        [Header("Mode")]
        [SerializeField] private TextMeshPro label = null;

        private AtomMode mode = AtomMode.Proton;
        private Grabbable grabbable = null;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip shotSound = null;

        private void Awake()
        {
            grabbable = GetComponent<Grabbable>();
            label.text = System.Enum.GetName(typeof(AtomMode), mode);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);

            if (!grabbable) Debug.LogWarning("No Grabbable Component Provided!");
        }

        private void Update()
        {
            Vector3 direction = transform.forward * 10;
            Debug.DrawRay(transform.position, direction, Color.green);
        }

        public void OnInputUp(InputEventData eventData) { }

        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == shootAction && grabbable.ActiveController != null)
            {
                Shoot();
                eventData.Use();
            }
            if (eventData.MixedRealityInputAction == toggleAction && grabbable.ActiveController != null)
            {
                Toggle();
                eventData.Use();
            }
        }

        private void Shoot()
        {
            GameObject bullet = Instantiate(GetBullet(), spawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(spawnPoint.transform.forward * speed, ForceMode.Impulse);
            audioSource.PlayOneShot(shotSound);
            Destroy(bullet, destroy);
        }

        private void Toggle()
        {
            if (mode == AtomMode.Proton) mode = AtomMode.Neutron;
            else if (mode == AtomMode.Neutron) mode = AtomMode.Electron;
            else if (mode == AtomMode.Electron) mode = AtomMode.Undo;
            else if (mode == AtomMode.Undo) mode = AtomMode.Proton;

            label.text = System.Enum.GetName(typeof(AtomMode), mode);
        }

        private GameObject GetBullet()
        {
            if (mode == AtomMode.Undo) return undoBullet;
            if (mode == AtomMode.Proton) return protonBullet;
            if (mode == AtomMode.Neutron) return neutronBullet;
            if (mode == AtomMode.Electron) return electronBullet;

            return null;
        }
    }
}
