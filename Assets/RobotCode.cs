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
    // Informacoes do Inimigo
    private Vector3 EnemyPosition;
    //Informacoes do Aliado
    private Vector3 AllyPosition;


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


        //IniciarComandosBasicos();
    }


    public bool acha(Variavel a) {
        return a.Label == "aaaa";
    }

    // Update is called once per frame
    void Update() {
        Vector3 temp = transform.position;
        temp.z = temp.y;
        transform.position = temp;

        if (AtackDelayCouter < AtackDelay) AtackDelayCouter += Time.deltaTime;
        if (HealDelayCouter < HealDelay) HealDelayCouter += Time.deltaTime;
        if (LaserDelayCouter < LaserDelay) LaserDelayCouter += Time.deltaTime;

    //    if (Code[ProgramCounter].Execute(this)) {

      //  } else {
        //    ProgramCounter = (ProgramCounter + 1) % Code.Count;
       // }
    }

    public bool Attack(Vector3 dir) {
        EnemyPosition = dir;
        if (AtackDelayCouter >= AtackDelay) {
            if (this.transform.position.x >= EnemyPosition.x) {
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            } else {
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }

            AtackDelayCouter = 0;
            Anima.SetBool("Attack", true);
            return false;
        }
        return false;
    }

    public bool Fix(Vector3 dir) {
        AllyPosition = dir;
        if(HealDelayCouter >= AtackDelay){
            if (this.transform.position.x >= AllyPosition.x) {
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            } else {
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }

            HealDelayCouter = 0;
            Anima.SetBool("Heal", true);
            return false;
        }
        return false;
    }

    public bool Laser(Vector3 dir) {
        EnemyPosition = dir;
        if(LaserDelayCouter >= LaserDelay){
            if (this.transform.position.x >= EnemyPosition.x) {
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            } else {
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            
            LaserDelayCouter = 0;
            Anima.SetBool("Laser", true);
            return false;
        }
        return false;
    }
    public bool WalkTo(Vector3 dest) {
        Debug.Log("Andar até " + dest);
        Vector3 movement = dest - transform.position;
        if (movement.magnitude <= StopingDistance) {
            this.GetComponent<Animator>().SetBool("IsMoving", false);
            rigid.velocity = Vector3.zero;
        } else {
            movement= movement.normalized;
            //movement.y = movement.z;
            rigid.velocity = (movement * Speed);
            print("Movement: " + movement);
            print("Speed: " + Speed);
            print("rigid.velocity: " + rigid.velocity);
            this.GetComponent<Animator>().SetBool("IsMoving", true);
            if (rigid.velocity.x <= 0) {
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            } else {
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }        
        return false;
    }

    public void ApplyDamage(){
    	LayerMask mask = (1 << this.gameObject.layer);
        mask |= (1 << 11);
        mask = ~mask;
        Vector3 dir = EnemyPosition - this.transform.position;
        dir = dir.normalized;
        Vector3 distanciaMao = this.transform.position - Mao.position;
        float alcance = distanciaMao.magnitude;
        Vector3 hitbox = (dir*alcance) + this.transform.position;
    	Collider[] hitColliders = Physics.OverlapBox(hitbox, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
            print(hitColliders[0].name);
        	hitColliders[0].transform.GetComponent<RobotCode>().VidaAtual -= Dano;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Hitted");
        }
        Anima.SetBool("Attack", false);
    }

    public void ApplyHeal(){
    	LayerMask mask = 1 << this.gameObject.layer;
    	Collider[] hitColliders = Physics.OverlapBox(Chave.position, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
        	hitColliders[0].GetComponent<Alive>().lifes += 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Healed");
        }
        Anima.SetBool("Heal", false);
    }

    public void ApplyLaserBeam(){
    	GameObject laser = Instantiate(LaserBeam, LaserPos.position, Quaternion.identity) as GameObject;
    	laser.transform.localScale = transform.localScale;
    	laser.GetComponent<Laser>().target = Alvo;
        laser.GetComponent<Laser>().mask = (1 << this.gameObject.layer);
        laser.GetComponent<Laser>().mask |= (1 << 11);
        laser.GetComponent<Laser>().mask = ~laser.GetComponent<Laser>().mask;
        Anima.SetBool("Laser", false);
    }
}
