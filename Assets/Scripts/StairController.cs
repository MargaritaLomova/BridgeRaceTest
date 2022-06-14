using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StairController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private List<StepController> steps = new List<StepController>();

    public void Set(Color color)
    {
        foreach(var step in steps)
        {
            step.Visual().GetComponent<MeshRenderer>().material.color = color;
            step.Visual().GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public int GetStepsCount()
    {
        return steps.Count;
    }

    public bool IsFinished()
    {
        foreach(var visualStep in steps.Select(x => x.Visual()))
        {
            if(!visualStep.GetComponent<MeshRenderer>().enabled)
            {
                return false;
            }
        }
        return true;
    }
}