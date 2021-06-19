using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Variables To Control Level"), SerializeField]
    private List<Color> colors = new List<Color>()
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };
    [SerializeField]
    private int countOfBlocksOnScene = 30;

    [Header("Prefabs"), SerializeField]
    private BlockController blockPrefab;
    [SerializeField]
    private GameObject planePrefab;

    [Header("Objects From Scene"), SerializeField]
    private PlayerController player;

    private Color playerColor;
    private List<BlockController> spawnedBlocks = new List<BlockController>();
    private List<GameObject> spawnedPlanes = new List<GameObject>();
    private StairController lastStair;

    private void Start()
    {
        playerColor = colors[Random.Range(0, colors.Count)];

        player.Set(playerColor);

        SetNewScene(new Vector3(0, 0, 0));
    }

    #region Custom Methods

    public void NewStepIsBuilded()
    {
        if(lastStair.IsFinished())
        {
            SetNewScene(new Vector3(spawnedPlanes.Last().transform.position.x - 2.5f, spawnedPlanes.Last().transform.position.y + 7.5f, spawnedPlanes.Last().transform.position.z + 40));
        }
    }

    private void SetNewScene(Vector3 planePosition)
    {
        var newPlane = Instantiate(planePrefab, planePosition, Quaternion.identity);
        spawnedPlanes.Add(newPlane);
        var newStair = newPlane.GetComponentInChildren<StairController>();
        lastStair = newStair;

        newStair.Set(playerColor);

        spawnedBlocks.Clear();

        for (int i = 0; i < countOfBlocksOnScene; i++)
        {
            var newBlock = Instantiate(blockPrefab, CreateSuitableVector(newPlane.transform), Quaternion.identity);
            newBlock.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Count)];
            spawnedBlocks.Add(newBlock);
        }

        if (spawnedBlocks.Where(x => x.GetColor() == playerColor).Count() < newStair.GetStepsCount())
        {
            StartCoroutine(SpawnAdditionalBlocksOfDesiredColor(newPlane.transform));
        }

        RemoveUselesPlanes();
    }

    private void RemoveUselesPlanes()
    {
        if (spawnedPlanes.Count > 2)
        {
            Destroy(spawnedPlanes.FirstOrDefault().gameObject);
            spawnedPlanes.Remove(spawnedPlanes.FirstOrDefault());
        }
    }

    private IEnumerator SpawnAdditionalBlocksOfDesiredColor(Transform plane)
    {
        while (spawnedBlocks.Where(x => x.GetColor() == playerColor).Count() < lastStair.GetStepsCount())
        {
            var newBlock = Instantiate(blockPrefab, CreateSuitableVector(plane), Quaternion.identity);
            newBlock.SetColor(playerColor);
            spawnedBlocks.Add(newBlock);
            yield return null;
        }
    }

    private Vector3 CreateSuitableVector(Transform plane)
    {
        var X = Random.Range(plane.position.x - 10, plane.position.x + 10);
        var Z = Random.Range(plane.position.z - 10, plane.position.z + 10);
        var Y = plane.position.y + 0.25f;

        return new Vector3(X, Y, Z);
    }

    #endregion
}