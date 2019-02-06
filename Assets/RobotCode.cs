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
    public Animator Anima;
    public Transform Mao;
    public Transform Chave;
    public Transform LaserPos;
    public Transform Alvo;
    public GameObject LaserBeam;

    //Codigo do robo
    public List<Statement> Code;
    private int ProgramCounter;
    
    //Atributos do robo
    public float Speed =1;
    public float VidaMax =10;
    public float VidaAtual =10;
    public float Dano =1;
    public float AtackDelay=1;
    public float DanoHeal = 1;
    public float HealDelay = 1;
    public float DanoLaser = 1;
    public float LaserDelay = 1;
    float AtackDelayCouter;
    float HealDelayCouter;
    float LaserDelayCouter;


    void Start() {
        VidaAtual = VidaMax;
        AtackDelayCouter = AtackDelay;
        HealDelayCouter = HealDelay;
        LaserDelayCouter = LaserDelay;
        ProgramCounter = 0;

        VarList = new List<Variavel>();

        Anima = GetComponent<Animator>();
        Inimigos = gameObject.transform.GetChild(10).GetComponent<Sensor>().adversarios;


        IniciarComandosBasicos();
    }

    void IniciarComandosBasicos() {
        //Code.Add(new AlocaInteiro("i",0));
        //Code.Add(new AndarAte(new Indexar(new RetornaVariavel("i"),RobotStatus.Posicao)));
        Code.Add(new AndarAte(new RetornaGlobal(GlobalVar.Objetivo)));

    }

    public bool acha(Variavel a) {
        return a.Label == "aaaa";
    }

    // Update is called once per frame
    void Update() {


        if (AtackDelayCouter < AtackDelay) AtackDelayCouter += Time.deltaTime;
        if (HealDelayCouter < HealDelay) HealDelayCouter += Time.deltaTime;
        if (LaserDelayCouter < LaserDelay) LaserDelayCouter += Time.deltaTime;

        if (Code[ProgramCounter].Execute(this)) {

        } else {
            ProgramCounter = (ProgramCounter + 1) % Code.Count;
        }
    }

    public bool Atack(Vector3 dir) {
        if (AtackDelayCouter >= AtackDelay) {
            AtackDelay = 0;
            Anima.SetBool("Attack", true);
            return false;
        }
        Anima.SetBool("Attack", false);
        return false;
    }

    public bool Fix(Vector3 dir) {
        if(HealDelayCouter >= AtackDelay){
            HealDelay = 0;
            Anima.SetBool("Heal", true);
            return false;
        }
        Anima.SetBool("Heal", false);
        return false;
    }

    public bool Laser(Vector3 dir) {
        if(LaserDelayCouter >= LaserDelay){
            LaserDelay = 0;
            Anima.SetBool("Laser", true);
            return false;
        }
        Anima.SetBool("Laser", false);
        return false;
    }
    public bool WalkToo(Vector3 dest) {
        rigid.velocity = (dest - transform.position).normalized * Speed;
        rigid.velocity = Vector3.zero;
        return false;
    }
    public void ApplyDamage(){
    	LayerMask mask = (1 << this.gameObject.layer);
        mask |= (1 << 11);
        mask = ~mask;
    	Collider[] hitColliders = Physics.OverlapBox(Mao.position, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
            print(hitColliders[0].name);
        	hitColliders[0].GetComponent<Alive>().lifes -= 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Hitted");
        }
    }

    public void ApplyHeal(){
    	LayerMask mask = 1 << this.gameObject.layer;
    	Collider[] hitColliders = Physics.OverlapBox(Chave.position, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
        	hitColliders[0].GetComponent<Alive>().lifes += 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Healed");
        }
    }

    public void ApplyLaserBeam(){
    	GameObject laser = Instantiate(LaserBeam, LaserPos.position, Quaternion.identity) as GameObject;
    	laser.transform.localScale = transform.localScale;
    	laser.GetComponent<Laser>().target = Alvo;
        laser.GetComponent<Laser>().mask = (1 << this.gameObject.layer);
        laser.GetComponent<Laser>().mask |= (1 << 11);
        laser.GetComponent<Laser>().mask = ~laser.GetComponent<Laser>().mask;
    }
}
