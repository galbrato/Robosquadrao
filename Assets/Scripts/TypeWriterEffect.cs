using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 0.05f;
    public string FullText;
    private string currentText = "";
    private Text texto;
    void Start()
    {
        texto = GetComponent<Text>();
        StartCoroutine(ShowText());
    }


    void OnEnable(){
        Start();
    }
    public IEnumerator ShowText(){
        for(int i = 0; i <= FullText.Length; i++){
            currentText = FullText.Substring(0, i);
            texto.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
