using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager instance;

    public List<AudioClip> audioInstructions;
    public List<string> textInstructions;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Instruction GetInstructionForCombo(FoodAnimalCombo combo)
    {
        // Search for the corresponding audio and text instructions based on the combo
        for (int i = 0; i < audioInstructions.Count; i++)
        {
            if (audioInstructions[i] != null && textInstructions.Count > i)
            {
                string audioClipName = audioInstructions[i].name;
                string expectedAudioClipName = $"{combo.foodName}{combo.animalName}Audio";
                if (audioClipName == expectedAudioClipName)
                {
                    // Return the found audio and text instruction
                    return new Instruction(audioInstructions[i], textInstructions[i]);
                }
            }
        }
        // If no instruction is found, return null.
        return null;
    }
}

[System.Serializable]
public class FoodAnimalCombo
{
    public string foodName;
    public string animalName;
}

[System.Serializable]
public class Instruction
{
    public AudioClip audioClip;
    public string textInstruction;

    public Instruction(AudioClip audio, string text)
    {
        audioClip = audio;
        textInstruction = text;
    }
}