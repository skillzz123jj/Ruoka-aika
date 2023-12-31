using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Instruction GetInstructionForCombo(FoodAnimalCombo combo)
    {

        bool audioInstructionsEnabled = PlayerPrefs.GetInt("AudioInstructionsEnabled", 1) == 1;
        bool textInstructionsEnabled = PlayerPrefs.GetInt("TextInstructionsEnabled", 1) == 1;

        // Search for the corresponding audio and text instructions based on the combo
        foreach (FoodAnimalInstruction instruction in instructions)
        {
            if (instruction.combo.foodName == combo.foodName && instruction.combo.animalName == combo.animalName)
            {
                
                AudioClip audioClip = (audioInstructionsEnabled) ? instruction.audioClip : null;
                string textInstruction = (textInstructionsEnabled) ? instruction.textInstruction : null;

                return new Instruction(audioClip, textInstruction);
                
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