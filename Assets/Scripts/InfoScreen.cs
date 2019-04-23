using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    public string Nome_tela;
    [TextArea]
    public string descricao;
    public Text ref_tela;
    public Text ref_descricao;
    public GameObject inter;
    
    public void Info(){
        ref_tela.text = Nome_tela;
        ref_descricao.text = descricao;

        inter.SetActive(true);
    }
}