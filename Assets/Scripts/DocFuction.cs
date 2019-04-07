using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocFuction : MonoBehaviour
{
    // Start is called before the first frame update

    public string parametro;
    public string saida;
    [TextArea]
    public string contexto;
    public Text ref_parametro;
    public Text ref_saida;
    public Text ref_contexto;
    public GameObject inter;
    public void FYI(){
        ref_parametro.text = parametro;
        ref_saida.text = saida;
        ref_contexto.text = contexto;

        inter.SetActive(true);
    }
}
