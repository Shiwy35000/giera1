using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrybDzia�ania {celKarta, celWrug};

public class celLinia : MonoBehaviour
{
    public Transform podparcie1, podparcie2;
    public LineRenderer lineRenderer;
    private float poprawkaWysoko�ciPojedy�cza = 2f;
    [HideInInspector] public Vector3 pocz�tek, koniec;
    [HideInInspector] public TrybDzia�ania trybDzia�ania;
    private Vector3 pocz�tekVwzgl�dnam, koniecVwzgl�dna;

    void Update()
    {
        if (pocz�tek != null && koniec != null)
        {
            if (trybDzia�ania == TrybDzia�ania.celKarta)
            {
                PodparciePojedy�cze(pocz�tek, koniec);
                RysyujLinie1(pocz�tek, podparcie1.position, koniec);
            }
            else if (trybDzia�ania == TrybDzia�ania.celWrug)
            {
                PodparciePodwujne(pocz�tek, koniec);
                RysyujLinie2(pocz�tek, podparcie1.position, podparcie2.position, koniec);
            }
        }
    }

    private void PodparciePojedy�cze(Vector3 Pocz�tek, Vector3 Koniec)
    {
        Vector3 newPoz = new Vector3(Pocz�tek.x + Koniec.x, Pocz�tek.y + Koniec.y, Pocz�tek.z + Koniec.z) / 2;
        float plusY = Mathf.Abs(Pocz�tek.y - Koniec.y);
        newPoz.y += (plusY + poprawkaWysoko�ciPojedy�cza);
        podparcie1.position = newPoz;
    }
    private void PodparciePodwujne(Vector3 Pocz�tek, Vector3 Koniec)
    {
        Vector3 newPoz1 = new Vector3(Pocz�tek.x, Koniec.y, Pocz�tek.z);
        Vector3 newPoz2 = new Vector3(Koniec.x, Pocz�tek.y, Koniec.z);   
        podparcie1.position = newPoz1;
        podparcie2.position = newPoz2;
    }

    private void RysyujLinie1(Vector3 Pocz�tek, Vector3 p1, Vector3 Koniec)
    {
        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * Pocz�tek + 2 * (1 - t) * t * p1 + t * t * Koniec;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
    private void RysyujLinie2(Vector3 Pocz�tek, Vector3 p1, Vector3 p2, Vector3 Koniec)
    {
        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * (1 - t) * Pocz�tek + 3 * (1 - t) * (1 - t) *
                t * p1 + 3 * (1 - t) * t * t * p2 + t * t * t * Koniec;

            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
}
