using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class LenguajeDropDown : MonoBehaviour
{
    public static LenguajeDropDown instance;
    [HideInInspector]
    public enum Lenguajes
    {
        English,
        Deutsch,
        Turkish,
        French,
        Spanish
    }
    public TMP_Dropdown dropdown;
    //public Text selectedName;
    [HideInInspector]
    public Lenguajes lenguaje;

    private void Awake()
    {
        instance = this;
        //lenguaje = Lenguajes.Turkish;
        //dropdown.itemText.text = lenguaje.ToString();
        //dropdown.value = (int)lenguaje;
    }
    public void Dropdown_IndexChanged(int index)
    {
        lenguaje = (Lenguajes)index;
        GameManager.Instance.changeLenguaje();
        GameManager.Instance.setLanguageIndex(index);

    }
    private void Start()
    {
        PopulateList();
        dropdown.value = (int)lenguaje;
        //dropdown.image = GameManager.Instance.vars.LanguageFlagsSPrites[(int)lenguaje]; 
        //dropdown.itemText.text = Enum.GetName(typeof(Lenguajes), lenguaje);
    }

    private void PopulateList()
    {
        string[] enumNames = Enum.GetNames(typeof(Lenguajes));
        List<string> names = new List<string>(enumNames);

        List<TMP_Dropdown.OptionData> LenguageItems = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < names.Count; i++)
        {
            //dropdown.AddOptions(GameManager.Instance.vars.LanguageFlagsSPrites);

            var LanguageOption = new TMP_Dropdown.OptionData(names[i], GameManager.Instance.vars.LanguageFlagsSPrites[i]);
            LenguageItems.Add(LanguageOption);
        }
        dropdown.AddOptions(LenguageItems);
        //dropdown.options[0].image.
        //dropdown.itemImage.sprite = GameManager.Instance.vars.LanguageFlagsSPrites[0];

    }

    public void InitLanguage(int index)
    {
        if(index == 0)
        {
            lenguaje = Lenguajes.English;
        }else if (index == 1)
        {
            lenguaje = Lenguajes.Deutsch;
        }
        else if (index == 2)
        {
            lenguaje = Lenguajes.Turkish;
        }
        else if (index == 3)
        {
            lenguaje = Lenguajes.French;
        }
        else if (index == 4)
        {
            lenguaje = Lenguajes.Spanish;
        }
    }

    private void Update()
    {
        /*if (dropdown.IsExpanded)
        {

        }*/
    }
}
