using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboBehaviour : MonoBehaviour{
    public float speed = 3;
    public float DistanciaDeParada = 1;

    //Lista de estados
    //Lista de transições

    //VARIAVEIS GLOBAIS QUE O ROBO TEMA CESSO
    //Lista de inimigos
    //Lista de amigos
    //Vida

    //Estruturas para guardar informação do codigo (passiveis a mudnaça ja que não existe codigo)
    //Lista de comandos
    //Lista de variaveis locais inteiras
    //Lista de variaveis locais continuas 

    private Rigidbody2D _Rigid;
    // Start is called before the first frame update
    void Start()
    {
        _Rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        //verificar transições
        //se for verdadeira limpar as memorias locais e mudar o estado
        //executar o estado
        //estado.execute();
    }

    //fuck you gringo
    public bool AndarAteh(Vector3 posição) {
        Vector3 aux = posição - transform.position;
        if (aux.magnitude < DistanciaDeParada) {
            _Rigid.velocity = Vector3.zero;
            return false;
        } else {
            _Rigid.velocity = aux.normalized * speed;
            return true;
        }
    }
        
}
