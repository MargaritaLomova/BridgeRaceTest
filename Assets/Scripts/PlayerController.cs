using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects From Scene"), SerializeField]
    private LevelController levelController;

    [Header("Variables To Control"), SerializeField]
    private float moveSpeed = 0.3f;

    private List<BlockController> collectedBlocks = new List<BlockController>();

    private void FixedUpdate()
    {
        Movement();
    }

    #region Custom Methods

    public void Set(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    public void PutBlockLikeStep()
    {
        Destroy(collectedBlocks.Last().gameObject);
        collectedBlocks.Remove(collectedBlocks.Last());
        levelController.NewStepIsBuilded();
    }

    public int GetCollectedBlocksCount()
    {
        return collectedBlocks.Count;
    }

    public void NewBlockCollected(BlockController newBlock)
    {
        collectedBlocks.Add(newBlock);
    }

    private void Movement()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed * Time.fixedDeltaTime);
    }

    #endregion
}