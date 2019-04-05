using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CodeManager : MonoBehaviour{
    //Cursos e variaveis para fazer o cursor piscar
    public Text Cursor;
   
    public RobotCode _ActualCode;

    //Lista de prfabs de codigo
    public List<GameObject> StatementsUIPrefabs;

    // Start is called before the first frame update
    void Start(){
        int NumberOfLines = GameObject.Find("Line").transform.parent.childCount;
        _ActualCode.Code = new List<Statement>(NumberOfLines);
        for (int i = 0; i < NumberOfLines; i++) {
            _ActualCode.Code.Add(new RetornaGlobal(GlobalVar.Objetivo));
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void _InsertCode(string StatementName) {
        //verificando se existe na lsita de prefabs
        GameObject UIPrefab = StatementsUIPrefabs.Find((GameObject vari) => { return vari.name == StatementName; });
        if (UIPrefab == null) {
            Debug.LogError("Satment " + StatementName + " não existe");
            return;
        }
        //verificar se tem como inserir
        if (Cursor.transform.parent.childCount == 2) {
            return;
        }
       

        //Pegando o Statement holder do pai do cursor para poder atribuir o novo statement para ele
        StatementHolder SH = Cursor.GetComponentInParent<StatementHolder>();
        SH._OriginalStatement = Statement.AlocByName(StatementName);
        
        //Se for um parametro, tem que isnerir no codigo "pai"
        if (SH.name == "Parameter") {
            StatementHolder SHFather = SH.transform.parent.GetComponentInParent<StatementHolder>();
            SHFather._OriginalStatement.Parametros[0] = SH._OriginalStatement;//issu vai dar merda quando tiver 2 parametros
        } else {//se não fopr parametro esta no inicio da linha insere direto no codigo
            if (!SH.name.Contains("Line")) {
                Debug.LogError("ERRO! " +name + " não é um uma linha e não é um parameter");
            }
            _ActualCode.Code[transform.GetSiblingIndex()] = SH._OriginalStatement;
        }
        
        //Inserir na UI
        GameObject newStatementUI = Instantiate(UIPrefab, Cursor.transform.parent.transform);
        
        //Selecionar novo botaum
        SH._SelectForReal();
        
    }

    public void _RemoveCode() {

    }
}
