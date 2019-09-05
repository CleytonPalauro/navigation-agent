using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargets : MonoBehaviour
{
    // Lista com os alvos para a câmera.
    public List<Transform> targets;

    [Range(1f, 200f)]
    public float minZoom = 50f;

    [Range(1f, 200f)]
    public float maxZoom = 20f;

    [Range(1f, 200f)]
    public float limitZoom = 50f;

    // Compensação dos valores da câmera.
    public Vector3 offset;

    // Suavização da navegação da câmera.
    [Range(0.1f, 1f)]
    public float smooth;

    private Vector3 camVelocity;

    private Camera myCamera;

    void Start()
    {
        myCamera = GetComponent<Camera>();
    }
    
    void Update()
    {
        if(targets.Count == 0)
        {
            Debug.LogError("Não há alvos para a camera");
        }

        ZoomCamera();

        MoveCamera();
    }

    private void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetBoundsGreatestDistance() / limitZoom);

        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, newZoom, Time.deltaTime);
    }

    private void MoveCamera()
    {
        Vector3 centerPoint = GetBoundsCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position, newPosition, ref camVelocity, smooth);
    }

    // Pega o tamanho da distancia no exio X da caixa delimitadora (Bounds).
    private float GetBoundsGreatestDistance()
    {
        // Pegando o primeiro target como origem.
        Bounds myBounds = new Bounds(targets[0].position, Vector3.zero);

        for(int i = 0; i < targets.Count; i++)
        {
            myBounds.Encapsulate(targets[i].position);
        }

        Debug.Log("x = " + myBounds.size.x);
        Debug.Log("y = " + myBounds.size.y);
        Debug.Log("z = " + myBounds.size.z);

        return myBounds.size.x;
    }

    // Pega o centro da caixa delimitadora (Bounds).
    private Vector3 GetBoundsCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        // Pegando o primeiro target como origem.
        Bounds myBounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            myBounds.Encapsulate(targets[i].position);
        }

        return myBounds.center;
    }

}
