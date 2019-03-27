using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotCode : MonoBehaviour {
    private Rigidbody rigid;
    private NavMeshAgent agent;
    private int side = 1;   // Marca para qual lado o robô está virado

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
    public float Speed = 10.0f;
    public float StopingDistance = 0.1f;
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
        Code = new List<Statement>();

        Anima = GetComponent<Animator>();
        Inimigos = gameObject.transform.GetChild(10).GetComponent<Sensor>().adversarios;
        rigid = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;   // Impede o NavMeshAgent de ficar rotacionando a sprite


        IniciarComandosBasicos();
    }

    void IniciarComandosBasicos() {
        //Code.Add(new AlocaInteiro("i",0));
        //Code.Add(new AndarAte(new Indexar(new RetornaVariavel("i"),RobotStatus.Posicao)));
        Code.Add(new AndarAte(new RetornaGlobal(GlobalVar.Objetivo)));

    }

    void ComandosBasicos2() {
        Se se = new Se();
        Compare comp = new Compare();
        comp.Operation = CompareOperator.Igual;
        //comp.Parametro1 = new RetornaVariavel();
        se.Parametro = comp;

        //Code.Add()
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
        Debug.Log("Destino: " + dest);
        Vector3 movement = dest - transform.position;   // Vetor para saber o vetor movimento (para onde irá se mover)
        movement.y = 0; // Ignora a posição em Y, já que esse eixo não importa na distância do personagem

        if (movement.magnitude <= StopingDistance) { // Se já estiver perto o suficiente
            Debug.Log("Parando");
            this.GetComponent<Animator>().SetBool("IsMoving", false);   // Muda a animação para Idle
        } else {
            Debug.Log("Movendo");
            agent.destination = dest;   // Caso contrário, seta o destino do agent
            this.GetComponent<Animator>().SetBool("IsMoving", true);    // E a animação para movimentação
            if (movement.x * side > 0) {    // Verifica se o movimento possui a mesma direção da sprite, caso contrário flipa a sprite
                side *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
            }
        }
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
