using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

using Avatars.Enum;
using DialogFlow;

namespace Avatars.Speech
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class SpeechInteraction : MonoBehaviour, IMixedRealityInputHandler
    {
        [Header("Input")]
        [SerializeField] private MixedRealityInputAction action = MixedRealityInputAction.None;

        [Header("Voice")]
        [SerializeField] private AvatarGender gender = AvatarGender.Neutral;

        [Header("Indicator")]
        [SerializeField] private GameObject indicator = null;
        [SerializeField] private Material listeningMaterial = null;
        [SerializeField] private Material defaultMaterial = null;
        [SerializeField] private Material busyMaterial = null;

        private bool isMicrophoneConnected = false;
        private bool isMicrophoneRecording = false;
        private bool isProcessingAudio = false;
        private bool isPressingAction = false;
        private bool inProximity = false;

        private DialogFlowService service = null;
        private AudioSource source = null;
        private int minimumFrequency = 0;
        private int maximumFrequency = 0;

        private Transform playerTransform = null;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                Debug.LogWarning("No AudioSource Component Provided!");
            }
            if (Microphone.devices.Length <= 0)
            {
                Debug.LogWarning("No Microphone Connected!");
                return;
            }

            Microphone.GetDeviceCaps(null, out minimumFrequency, out maximumFrequency);
            if (minimumFrequency == 0 && maximumFrequency == 0)
            {
                maximumFrequency = 44100;
            }

            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
            indicator.gameObject.SetActive(false);
            service = new DialogFlowService();
            isMicrophoneConnected = true;
        }

        private void Update()
        {
            if (isMicrophoneConnected && inProximity)
            {
                if (isPressingAction) StartListening();
                else StopListening();
                SetMaterial();
            }

            if (inProximity && playerTransform) transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                playerTransform = other.transform;
                inProximity = true;
                indicator.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                playerTransform = null;
                inProximity = false;
                indicator.SetActive(false);
            }
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (isMicrophoneConnected && inProximity && eventData.MixedRealityInputAction == action)
            {
                isPressingAction = false;
                eventData.Use();
            }
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (isMicrophoneConnected && inProximity && eventData.MixedRealityInputAction == action)
            {
                isPressingAction = true;
                eventData.Use();
            }
        }

        private void StartSpeaking(AudioClip audio)
        {
            if (audio == null)
            {
                isProcessingAudio = false;
                return;
            }

            source.spatialBlend = 0.0f;
            source.clip = audio;
            source.Play();
            isProcessingAudio = false;
        }

        private void StartListening()
        {
            if (!isMicrophoneRecording && !isProcessingAudio && !source.isPlaying)
            {
                source.clip = Microphone.Start(null, true, 20, 16000);
                isMicrophoneRecording = true;
            }
        }

        private void StopListening()
        {
            if (isMicrophoneRecording && !isProcessingAudio && !source.isPlaying)
            {
                Microphone.End(null);
                isMicrophoneRecording = false;
                isProcessingAudio = true;

                StartCoroutine(service.DetectIntent(source.clip, gender, StartSpeaking));
            }
        }

        private void SetMaterial()
        {
            if (isMicrophoneRecording)
            {
                indicator.GetComponent<Renderer>().material = listeningMaterial;
                return;
            }
            if (isProcessingAudio || source.isPlaying)
            {
                indicator.GetComponent<Renderer>().material = busyMaterial;
                return;
            }
            indicator.GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}
