using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CodeManager : MonoBehaviour{
    //Cursos e variaveis para fazer o cursor piscar
    public Text Cursor;
    public Transform _CodeContent;
   
    public RobotCode _ActualCode;

    public GameObject LinePrefab;

    //Lista de prfabs de codigo
    public List<GameObject> StatementsUIPrefabs;

    // Start is called before the first frame update
    void Start(){
        //_CodeContent = transform.Find("Content");
        int NumberOfLines = GameObject.Find("Line").transform.parent.childCount;
        _ActualCode.Code = new List<Statement>(NumberOfLines);
        for (int i = 0; i < NumberOfLines; i++) {
            _ActualCode.Code.Add(new Vazio());
        }
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyUp(KeyCode.R)) {
            _PutCodeOnUI();
        }
    }

    private void InsertStatment(StatementHolder SH, Statement S) {
        Debug.Log("inserindo no " + SH.name + " o " + S.ToString());
        SH._OriginalStatement = S;
        GameObject UI = StatementsUIPrefabs.Find((GameObject g) => { return S.ToString().Contains(g.name); });
        if (UI == null) {
            Debug.LogError("ERRO, Não conseguiu achar " + S.ToString() + " dentro da lsita de prefabs");
        }
        GameObject SUI = Instantiate(UI, SH.transform);
        if (S.Parametros != null) {
            StatementHolder[] SHs = SUI.GetComponentsInChildren<StatementHolder>();
            for (int i = 0; i < S.Parametros.Length; i++) {
                if (S.Parametros[i] != null) {
                    StatementHolder newS = SHs[i];
                    InsertStatment(newS, S.Parametros[i]);
                }
            }
        }
    }

    public void _PutCodeOnUI() {
        if (_ActualCode.Code.Count <= 0) {
            Debug.LogError("Erro!, Codigo vazio");
        }
        //Tirar o cursor de filho apra náo ser deletado ao limapr a interface
        Cursor.transform.SetParent(null);
        //Limpando a interface de codigo
        for (int i = 0; i < _CodeContent.childCount; i++) {
            Debug.Log(_CodeContent.name + " tem " +  _CodeContent.childCount + " deletando o " + _CodeContent.GetChild(i).gameObject.name);
            Destroy(_CodeContent.GetChild(i).gameObject);
        }
        foreach (Statement s in _ActualCode.Code) {
            GameObject NewLine = Instantiate(LinePrefab, _CodeContent);
            InsertStatment(NewLine.GetComponent<StatementHolder>(), s);
        }
    }

    public void _InsertLine() {
        Transform t = Cursor.transform.parent;
        while (!t.name.Contains("Line")) {
            t = t.parent;
        }
        _ActualCode.Code.Insert(t.GetSiblingIndex() + 1, new Vazio());
        GameObject NewLine = Instantiate(LinePrefab, t.parent);
        NewLine.transform.SetSiblingIndex(t.GetSiblingIndex() + 1);
        NewLine.name = "Line " + NewLine.transform.GetSiblingIndex();

        NewLine.GetComponent<StatementHolder>()._SelectForReal();
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
            _ActualCode.Code[SH.transform.GetSiblingIndex()] = SH._OriginalStatement;
        }
        
        //Inserir na UI
        GameObject newStatementUI = Instantiate(UIPrefab, Cursor.transform.parent.transform);
        
        //Selecionar novo botaum
        SH._SelectForReal();
        
    }

    public void _RemoveCode() {
        Transform t = Cursor.transform.parent;
        Debug.Log("Caso : "+t.name + " childs: " + t.childCount);

        if (t.name.Contains("Parameter")) {
            if (t.childCount == 2) {
                //deletar o filho do parameter (irmão do cursor)
                StatementHolder Parameter = t.GetComponent<StatementHolder>();
                StatementHolder ParameterFather = t.parent.GetComponentInParent<StatementHolder>();
                ParameterFather._OriginalStatement.Parametros[0] = null;
                Parameter._OriginalStatement = null;
                Destroy(t.GetChild(0).gameObject);
            } else if (t.childCount == 1) {
                // Deletar o que contem o parameter
                StatementHolder ParameterFather = t.parent.GetComponentInParent<StatementHolder>();

                // Deletar da lista de codigo se estiver na linha
                if (ParameterFather.name.Contains("Line")) {
                    int i = ParameterFather.transform.GetSiblingIndex();
                    //Debug.Log(ParameterFather.name + " remover" + i);
                    _ActualCode.Code[i] = new Vazio();
                } else {// Deletar da lista de parametros do pai
                    StatementHolder FatherFather = ParameterFather.transform.parent.GetComponentInParent<StatementHolder>();
                    FatherFather._OriginalStatement.Parametros[0] = null;
                }
                Cursor.transform.SetParent(ParameterFather.transform);

                //Debug.Log(Cursor.transform.name + " vo colocar como filho de " + ParameterFather.transform);
                Destroy(ParameterFather.transform.GetChild(0).gameObject);
            } else {
                Debug.LogError("ERRO, essa parte nunca deveria acotencer");
            }
        } else if (t.name.Contains("Line")) {
            if (t.childCount == 2) {
                //Linha com statement, deletar o irmão do cursor
                StatementHolder Line = t.GetComponent<StatementHolder>();
                //Removendo o statment da linah do codigo
                int i = _ActualCode.Code.IndexOf(Line._OriginalStatement);
                _ActualCode.Code[i] = new Vazio();
                //Tirando a referencia do do codigo do StatementHolder da linha
                Line._OriginalStatement = null;
                //Deletando o statement
                Destroy(t.GetChild(0).gameObject);
            } else if (t.parent.childCount == 1) {
                Debug.Log("Unica linha, não podo ser deletada");
                return;
            } else if(t.childCount == 1) {//Deletar a linha
                //pegando a nova linah do cursor
                int i = t.GetSiblingIndex() - 1;
                if (i < 0) i = t.GetSiblingIndex() + 1;
                t.parent.GetChild(i).GetComponent<StatementHolder>()._SelectForReal();
                //Removedo a linha no codigo
                _ActualCode.Code.RemoveAt(t.GetSiblingIndex());
                //Deletando a linha
                Destroy(t.gameObject);
            } else {
                Debug.LogError("ERRO, essa parte nunca deveria acotencer");
            }
        } else {
            Debug.LogError("ERRO, tentando remover4 algo que náo é statement nem linha");
        }
    }
}
