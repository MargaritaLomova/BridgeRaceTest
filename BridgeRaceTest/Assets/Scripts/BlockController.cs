using System.Collections;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private ParticleSystem plume;

    private Material material;

    private void Start()
    {
        plume.Stop();
        plume.gameObject.SetActive(false);

        material = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && material.color == other.GetComponent<MeshRenderer>().material.color)
        {
            if (other.GetComponent<PlayerController>().GetCollectedBlocksCount() < 5)
            {
                Magnetization(other.gameObject);
            }
            else
            {
                Debug.Log("You are carrying too many blocks!");
            }
        }
    }

    #region Custom Methods

    public void SetColor(Color color)
    {
        if(material == null)
        {
            material = GetComponent<MeshRenderer>().material;
        }
        material.color = color;
    }

    public Color GetColor()
    {
        if (material == null)
        {
            material = GetComponent<MeshRenderer>().material;
        }
        return material.color;
    }

    private void Magnetization(GameObject gameObjectToMagnite)
    {
        PlayerController playerController = gameObjectToMagnite.GetComponent<PlayerController>();

        plume.gameObject.SetActive(true);
        plume.Play();
        transform.SetParent(gameObjectToMagnite.transform);
        playerController.NewBlockCollected(gameObject.GetComponent<BlockController>());

        StartCoroutine(MagnitizationAnimation(90, new Vector3(0, playerController.GetCollectedBlocksCount() * transform.localScale.y, -0.75f), 0.2f));
    }

    private IEnumerator MagnitizationAnimation(float degreesToRotate, Vector3 positionToMove, float duration = 1)
    {
        Quaternion fromRotate = transform.rotation;
        Quaternion toRotate = transform.rotation;
        toRotate *= Quaternion.Euler(Vector3.up * degreesToRotate);

        Vector3 fromPosition = transform.localPosition;

        float elapsed = 0;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(fromRotate, toRotate, elapsed / duration);
            transform.localPosition = Vector3.Lerp(fromPosition, positionToMove, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = toRotate;
        transform.localPosition = positionToMove;
    }

    #endregion
}