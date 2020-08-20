using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class SetRandomButtonPositions : MonoBehaviour
{
    [SerializeField] List<Transform> Buttons;
    [SerializeField] List<Transform> ButtonPositions;
    [SerializeField] List<Material> ButtonMaterials;

    private void Awake()
    {
        ShuffleList();
        //for(int i = 0; i < ButtonPositions.Count; ++i)
        //{
        //    GameObject button = Instantiate(ButtonPrefab,ButtonPositions[i].position,ButtonPositions[i].rotation, ButtonPositions[i].transform);
        //    ButtonLUT buttonLut = button.GetComponent<ButtonLUT>();
        //    buttonLut.ButtonController.buttonNumber = i;
        //    buttonLut.SymbolPlaneMeshRenderer.material = ButtonMaterials[i];
        //}

        for(int i = 0; i < Buttons.Count; ++i)
        {
            Buttons[i].transform.position = ButtonPositions[i].transform.position;
            Buttons[i].transform.rotation = ButtonPositions[i].transform.rotation;
            Buttons[i].gameObject.SetActive(true);
        }
    }

    public void ShuffleList()
    {
        for (int i = 0; i < ButtonPositions.Count; i++)
        {
            Transform temp = ButtonPositions[i];
            int randomIndex = UnityEngine.Random.Range(i, ButtonPositions.Count);
            ButtonPositions[i] = ButtonPositions[randomIndex];
            ButtonPositions[randomIndex] = temp;
        }
    }

}
