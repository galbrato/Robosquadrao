using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;   

public class RobotCode : MonoBehaviour {
    public string myName = "Rola";
    public int myID = 0;

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
    public float VidaMax =10;
    public float VidaAtual =10;
    public float Dano =1;
    public float AtackDelay=1;
    public float DanoHeal = 1;
    public float HealDelay = 1;
    public float DanoLaser = 1;
    public float LaserDelay = 1;
    public Image HealthBar;
    public Text nome_text;
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
            return this.transform.parent.position;
        }
    }
    private Vector3 robotScale {        // Abstração da escala local do robô
        get {
            return this.transform.parent.localScale;
        }
        set {
            this.transform.parent.localScale = value;
        }
    }


    void Start() {
        //Pegando os dados do robo do arquivo
        RobotData Data = SaveSystem.LoadRobot(myID);
        Code = Data.Code;
        myName = Data.Name;

        VidaAtual = VidaMax;
        AtackDelayCouter = AtackDelay;
        HealDelayCouter = HealDelay;
        LaserDelayCouter = LaserDelay;
        ProgramCounter = 0;

        VarList = new List<Variavel>();

        Anima = GetComponent<Animator>();
        DamageAnimator = GetComponentsInChildren<Animator>()[1];

        rigid = transform.parent.GetComponent<Rigidbody>();

        agent = transform.parent.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;   // Impede o NavMeshAgent de ficar rotacionando a sprite

        StopingDistance = agent.stoppingDistance;

        Inicio = transform.parent.position;
        //Objetivo = Alvo.position;
        nome_text.text = myName;
        playmode = true;
    }
    
    
    public bool acha(Variavel a) {
        return a.Label == "aaaa";
    }

    // Update is called once per frame
    void Update() {

        if (AtackDelayCouter < AtackDelay) AtackDelayCouter += Time.deltaTime;
        if (HealDelayCouter < HealDelay) HealDelayCouter += Time.deltaTime;
        if (LaserDelayCouter < LaserDelay) LaserDelayCouter += Time.deltaTime;
        
        Inimigos.RemoveAll((RobotCode r) => {return r == null;});
        
        if (Code[ProgramCounter].Execute(this)) {

        } else {
            ProgramCounter = (ProgramCounter + 1) % Code.Count;
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
        AllyPosition = dir;
        if(HealDelayCouter >= AtackDelay){
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
        Vector3 movement = dest - transform.parent.position;   // Vetor para saber o vetor movimento (para onde irá se mover)
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
    bool playmode = false;
    Vector3 hitbox = Vector3.zero;
    void OnDrawGizmos() {
        if (playmode) {
            Gizmos.color = Color.red;
            //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(hitbox,0.5f);
        }
    }

    public bool TakeDamage(float damage) {
        VidaAtual -= damage;
        if (VidaAtual <= 0) {
            Tchakabuuum();
            return true;
        }
        DamageAnimator.SetTrigger("Hitted");
        return false;
    }

    public void ApplyDamage(){
        Vector3 dir = EnemyPosition - robotPosition;
        dir = dir.normalized;
        float alcance = agent.stoppingDistance;
        Vector3 DamagePosition = robotPosition + (dir * alcance);
        hitbox = DamagePosition;
        for(int i = 0; i < Inimigos.Count; i++){
            print("damage:" + DamagePosition + " inimigo " + Inimigos[i].name + " pos" + Inimigos[i].transform.position + " Distance : " + Vector3.Distance(DamagePosition, Inimigos[i].transform.position));
            if(Vector3.Distance(DamagePosition, Inimigos[i].transform.position) < 1.0f) {
                if (Inimigos[i].TakeDamage(Dano)) {
                    Inimigos.Remove(Inimigos[i]);
                    i--;
                }
            }
        }
        
        Anima.SetBool("Attack", false);
    }

    public void ApplyHeal(){
        print("APLICA HEAL POHA!");
    	LayerMask mask = 1 << this.gameObject.layer;
        Vector3 dir = AllyPosition - this.transform.position;
        dir = dir.normalized;
        Vector3 distanciaChave = this.transform.position - Chave.position;
        float alcance = distanciaChave.magnitude;
        Vector3 hitbox = (dir*alcance) + this.transform.position;
    	Collider[] hitColliders = Physics.OverlapBox(hitbox, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
        	hitColliders[0].transform.GetComponent<RobotCode>().VidaAtual += DanoHeal;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Healed");
        }
        Anima.SetBool("Heal", false);
    }

    public void ApplyLaserBeam(Vector3 target){
    	GameObject laser = Instantiate(LaserBeam, LaserPos.position, Quaternion.identity) as GameObject;
    	laser.transform.localScale = transform.localScale;
        Laser ls = laser.GetComponent<Laser>();
    	ls.target = target;
        ls.mask = (1 << this.gameObject.layer);
        ls.mask |= (1 << 11);
        ls.mask |= (1 << 0);
        ls.mask = ~ls.mask;

        Anima.SetBool("Laser", false);
    }

    public void Tchakabuuum(){
        Anima.enabled = false;
        agent.enabled = false;
        gameObject.layer = 0;
        
        SpriteRenderer[] sprites = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer piece in sprites){
            Rigidbody rig = piece.gameObject.AddComponent<Rigidbody>() as Rigidbody;
            Vector3 direction = Random.insideUnitCircle.normalized;
            float rand = Random.Range(20,50);
            rig.AddForce(direction*rand, ForceMode.Impulse);
        }
        Destroy(transform.parent.gameObject, 2);
    }
}
