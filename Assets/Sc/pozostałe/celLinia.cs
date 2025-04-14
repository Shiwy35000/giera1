using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrybDzia³ania {celKarta, celWrug};

public class celLinia : MonoBehaviour
{
    public Transform podparcie1, podparcie2;
    public LineRenderer lineRenderer;
    private float poprawkaWysokoœciPojedyñcza = 2f;
    [HideInInspector] public Vector3 pocz¹tek, koniec;
    [HideInInspector] public TrybDzia³ania trybDzia³ania;
    private Vector3 pocz¹tekVwzglêdnam, koniecVwzglêdna;

    void Update()
    {
        if (pocz¹tek != null && koniec != null)
        {
            if (trybDzia³ania == TrybDzia³ania.celKarta)
            {
                PodparciePojedyñcze(pocz¹tek, koniec);
                RysyujLinie1(pocz¹tek, podparcie1.position, koniec);
            }
            else if (trybDzia³ania == TrybDzia³ania.celWrug)
            {
                PodparciePodwujne(pocz¹tek, koniec);
                RysyujLinie2(pocz¹tek, podparcie1.position, podparcie2.position, koniec);
            }
        }
    }

    private void PodparciePojedyñcze(Vector3 Pocz¹tek, Vector3 Koniec)
    {
        Vector3 newPoz = new Vector3(Pocz¹tek.x + Koniec.x, Pocz¹tek.y + Koniec.y, Pocz¹tek.z + Koniec.z) / 2;
        float plusY = Mathf.Abs(Pocz¹tek.y - Koniec.y);
        newPoz.y += (plusY + poprawkaWysokoœciPojedyñcza);
        podparcie1.position = newPoz;
    }
    private void PodparciePodwujne(Vector3 Pocz¹tek, Vector3 Koniec)
    {
        Vector3 newPoz1 = new Vector3(Pocz¹tek.x, Koniec.y, Pocz¹tek.z);
        Vector3 newPoz2 = new Vector3(Koniec.x, Pocz¹tek.y, Koniec.z);   
        podparcie1.position = newPoz1;
        podparcie2.position = newPoz2;
    }

    private void RysyujLinie1(Vector3 Pocz¹tek, Vector3 p1, Vector3 Koniec)
    {
        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * Pocz¹tek + 2 * (1 - t) * t * p1 + t * t * Koniec;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
    private void RysyujLinie2(Vector3 Pocz¹tek, Vector3 p1, Vector3 p2, Vector3 Koniec)
    {
        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * (1 - t) * Pocz¹tek + 3 * (1 - t) * (1 - t) *
                t * p1 + 3 * (1 - t) * t * t * p2 + t * t * t * Koniec;

            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }
}
