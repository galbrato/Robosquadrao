using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;   

public class RobotCode : MonoBehaviour {
    public string myName = "Rola";
    public int myID = -1;

    private Rigidbody rigid;
    private NavMeshAgent agent;

    //Lista de variaveis, "Memoria" do robo
    public List<Variavel> VarList;
    public Variavel Retorno;
    
    //Variaveis do Globais
    public List<RobotCode> Aliados;
    public List<RobotCode> Inimigos;
    public Vector3 Objetivo;
    public Vector3 Inicio;
    public Animator Anima;
    public Animator DamageAnimator;
    public Transform LaserPos;
    public Transform Alvo;
    public GameObject LaserBeam;

    //Codigo do robo
    public List<Statement> Code;
    private int ProgramCounter;
    
    //Atributos do robo
    public float Speed = 10.0f;
    public float VidaMax =10;
    public float VidaAtual =10;
    public float Dano =1;
    public float AtackDelay=1;
    public float DanoHeal = 1;
    public float HealDelay = 1;
    public float DanoLaser = 1;
    public float LaserDelay = 2;
    public Image HealthBar;
    public Text nome_text;
    public AudioSource Explosion;
    public AudioClip die;
    public AudioClip heal;
    public AudioClip punch;
    public Transform chave;
    public Transform soco;
    float AtackDelayCouter;
    float HealDelayCouter;
    float LaserDelayCouter;
    float StopingDistance;
    // Informacoes do Inimigo
    private Vector3 EnemyPosition;
    //Informacoes do Aliado
    private Vector3 AllyPosition;


    public Vector3 robotPosition {     // Abstração da posição do robô
        get {
            return this.transform.position;
        }
    }
    private Vector3 robotScale {        // Abstração da escala local do robô
        get {
            return this.transform.localScale;
        }
        set {
            this.transform.localScale = value;
        }
    }


    void Start() {
        this.Objetivo = Battle.Objetivo;
        this.Inicio = Battle.Inicio;

        if (myID >= 0) {
            //Pegando os dados do robo do arquivo
            RobotData Data = SaveSystem.LoadRobot(myID);
            Code = Data.Code;
            myName = Data.Name;
        }

        //VidaAtual = VidaMax;
        AtackDelayCouter = AtackDelay;
        HealDelayCouter = HealDelay;
        LaserDelayCouter = LaserDelay;
        ProgramCounter = 0;

        VarList = new List<Variavel>();

        Anima = GetComponentsInChildren<Animator>()[0];
        DamageAnimator = GetComponentsInChildren<Animator>()[1];

        rigid = transform.GetComponent<Rigidbody>();

        agent = transform.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;   // Impede o NavMeshAgent de ficar rotacionando a sprite

        StopingDistance = agent.stoppingDistance;

        //Inicio = transform.position;
        //Objetivo = Alvo.position;
        nome_text.text = myName;
    }
    
    
    public bool acha(Variavel a) {
        return a.Label == "aaaa";
    }

    // Update is called once per frame
    void Update() {
        if (agent.enabled)
            agent.isStopped = true;
        Anima.SetBool("IsMoving", false);

        if (AtackDelayCouter < AtackDelay) AtackDelayCouter += Time.deltaTime;
        if (HealDelayCouter < HealDelay) HealDelayCouter += Time.deltaTime;
        if (LaserDelayCouter < LaserDelay) LaserDelayCouter += Time.deltaTime;
        
        Inimigos.RemoveAll((RobotCode r) => {return r == null;});
        if (myID>=0) {
            if (Code[ProgramCounter].Execute(this)) {

            } else {
                ProgramCounter = (ProgramCounter + 1) % Code.Count;
            }
        }


        if (Input.GetKeyUp(KeyCode.Space)) {
            _PrintCode();
        }

        HealthBar.fillAmount = VidaAtual / VidaMax;
    }

    void _PrintCode() {
        for (int i = 0; i < Code.Count; i++) {
            Debug.Log("Linha[" + i + "]: " + Code[i].ToString());
        }
    }

    private void FlipRobot(Vector3 actionDirection) {   // Corrige a posição na qual o robô está olhando
        float scaleX = Mathf.Abs(robotScale.x);         // Pega a escala x original do objeto (para evitar resize)
        float novoX = (actionDirection.x - robotPosition.x > 0) ? -scaleX : scaleX; // Verifica se a direção de ação está para
        // a direita ou esquerda do personagem, usando o negativo para flipar para o lado certo
        robotScale = new Vector3(novoX, robotScale.y, robotScale.z);    // Configura as escalas do robô
    }

    public bool Attack(Vector3 dir) {
        EnemyPosition = dir;
        if (AtackDelayCouter >= AtackDelay) {
            FlipRobot(dir);

            AtackDelayCouter = 0;
            Anima.SetBool("Attack", true);
            return false;
        }
        return false;
    }

