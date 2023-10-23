using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager instance;

    [System.Serializable]
    public class FoodAnimalInstruction
    {
        public FoodAnimalCombo combo;
        public AudioClip audioClip;
        public string textInstruction;

    }

    public List<FoodAnimalInstruction> instructions;
    //public List<AudioClip> audioInstructions;
    //public List<string> textInstructions;

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
        foreach (FoodAnimalInstruction instruction in instructions)
        {
            if (instruction.combo.foodName == combo.foodName && instruction.combo.animalName == combo.animalName)
            {
                //string audioClipName = audioInstructions[i].name;
                //string expectedAudioClipName = $"{combo.foodName}{combo.animalName}Audio";
                //if (audioClipName == expectedAudioClipName)
                // Return the found audio and text instruction
                //return new Instruction(audioInstructions[i], textInstructions[i]);
                return new Instruction(instruction.audioClip, instruction.textInstruction);
                
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