using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager instance;

    public Instruction[] instructionSet; // Store instructions here
    public FoodAnimalCombo[] foodAnimalCombos; // Store food-animal combos here

    private int currentInstructionIndex = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Instruction GetInstructionForCombo(FoodAnimalCombo combo)
    {
        // Find the instruction associated with the given combo
        foreach (Instruction instruction in instructionSet)
        {
            if(instruction.combo == combo)
            {
                return instruction;
            }
        }
        // Handle the case when no instruction is found
        return null;
    }
}