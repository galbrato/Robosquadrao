using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CodeManager : MonoBehaviour{
    //Cursos e variaveis para fazer o cursor piscar
    public Text Cursor;
    private Transform CursorExParent;
    public Transform _CodeContent;
    public Transform _FuncContent;
	public InputField inputField;


    public RobotData _ActualRobot;

    public GameObject LinePrefab;

    //Lista de prfabs de codigo
    public List<GameObject> StatementsUIPrefabs;
    private List<Button> StatementsButtons;

    // Start is called before the first frame update
    void Start(){
        CursorExParent = Cursor.transform.parent;
        if (StatementsButtons == null) {
            Assossiate();
        }
    }

    private void Assossiate() {
        StatementsButtons = new List<Button>();
        foreach (GameObject Stmt in StatementsUIPrefabs) {
            Button[] buttons = _FuncContent.GetComponentsInChildren<Button>();
            foreach (Button b in buttons) {
                if (b.transform.parent.name.Contains(Stmt.name)) {
                    StatementsButtons.Add(b);
                    break;
                }
            }
        }
        if (StatementsButtons.Count == StatementsUIPrefabs.Count) {
            for (int i = 0; i < StatementsUIPrefabs.Count; i++) {
                Debug.Log(StatementsUIPrefabs[i].name + " tem como botão " + StatementsButtons[i].transform.parent.name);
            }
        } else {
            Debug.LogError("ERRO! não achou tudo");
        }
    }

    public void Contextualize() {
        if (StatementsButtons == null) Assossiate();

        //descobrir o tipo esperando
        Tipo TipoEsperado = Tipo.Invalido;
        StatementHolder SH = Cursor.GetComponentInParent<StatementHolder>();
        if (SH.name.Contains("Line")) {
            if (SH._OriginalStatement == null) {
                TipoEsperado = Tipo.Vazio;
            } 
        }else if (SH.name.Contains("Parameter")) {
            if (SH._OriginalStatement == null) {
                StatementHolder SHFather = SH.transform.parent.GetComponentInParent<StatementHolder>();
                if (SHFather._OriginalStatement.ParametrosTipos != null) {
                    TipoEsperado = SHFather._OriginalStatement.ParametrosTipos[0];
                }
            } 
        }

        //Deligar os botões
        for (int i = 0; i < StatementsButtons.Count; i++) {
            Statement s = Statement.AlocByName(StatementsUIPrefabs[i].name);
            StatementsButtons[i].interactable = (s.ReturnTipo() == TipoEsperado) ;
        }
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyUp(KeyCode.R)) {
            _PutCodeOnUI();
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            _PrintCode();
        }
        
    }

    public void _LoadRobot(int ID) {
        try {
            _ActualRobot = SaveSystem.LoadRobot(ID);
        } catch (System.IO.FileNotFoundException) {
            Debug.Log("Robo " + ID + " inexistente criando novo");
            _ActualRobot = new RobotData(ID);
        }
    }

    public void _SaveRobot() {
        if (_ActualRobot != null) {
            _ActualRobot.Name = inputField.text;
            SaveSystem.SaveRobot(_ActualRobot);
        }
    }

    public void _StartEditRobot(int robotID) {
        gameObject.SetActive(true);
        _LoadRobot(robotID);
        _PutCodeOnUI();
        inputField.text = _ActualRobot.Name;
    }

    private void InsertStatment(StatementHolder SH, Statement S) {
        if (S.ToString().Contains("Vazio")) {
            return;
        }
        SH._OriginalStatement = S;
        GameObject UI = StatementsUIPrefabs.Find((GameObject g) => { return S.name.Contains(g.name); });
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
        //Tirar o cursor de filho apra náo ser deletado ao limapr a interface
        Cursor.transform.parent = null;

        //Limpando a interface de codigo
        for (int i = 0; i < _CodeContent.childCount; i++) {
            Destroy(_CodeContent.GetChild(i).gameObject);
        }
        _CodeContent.DetachChildren();

        if (_ActualRobot.Code.Count <= 0) {
            Debug.LogError("Codigo vazio");
        } 

        // Inserindo o codigo na interface
        StatementHolder aux = new StatementHolder();
        foreach (Statement s in _ActualRobot.Code) {
            GameObject NewLine = Instantiate(LinePrefab, _CodeContent);
            aux = NewLine.GetComponent<StatementHolder>();
            InsertStatment(aux, s);
        }
        if(aux != null)aux._Select();
    }

    public void _InsertLine() {
        Transform t = Cursor.transform.parent;
        while (!t.name.Contains("Line")) {
            t = t.parent;
        }
        _ActualRobot.Code.Insert(t.GetSiblingIndex() + 1, new Vazio());
        GameObject NewLine = Instantiate(LinePrefab, t.parent);
        NewLine.transform.SetSiblingIndex(t.GetSiblingIndex() + 1);
        NewLine.name = "Line " + NewLine.transform.GetSiblingIndex();

        NewLine.GetComponent<StatementHolder>()._Select();
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
            if (SHFather._OriginalStatement.ParametrosTipos[0] != SH._OriginalStatement.ReturnTipo()) {
                Debug.LogError(SHFather._OriginalStatement.name + " espera argumento do tipo " + SHFather._OriginalStatement.ParametrosTipos[0]+ " foi passado " + SH._OriginalStatement.ReturnTipo());
                SH._OriginalStatement = null;
                return;
            }
            SHFather._OriginalStatement.Parametros[0] = SH._OriginalStatement;//issu vai dar merda quando tiver 2 parametros
        } else {//se não fopr parametro esta no inicio da linha insere direto no codigo
            if (!SH.name.Contains("Line")) {
                Debug.LogError("ERRO! " +name + " não é um uma linha e não é um parameter");
                return;
            }
            if (SH._OriginalStatement.ReturnTipo() != Tipo.Vazio) {
                SH._OriginalStatement = null;
                Debug.LogError("ERRO! achamando uma func que não retorna vazio");
                return;
            }
            _ActualRobot.Code[SH.transform.GetSiblingIndex()] = SH._OriginalStatement;
        }
        
        //Inserir na UI
        GameObject newStatementUI = Instantiate(UIPrefab, Cursor.transform.parent.transform);
        
        //Selecionar novo botaum
        SH._Select();
        
    }

    public void _RemoveCode() {
        Transform t = Cursor.transform.parent;

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
                    ParameterFather._OriginalStatement = null;
                    _ActualRobot.Code[i] = new Vazio();
                } else {// Deletar da lista de parametros do pai
                    StatementHolder FatherFather = ParameterFather.transform.parent.GetComponentInParent<StatementHolder>();
                    FatherFather._OriginalStatement.Parametros[0] = null;
                }
                Cursor.transform.SetParent(ParameterFather.transform);

                Destroy(ParameterFather.transform.GetChild(0).gameObject);
            } else {
                Debug.LogError("ERRO, essa parte nunca deveria acotencer");
            }
        } else if (t.name.Contains("Line")) {
            if (t.childCount == 2) {
                //Linha com statement, deletar o irmão do cursor
                StatementHolder Line = t.GetComponent<StatementHolder>();
                //Removendo o statment da linah do codigo
                int i = _ActualRobot.Code.IndexOf(Line._OriginalStatement);
                _ActualRobot.Code[i] = new Vazio();
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
                t.parent.GetChild(i).GetComponent<StatementHolder>()._Select();
                //Removedo a linha no codigo
                _ActualRobot.Code.RemoveAt(t.GetSiblingIndex());
                //Deletando a linha
                Destroy(t.gameObject);
            } else {
                Debug.LogError("ERRO, essa parte nunca deveria acotencer");
            }
        } else {
            Debug.LogError("ERRO, tentando remover4 algo que náo é statement nem linha");
        }
        Contextualize();
    }

    void _PrintCode() {
        for (int i = 0; i < _ActualRobot.Code.Count; i++) {

            Debug.Log("Linha[" + i + "]: " + _ActualRobot.Code[i].ToString());
        }
    }

}

[System.Serializable]
public class RobotData {
    public int Id;
    public string Name;
    public List<Statement> Code;

    public RobotData(RobotCode robot) {
        Name = robot.myName;
        Id = robot.myID;
        Code = robot.Code;
    }

    public RobotData(int Id) {
        this.Id = Id;
        Name = "Robo";
        Code = new List<Statement>();
        Code.Add(new Vazio());
    }
}

