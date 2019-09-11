using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson; // <- Insert!

public class TurnManager : MonoBehaviour
{
    // Tempo de duração para cada turno.
    [Range(1, 10)]
    public int turnTime = 5;

    // Lista de agentes para os turnos.
    public List<GameObject> charList = new List<GameObject>();

    // A quantidade de agentes na lista.
    private int players;

    // Recebe a conversão em segundos do tempo decorrido.
    private float secondsCount;

    // De quem é o turno?
    private int charTurn;

    // Com quem começa?
    public int charStart = 1;

    void Start()
    {
        // Armazenando a quantidade de agentes.
        players = charList.Count;

        foreach (GameObject agents in charList)
        {
            // Resetando de quem é a vez.
            //agents.GetComponent<MouseController>().enabled = false;
            agents.GetComponent<MouseController>().myTurn = false;
            agents.transform.GetChild(0).gameObject.SetActive(false);
        }

        // Definir que é o primeiro.
        //charList[charStart - 1].GetComponent<MouseController>().enabled = true;
        charList[charStart - 1].GetComponent<MouseController>().myTurn = true;
        charList[charStart - 1].transform.GetChild(0).gameObject.SetActive(true);
    }

    void Update()
    {
        ChangeAgentTurn(players);
    }

    private void ChangeAgentTurn(int pCount)
    {
        secondsCount += Time.deltaTime;
        //Debug.Log("secondsCount = " + secondsCount);
        //Debug.Log("Time.deltaTime = " + Time.deltaTime);

        // Se a quantidade de segundos for maior que o tempo definido com turno...
        if(secondsCount > turnTime)
        {
            // Subtrai da quantidade de segundos o tempo definido por turno.
            secondsCount -= turnTime;
            //Debug.Log("Troca turno!");

            // Aponta de quem é a vez.
            // 1 % 2 = 1 // 2 % 2 = 0
            // 1 % 3 = 1 // 2 % 3 = 2 // 3 % 3 = 0
            // 1 % 4 = 1 // 2 % 4 = 2 // 3 % 4 = 3 // 4 % 4 = 0
            charTurn = charStart % charList.Count;

            for(int i = 0; i < charList.Count; i++)
            {
                //charList[i].GetComponent<MouseController>().enabled = false;
                charList[i].GetComponent<MouseController>().myTurn = false;
                charList[i].transform.GetChild(0).gameObject.SetActive(false);

                if(i == charTurn)
                {
                    //charList[i].GetComponent<MouseController>().enabled = true;
                    charList[i].GetComponent<MouseController>().myTurn = true;
                    charList[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            
            if(charStart < charList.Count)
            {
                charStart++;
            }
            else
            {
                charStart = 1;
            }
        }
    }
}
