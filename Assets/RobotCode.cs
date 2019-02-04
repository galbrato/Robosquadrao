using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCode : MonoBehaviour {
    Rigidbody rigid;

    //Lista de variaveis, "Memoria" do robo
    public List<Variavel> VarList;
    public Variavel Retorno;
    
    //Variaveis do Globais
    public List<RobotCode> Aliados;
    public List<RobotCode> Inimigos;
    public Vector3 Objetivo;
    public Vector3 Inicio;

    //Codigo do robo
    public List<Statement> Code;
    private int ProgramCounter;
    
    //Atributos do robo
    public float Speed =1;
    public float VidaMax =10;
    public float VidaAtual =10;
    public float Dano =1;
    public float AtackDelay=1;
    float AtackDelayCouter;

    void Start() {
        VidaAtual = VidaMax;
        AtackDelayCouter = AtackDelay;
        ProgramCounter = 0;

        VarList = new List<Variavel>();
    }

    void IniciarComandosBasicos() {
        Code.Add(new AlocaInteiro("i", 0));
        Code.Add(new IndexarAliados(new RetornaVariavel("i"),RobotStatus.Posicao));

    }

    public bool acha(Variavel a) {
        return a.Label == "aaaa";
    }
    // Update is called once per frame
    void Update() {


        if (AtackDelayCouter < AtackDelay) AtackDelayCouter += Time.deltaTime;

        if (Code[ProgramCounter].Execute(this)) {

        } else {
            ProgramCounter = (ProgramCounter + 1) % Code.Count;
        }
    }

    public bool Atack(Vector3 dir) {
        if (AtackDelayCouter >= AtackDelay) {
            AtackDelay = 0;
            Debug.Log("Ataquei");
            return false;
        }
        return false;
    }
    public bool WalkToo(Vector3 dest) {
        rigid.velocity = (dest - transform.position).normalized * Speed;
        return false;
    }
}
