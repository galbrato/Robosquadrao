﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CodeManager : MonoBehaviour{
    //Cursos e variaveis para fazer o cursor piscar
    public Text Cursor;
   
    //Lista de prfabs de codigo
    public GameObject _UIPrefabAndarAte;

    
    public RobotCode _ActualCode;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }
    public void _InsertCode(string StatementName) {
        //verificar se tem como inserir
        if (Cursor.transform.parent.childCount == 2) {
            Debug.Log("Statement full, não tem como inserir");
            return;
        }
        //Verificar se o statement passado é valido

        //Inserir na UI
        GameObject newStatementUI = Instantiate(_UIPrefabAndarAte, Cursor.transform.parent.transform);
        StatementHolder SH = newStatementUI.GetComponentInChildren<StatementHolder>();
        if (SH != null) {
            Debug.Log("selecionando o novo botaum");
            SH._CodeManager = this;
            SH._SelectForReal();
        }
        if (_ActualCode != null) {
            
        }
    }
}