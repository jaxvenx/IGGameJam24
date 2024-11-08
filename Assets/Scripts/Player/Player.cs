using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // TwoDimensional float array to store the players seven deadly sin values
    private int[,] sevenDeadlySins = new int[7, 1];
    private string[] sinNames = { "Lust", "Gluttony", "Greed", "Sloth", "Wrath", "Envy", "Pride" };
    [SerializeField] private int maxSinValue = 10;
    [SerializeField] private int minSinValue = 0;

    private void Start()
    {
        //Each sin gets filled halfway at the start of the game
        for (int i = 0; i < sevenDeadlySins.GetLength(0); i++)
        {
            sevenDeadlySins[i, 0] = (maxSinValue + minSinValue) / 2;
        }
    }

    public void TakeDamage()
    {
        //If all sins are at their minimum value, the player dies
        for (int i = 0; i < sevenDeadlySins.GetLength(0); i++)
        {
            if (sevenDeadlySins[i,0] <= minSinValue)
            {
                Debug.Log("Player died");
                return;
            }
        }
        //Each sin gets decreased by 0.1f when the player takes damage
        for (int i = 0; i < sevenDeadlySins.GetLength(0); i++)
        {
            sevenDeadlySins[i, 0] -= 1;
            //If the sin value is below the minimum value, set it to the minimum value
            if (sevenDeadlySins[i, 0] <= minSinValue)
            {
                sevenDeadlySins[i, 0] = minSinValue;
            }
            //Print the sin value to the console
            Debug.Log("Sin " + sinNames[i] + " value: " + sevenDeadlySins[i, 0]);
        }
    }
}
