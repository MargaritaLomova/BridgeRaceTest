using UnityEngine;

public class StepController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private GameObject visualStep;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (visualStep.GetComponent<MeshRenderer>().enabled)
            {
                other.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            }
            else
            {
                if (other.GetComponent<PlayerController>().GetCollectedBlocksCount() > 0)
                {
                    visualStep.GetComponent<MeshRenderer>().enabled = true;
                    other.GetComponent<PlayerController>().PutBlockLikeStep();
                    other.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                }
            }
        }
    }

    public GameObject Visual()
    {
        return visualStep;
    }
}