    public bool Fix(Vector3 dir) {
        Debug.Log("Curaaaaaaaaaaa");
        AllyPosition = dir;
        if(HealDelayCouter >= HealDelay){
            FlipRobot(dir);

            HealDelayCouter = 0;
            Anima.SetBool("Heal", true);
            return false;
        }
        return false;
    }

    public bool Laser(Vector3 dir) {
        EnemyPosition = dir;
        if(LaserDelayCouter >= LaserDelay){
            FlipRobot(dir);
            
            LaserDelayCouter = 0;
            Anima.SetBool("Laser", true);
            return false;
        }
        return false;
    }

    public bool WalkTo(Vector3 dest) {
        if (!agent.enabled) return false;
        Vector3 movement = dest - transform.position;   // Vetor para saber o vetor movimento (para onde irá se mover)
        movement.y = 0; // Ignora a posição em Y, já que esse eixo não importa na distância do personagem

        if (movement.magnitude <= StopingDistance) { // Se já estiver perto o suficiente
            Anima.SetBool("IsMoving", false);   // Muda a animação para Idle
            //rigid.velocity = new Vector3(0,0,0);
            agent.isStopped = true;
        } else {
            agent.isStopped = false;
            agent.destination = dest;   // Caso contrário, seta o destino do agent
            Anima.SetBool("IsMoving", true);    // E a animação para movimentação

            FlipRobot(dest);
        }
        return false;
    }

    public bool TakeDamage(float damage) {
        soco.GetComponent<AudioSource>().PlayOneShot(punch);
        VidaAtual -= damage;
        if (VidaAtual <= 0) {
            Tchakabuuum();
            return true;
        }
        DamageAnimator.SetTrigger("Hitted");
        return false;
    }

    public bool TakeHeal(float cura) {
        VidaAtual += cura;
        if (VidaAtual > VidaMax) VidaAtual = VidaMax;

        DamageAnimator.SetTrigger("Healed");
        return false;
    }

    public void ApplyDamage(){
        Vector3 dir = EnemyPosition - robotPosition;
        dir = dir.normalized;
        float alcance = agent.stoppingDistance;
        Vector3 DamagePosition = robotPosition + (dir * alcance);
        Vector3 hitbox = DamagePosition;
        for(int i = 0; i < Inimigos.Count; i++){
            print("damage:" + DamagePosition + " inimigo " + Inimigos[i].name + " pos" + Inimigos[i].transform.position + " Distance : " + Vector3.Distance(DamagePosition, Inimigos[i].transform.position));
            if(Vector3.Distance(DamagePosition, Inimigos[i].transform.position) < 1.3f && !Inimigos[i].CompareTag("Untagged")) {
                if (Inimigos[i].TakeDamage(Dano)) {
                    Inimigos.Remove(Inimigos[i]);
                    i--;
                }
            }
        }
        
        Anima.SetBool("Attack", false);
    }

    public void ApplyHeal(){
        Vector3 dir = AllyPosition - robotPosition;
        dir = dir.normalized;
        float alcance = agent.stoppingDistance;
        Vector3 HealPosition = robotPosition + (dir*alcance);
        Vector3 hitbox = HealPosition;
        chave.GetComponent<AudioSource>().PlayOneShot(heal);

        for(int i = 0; i < Aliados.Count; i++){
            print("Heal:" + HealPosition + " inimigo " + Aliados[i].name + " pos" + Aliados[i].robotPosition + " Distance : " + Vector3.Distance(HealPosition, Aliados[i].robotPosition));
            if(Vector3.Distance(HealPosition, Aliados[i].robotPosition) < 1.3f) {
                Aliados[i].TakeHeal(DanoHeal);
            }
        }

        Anima.SetBool("Heal", false);
    }

    public void ApplyLaserBeam(){
    	GameObject laser = Instantiate(LaserBeam, LaserPos.position, Quaternion.identity) as GameObject;
    	laser.transform.localScale = transform.localScale;
        Laser ls = laser.GetComponent<Laser>();
    	ls.target = EnemyPosition;
        ls.mae = this;
        ls.ad = LaserPos.gameObject.GetComponent<AudioSource>();

        Anima.SetBool("Laser", false);
    }

    public void ApplyLaserDamage(Vector3 target){
        for(int i = 0; i < Inimigos.Count; i++){
            if(Vector3.Distance(target, Inimigos[i].robotPosition) < 1.3f) {
                if (Inimigos[i].TakeDamage(DanoLaser)) {
                    Inimigos.Remove(Inimigos[i]);
                    i--;
                }
            }
        }
    }

    public void Tchakabuuum(){
        Anima.enabled = false;
        agent.enabled = false;
        gameObject.layer = 0;
        this.tag = "Untagged";
        Explosion.PlayOneShot(die);
        
        SpriteRenderer[] sprites = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer piece in sprites){
            Rigidbody rig = piece.gameObject.AddComponent<Rigidbody>() as Rigidbody;
            Vector3 direction = Random.insideUnitCircle.normalized;
            float rand = Random.Range(20,50);
            rig.AddForce(direction*rand, ForceMode.Impulse);
        }
        Destroy(transform.gameObject, 2);
    }
}
