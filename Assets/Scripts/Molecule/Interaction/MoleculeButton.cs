﻿using System;
using UnityEngine;

using Interactables;
using Molecule.Builder;

namespace Molecule.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class MoleculeButton : MonoBehaviour
    {
        [Header("Build")]
        [SerializeField] private MoleculeBuilder builder = null;

        [Header("Highlight")]
        [SerializeField] private Renderer button = null;
        [Range(0f, 0.1f)]
        [SerializeField] private float thickness = 0.025f;

        [Header("Audio")] 
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip audioClip = null;

        private void OnTriggerEnter(Collider other)
        {
            builder.CreateMolecule();
            button.material.SetFloat("_OutlineWidth", thickness);
            audioSource.PlayOneShot(audioClip);
        }

        private void OnTriggerExit(Collider other)
        {
            button.material.SetFloat("_OutlineWidth", 0f);
        }
    }
}
