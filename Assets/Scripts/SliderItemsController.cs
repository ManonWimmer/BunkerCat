using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderItemsController : MonoBehaviour
{
    public int currentProgress = 0;
    public int maxProgress = 0;

    private GameObject[] itemsInScene;

    [SerializeField]
    private Slider slider;


    private void Start()
    {
        InitializeMaxProgress();
        slider.value = 0;
    }

    public void InitializeMaxProgress()
    {
        itemsInScene = GameObject.FindGameObjectsWithTag("Items");
        for (int i = 0; i < itemsInScene.Length; i++)
        {
            maxProgress += itemsInScene[i].GetComponent<Item>().Quantity;
        }
        Debug.Log(maxProgress);
        slider.maxValue = maxProgress;
    }

    public void UpdateProgressItems(int valueToAdd)
    {
        Debug.Log("update progress bar items");
        currentProgress += valueToAdd;
        slider.value = currentProgress;
    }
}
