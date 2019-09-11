using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // <- Importar!
using UnityStandardAssets.Characters.ThirdPerson; // <- Importar!

public class MouseController : MonoBehaviour
{
    public Camera myMainCamera;

    public NavMeshAgent myAgent;

    public ThirdPersonCharacter myTPC;

    // É o turno do GameObject que possui esse script?
    public bool myTurn = true;

    // Indicador de destino do personagem.
    public GameObject totem;

    void Start()
    {
        // Desabilitar a atualização da rotação do agente. 
        myAgent.updateRotation = false;
    }

    void Update()
    {
        if (myTurn == true)
        {
            // 0 (esquerdo), 1 (direito), 2 (meio)
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Clicou 0!");

                SetDestinationToMousePosition();
            }

            //Debug.Log(Input.mousePosition);

            // Update para exbir o raio!
            Ray myRay = myMainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(myRay.origin, myRay.direction * 25, Color.red);

            // Se a distancia restante do agente para o alvo for maior...
            // ...que o ponto de parada...
            if (myAgent.remainingDistance > myAgent.stoppingDistance)
            {
                // TPC mover de acordo com a velocidade desejada...
                // ...não vai agachar e não vai pular.
                myTPC.Move(myAgent.desiredVelocity, false, false); // <-
            }
            else
            {
                myTPC.Move(Vector3.zero, false, false);
            }
        }
        else
        {
            myTPC.Move(myAgent.desiredVelocity, false, false); // <-
        }
    }

    void SetDestinationToMousePosition()
    {
        //Debug.Log("SetDestinationToMousePosition!");

        // Retornar um raio (Ray) indo da MainCamera atrevéz de um ponto na tela.
        Ray myRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        // Obter um raycast!
        RaycastHit myRaycastHit;

        if (Physics.Raycast(myRay, out myRaycastHit))
        {
            Debug.Log(myRaycastHit.collider.name); // <- Update!

            if (myRaycastHit.collider.CompareTag("Coletavel"))
            {

                myRaycastHit.collider.tag = "Destruir";
            }

            myAgent.SetDestination(myRaycastHit.point);
            Debug.DrawLine(myRay.origin, myRaycastHit.point, Color.red);

            // Reposicionar a indicação de navegação do agente.
            totem.transform.position = myRaycastHit.point;
        }
    }

    // O objeto que colidir com o personagem vai destruir...
    private void OnCollisionStay(Collision myCollision)
    {
        if (myCollision.gameObject.CompareTag("Destruir"))
        {
            Destroy(myCollision.gameObject);
        }
    }
}
