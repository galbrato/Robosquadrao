﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Statement {
    protected Tipo type;
    public Statement[] Parametros;
    public Tipo[] ParametrosTipos;
    public string name;
    public Tipo ReturnTipo() {
        return type;
    }

    /// <summary>
    /// Função que ira executar o comando, ira retornar true enquanto ela tiver executando, e retornara false quando terminar
    /// </summary>
    /// <returns>retorna true se esta executando, false caso ja tenha terminado de executar</returns>
    public abstract bool Execute(RobotCode Robot);

    public static Statement AlocByName(string name) {
        if (name == "AndarAte") {
            return new AndarAte();
        } else if (name == "Objetivo") {
            return new RetornaGlobal(GlobalVar.Objetivo);
        } else if (name == "Inicio") {
            return new RetornaGlobal(GlobalVar.Inicio);
        } else if (name == "Atacar") {
            return new Atacar();
        } else if (name == "Fix") {
            return new Fix();
        } else if (name == "Vazio") {
            return new Vazio();
        } else if (name == "InimigoProximo") {
            return new InimigoProximo();
        } else if (name == "TemInimigoProximo") {
            return new TemInimigoProximo();
        } else if (name == "AliadoDanificado") {
            return new AliadoDanificado();
        } else if (name == "LaserAtaque") {
            return new LaserAtaque();
        } else if (name == "Se") {
            return new Se();
        } else if (name == "FimEntao") {
            return new FimEntao();
        } else if (name == "TemAliadoDanificado") {
            return new TemAliadoDanificado();
        } else if (name == "Nao") {
            return new Nao();
        } else if (name == "Avancar") {
            return new Avancar();
        }
        return null;
    }
}

[Serializable]
public class Vazio : Statement {

    public Vazio() {
        name = "Vazio";
        type = Tipo.Vazio;
    }

    public override bool Execute(RobotCode Robot) {
        return false;
    }
    public override string ToString() {
        return "Vazio";
    }
}

[Serializable]
public class InimigoProximo : Statement {
    public InimigoProximo() {
        name = "InimigoProximo";
        type = Tipo.Posicao;
    }
    public override bool Execute(RobotCode Robot) {
        if (Robot.Inimigos == null || Robot.Inimigos.Count == 0) {
            return false;
        }
        RobotCode escolhido = Robot.Inimigos[0];
        foreach (RobotCode a in Robot.Inimigos) {
            float DistEscolhido = Vector3.Distance(escolhido.transform.position, Robot.transform.position);
            float DistAtual = Vector3.Distance(a.transform.position, Robot.transform.position); ; 
            if ( DistAtual < DistEscolhido) {
                escolhido = a;
            }
        };
        
        Robot.Retorno = new VarPosicao(escolhido.name, escolhido.transform.position);
        return false;
    }

    public override string ToString() {
        return "InimigoProximo";
    }
}

[Serializable]
public class AliadoDanificado : Statement {
    public AliadoDanificado() {
        name = "AliadoDanificado";
        type = Tipo.Posicao;
    }
    public override bool Execute(RobotCode Robot) {
        if (Robot.Aliados == null || Robot.Aliados.Count == 0) {
            return false;
        }
        RobotCode RoboMachucado = Robot.Aliados.Find((RobotCode r) => { return (r.VidaAtual < r.VidaMax); });
        if (RoboMachucado == null) {
            Robot.Retorno = null;
            return false;
        }
        Robot.Retorno = new VarPosicao(RoboMachucado.name, RoboMachucado.transform.position);
        return false;
    }
    public override string ToString() {
        return "AliadoDanificado";
    }
}

[Serializable]
public abstract class Variavel {
    public string Label;
    public Tipo type;
   
    public static Variavel AlocateByTipo(Tipo t) {
        if (t == Tipo.Booleano) {
            return new VarBooleano("None");
        }
        if (t == Tipo.Continuo) {
            return new VarContinuo("None");
         }
        if (t == Tipo.Inteiro) {
            return new VarInteiro("None");
        }
        if (t == Tipo.Posicao) {
            return new VarPosicao("None");
        }
        Debug.LogError("ERRO, tentando AlocateByTipo vazio");
        return null;
    }

    public abstract Variavel Clone();
}

[Serializable]
public class VarInteiro : Variavel {
    public int Value;
    public VarInteiro(string lab) {
        Label = lab;
        type = Tipo.Inteiro;
    }
    public VarInteiro(string lab, int val) {
        Label = lab;
        type = Tipo.Inteiro;
        Value = val;
    }

    public override Variavel Clone() {
        return (Variavel)(new VarInteiro(Label, Value));
    }
}

[Serializable]
public class VarContinuo : Variavel {
    public float Value;
    public VarContinuo(string lab) {
        Label = lab;
        type = Tipo.Continuo;
    }
    public VarContinuo(string lab, float val) {
        Label = lab;
        type = Tipo.Continuo;
        Value = val;
    }

    public override Variavel Clone() {
        return (Variavel)(new VarContinuo(Label, Value));
    }
}

[Serializable]
public class VarPosicao : Variavel {
    public float x;
    public float y;
    public float z;
    public VarPosicao(string lab) {
        Label = lab;
        type = Tipo.Posicao;
    }
    public VarPosicao(string lab, float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
        Label = lab;
        type = Tipo.Posicao;
    }

    public VarPosicao(string lab, Vector3 pos) {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        Label = lab;
        type = Tipo.Posicao;
    }
    public override Variavel Clone() {
        return (Variavel)(new VarPosicao(Label, x,y,z));
    }
}

[Serializable]
public class VarBooleano : Variavel {
    public bool Value;
    public VarBooleano(string lab) {
        Label = lab;
        type = Tipo.Booleano;
    }
    public VarBooleano(string lab, bool val) {
        Label = lab;
        type = Tipo.Booleano;
        Value = val;
    }

    public override Variavel Clone() {
        return (Variavel)(new VarBooleano(Label, Value));
    }
}

[Serializable]
public enum RobotStatus {
    Vida,
    Dano,
    Posicao,
}

[Serializable]
public enum Tipo {
    Vazio,
    Inteiro,
    Continuo,
    Posicao,
    Booleano,
    Invalido,
}

[Serializable]
public class AlocaInteiro : Statement {
    string ParameterLabel;
    int ParameterValue = 0;

    public AlocaInteiro() {
        
        type = Tipo.Vazio;
        ParameterLabel = "Variavel Intiero";
        ParameterValue = 0;
    }

    public AlocaInteiro(string Label, int val) {

        type = Tipo.Vazio;
        ParameterLabel = Label;
        ParameterValue = val;
    }

    public override bool Execute(RobotCode Robot) {
        if (Robot.VarList.Exists((Variavel vari) => { return vari.Label == ParameterLabel; })) {
            Debug.LogError("Usuario esta tentando alocar uma variavel que ja existe");
            return false;
        }
        Robot.VarList.Add(new VarInteiro(ParameterLabel, ParameterValue));
        return false;
    }
}

[Serializable]
public class AlocaContinuo : Statement {
    string ParameterLabel;
    float ParameterValue = 0;

    public AlocaContinuo() {
        type = Tipo.Vazio;
        ParameterLabel = "Variavel Continua";
        ParameterValue = 0f;
    }
    
    public override bool Execute(RobotCode Robot) {
        if(Robot.VarList.Exists((Variavel vari) => { return vari.Label == ParameterLabel; })) {
            Debug.LogError("Usuario esta tentando alocar uma variavel que ja existe");
            return false;
        }
        Robot.VarList.Add(new VarContinuo(ParameterLabel, ParameterValue));
        return false;
    }
}

[Serializable]
public class AlocaBooleano : Statement {
    string ParameterLabel;
    bool ParameterValue = false;

    public AlocaBooleano() {
        type = Tipo.Vazio;
        ParameterLabel = "Variavel Boleana";
        ParameterValue = false;
    }

    public override bool Execute(RobotCode Robot) {
        if (Robot.VarList.Exists((Variavel vari) => { return vari.Label == ParameterLabel; })) {
            Debug.LogError("Usuario esta tentando alocar uma variavel que ja existe");
            return false;
        }
        Robot.VarList.Add(new VarBooleano(ParameterLabel, ParameterValue));
        return false;
    }
}

[Serializable]
public class AlocaPosicao : Statement {
    public string ParameterLabel;
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    public AlocaPosicao() {
        type = Tipo.Vazio;
        ParameterLabel = "Variavel Posição";
    }

    public override bool Execute(RobotCode Robot) {
        if (Robot.VarList.Exists((Variavel vari) => { return vari.Label == ParameterLabel; })) {
            Debug.LogError("Usuario esta tentando alocar uma variavel que ja existe");
            return false;
        }
        Robot.VarList.Add(new VarPosicao(ParameterLabel, x,y,z));
        return false;
    }
}

[Serializable]
public enum ArithmeticOperator {
    soma,
    multiplicação,
    subtração,
    divisão,
    modulo,
}

[Serializable]
public class ExpressaoAritimetica : Statement {
    public Statement Parametro1;
    public Statement Parametro2;
    public ArithmeticOperator Operation;

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro1 == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro1.ReturnTipo() != Tipo.Continuo && Parametro1.ReturnTipo() != Tipo.Inteiro) {
            Debug.LogError("Em Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro1.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Continuo && Robot.Retorno.type != Tipo.Inteiro) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        float p1 = 0f;
        if (Robot.Retorno.type == Tipo.Continuo) {
            p1 = ((VarContinuo)Robot.Retorno).Value;
        } else if (Robot.Retorno.type == Tipo.Inteiro) {
            p1 = ((VarInteiro)Robot.Retorno).Value;
        } else {
            Debug.LogError("ERRO!, operações aritimeticas so com inteiro ou continuo");
        }
        //Verificando se o Parametro foi passado
        if (Parametro2 == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro2.ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro2.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        float p2 = 0f;
        if (Robot.Retorno.type == Tipo.Continuo) {
            p2 = ((VarContinuo)Robot.Retorno).Value;
        }else if ( Robot.Retorno.type == Tipo.Inteiro) {
            p2 = ((VarInteiro)Robot.Retorno).Value;
        } else {
            Debug.LogError("ERRO!, operações aritimeticas so com inteiro ou continuo");
        }

        VarContinuo r = new VarContinuo("Retorno");

        switch (Operation) {
            case ArithmeticOperator.soma:
                r.Value = p1 + p2;
                break;
            case ArithmeticOperator.multiplicação:
                r.Value = p1 * p2;
                break;
            case ArithmeticOperator.subtração:
                r.Value = p1 - p2;
                break;
            case ArithmeticOperator.divisão:
                r.Value = p1 / p2;
                break;
            case ArithmeticOperator.modulo:
                r.Value = p1 % p2;
                break;
            default:
                r.Value = p1 + p2;
                break;
        }
        Robot.Retorno = r;
        return false;
    }
}

[Serializable]
public class CastVariavel : Statement {
    public Statement Parametro;
    public Tipo CastTipo;
    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
 
            return false;
        }
        Tipo ParameterTipor = Tipo.Vazio;
        if (CastTipo == Tipo.Continuo) {
            ParameterTipor = Tipo.Inteiro;
        } else if (CastTipo == Tipo.Inteiro) {
            ParameterTipor = Tipo.Continuo;
        } else {
            Debug.LogError("Tipo não pode ser feito cast");
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro.ReturnTipo() != ParameterTipor) {
            Debug.LogError("Em atribuindo tipo diferente a uma variavel");
            return false;
        }
        //Executando o parametro
        Parametro.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != ParameterTipor) {
            Debug.LogError("Retorno de tipo diferente, esta faltando cast?");
            return false;
        }
        Variavel var;
        if (CastTipo == Tipo.Continuo) {
            var = new VarContinuo("CastReturn", ((VarInteiro)Robot.Retorno).Value);
        } else {
            var = new VarInteiro("CastReturn", (int)((VarContinuo)Robot.Retorno).Value);
        }
        Robot.Retorno = var;

        return false;
    }
}

[Serializable]
public class AtribuiVariavel : Statement {
    public Statement Parametro;
    public string VarName;

    public AtribuiVariavel(Statement arg, string VarLabel) {
        Parametro = arg;
        VarName = VarLabel;
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o nome da variavel é nulo
        if (VarName == null) {
            Debug.LogError("ERRO, nome da variavel nulo");
            return false;
        }
        //Achando a variavel destino
        Variavel v = Robot.VarList.Find((Variavel vari) => { return vari.Label == VarName; });
        if (v == null) {
            Debug.LogError("ERRO! não foi possivel achar a variavel na memoria");
            return false;
        }
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro.ReturnTipo() != v.type) {
            Debug.LogError("Em atribuindo tipo diferente a uma variavel, esta faltando cast?");
            return false;
        }
        //Executando o parametro
        Parametro.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != v.type) {
            Debug.LogError("Retorno de tipo diferente, esta faltando cast?");
            return false;
        }
        
        //Atribuindo parametro
        Robot.Retorno.Label = v.Label;
        v = Robot.Retorno.Clone();

        return false;
    }
}

[Serializable]
public class RetornaVariavel : Statement {
    private string VarName;

    public RetornaVariavel(string label) {
        VarName = label;
    }

    public override bool Execute(RobotCode Robot) {
        if (VarName == null) {
            Debug.LogError("ERRO, nome da variavel nulo");
        }
        //Limpando a Lista de variaveis
        Variavel v = Robot.VarList.Find((Variavel vari) => { return vari.Label == VarName; });
        if (v == null) {
            Debug.LogError("ERRO! não foi possivel achar a variavel na memoria");
            return false;
        }
        Robot.Retorno = v.Clone();

        return false;
    }
}

[Serializable]
public enum GlobalVar {
    Inicio,
    Objetivo,
}
[Serializable]
public class RetornaGlobal : Statement {
    public GlobalVar Global2Return;
    public RetornaGlobal(GlobalVar gvar) {
        Global2Return = gvar;
        switch (Global2Return) {
            case GlobalVar.Inicio:
                name = "Inicio";
                type = Tipo.Posicao;
                break;
            case GlobalVar.Objetivo:
                type = Tipo.Posicao;
                name = "Objetivo";
                break;
            default:
                break;
        }
        type = Tipo.Posicao;
    }
    public override bool Execute(RobotCode Robot) {
        //Limpando a Lista de variaveis
        Variavel v;
        if (Global2Return == GlobalVar.Inicio) {
            v = new VarPosicao("Inicio", Robot.Inicio);
        } else {
            v = new VarPosicao("Objetivo", Robot.Objetivo);
        }

        Robot.Retorno = v;

        return false;
    }

    public override string ToString() {
        switch (Global2Return) {
            case GlobalVar.Inicio:
                return "Inicio";
            case GlobalVar.Objetivo:
                return "Objetivo";
            default:
                break;
        }
        return "ERRO!";
    }
}

[Serializable]
public class Atacar : Statement {
    private float TimeCounter = 0f;
    public Atacar() {
        name = "Atacar";
        type = Tipo.Vazio;
        Parametros = new Statement[1];
        ParametrosTipos = new Tipo[1];
        ParametrosTipos[0] = Tipo.Posicao;
    }

    public override bool Execute(RobotCode Robot) {
        if (Parametros[0] == null) return false;
        if (Parametros[0].ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametros[0].Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if(Robot.Retorno == null) {
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        VarPosicao pos = (VarPosicao)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;
        //Executando o comando
        Robot.Attack(new Vector3(pos.x, pos.y, pos.z));

        TimeCounter += Time.deltaTime;
        if (TimeCounter > Robot.AtackDelay) {
            TimeCounter = 0f;
            return false;
        }

        return true;
    }

    public override string ToString() {
        if (Parametros[0] != null) return "Atacar(" + Parametros[0].ToString() + ")";
        else return "Atacar(NULL)";
    }
}

[Serializable]
public class Fix : Statement {
    private float TimeCounter = 0f;
    public Fix() {
        name = "Fix";
        type = Tipo.Vazio;
        Parametros = new Statement[1];
        ParametrosTipos = new Tipo[1];
        ParametrosTipos[0] = Tipo.Posicao;
    }

    public override bool Execute(RobotCode Robot) {
        if (Parametros[0] == null) return false;

        if (Parametros[0].ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em fix Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametros[0].Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
             
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        VarPosicao pos = (VarPosicao)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;
        //Executando o comando
        Robot.Fix(new Vector3(pos.x, pos.y, pos.z));

        TimeCounter += Time.deltaTime;
        if (TimeCounter > Robot.HealDelay) {
            TimeCounter = 0f;
            return false;
        }

        return true;
    }

    public override string ToString() {
        if (Parametros[0] != null) return "Reparar" + Parametros[0].ToString() + ")";
        else return "Reparar(NULL)";
    }
}

[Serializable]
public class LaserAtaque : Statement {
    private float TimeCounter=0f;
    public LaserAtaque() {
        name = "LaserAtaque";
        type = Tipo.Vazio;
        Parametros = new Statement[1];
        ParametrosTipos = new Tipo[1];
        ParametrosTipos[0] = Tipo.Posicao;
    }

    public override bool Execute(RobotCode Robot) {
        if (Parametros[0] == null) return false;

        if (Parametros[0].ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametros[0].Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
             
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        VarPosicao pos = (VarPosicao)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;
        //Executando o comando
        Robot.Laser(new Vector3(pos.x, pos.y, pos.z));

        TimeCounter += Time.deltaTime;
        if (TimeCounter > Robot.LaserDelay) {
            TimeCounter = 0;
            return false;
        }

        return true ;
    }

    public override string ToString() {
        if (Parametros[0] != null) return "LaserAtaque(" + Parametros[0].ToString() + ")";
        else return "LaserAtaque(NULL)";
    }
}

[Serializable]
public class AndarAte : Statement {
    public AndarAte() {
        name = "AndarAte";
        type = Tipo.Vazio;
        this.Parametros = new Statement[1];
        this.ParametrosTipos = new Tipo[1];
        this.ParametrosTipos[0] = Tipo.Posicao;
    }


    public override string ToString() {
        if (Parametros[0] != null) return "AndarAte(" + Parametros[0].ToString() + ")";
        else return "AndarAte(NULL)";
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametros[0] == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametros[0].ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametros[0].Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        VarPosicao pos = (VarPosicao)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;
        //Executando o comando
        return !Robot.WalkTo(new Vector3(pos.x, pos.y, pos.z));   
    }
}

[Serializable]
public enum ListTipo {
    Inimigos,
    Alidados,
}

[Serializable]
public class Indexar : Statement {
    public Statement Parametro;
    public ListTipo ListToIndex;
    public RobotStatus StatusToReturn;

    public Indexar() {
        type = Tipo.Vazio;
    }

    
    public Indexar(Statement s, RobotStatus st) {
        StatusToReturn = st;
        if (st == RobotStatus.Posicao) {
            type = Tipo.Posicao;
        } else {
            type = Tipo.Continuo;
        }
        Parametro = s;

    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro.ReturnTipo() != Tipo.Inteiro) {
            Debug.LogError("Em indexar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Inteiro) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        VarInteiro index = (VarInteiro)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;

        List<RobotCode> RobotList;
        if (ListToIndex == ListTipo.Alidados) {
            RobotList = Robot.Aliados;
        } else {
            RobotList = Robot.Inimigos;
        }

        //Executando o comando
        switch (StatusToReturn) {
            case RobotStatus.Vida:
                float vida = RobotList[index.Value].VidaAtual;
                Robot.VarList.Add(new VarContinuo("Retorno", vida));
                break;
            case RobotStatus.Dano:
                float dano = RobotList[index.Value].Dano;
                Robot.VarList.Add(new VarContinuo("Retorno", dano));
                break;
            case RobotStatus.Posicao:
                Vector3 pos = RobotList[index.Value].transform.position;
                Robot.VarList.Add(new VarPosicao("Retorno", pos));
                break;
            default:
                break;
        }
        Robot.VarList.Remove(index);

        return false;
    }
}

[Serializable]
public enum CompareOperator {
    Igual,
    Diferente,
    Maior,
    MaiorIgual,
    Menor,
    MenorIgual,
}

[Serializable]
public class Compare : Statement {
    public Statement Parametro1;
    public Statement Parametro2;
    public CompareOperator Operation;

    public Compare() {
        type = Tipo.Booleano;
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro1 == null) {
 
            return false;
        }
        //Executando o parametro
        Parametro1.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        bool BoolFlag = false;//boleano que diara se a operação é com boleanos, false se for inteiro e continuo
        //Obtendo parametro
        float p1 = 0f;
        if (Robot.Retorno.type == Tipo.Continuo) {
            p1 = ((VarContinuo)Robot.Retorno).Value;
        } else if (Robot.Retorno.type == Tipo.Inteiro) {
            p1 = ((VarInteiro)Robot.Retorno).Value;
        } else if (Robot.Retorno.type == Tipo.Booleano) {
            BoolFlag = true;
            if (((VarBooleano)Robot.Retorno).Value) {
                p1 = 1;
            } else {
                p1 = 0;
            }
        } else {
            Debug.LogError("ERRO!, operações comparação so com inteiro ou continuo talvez booleano");
        }
        //Verificando se o Parametro foi passado
        if (Parametro2 == null) {
 
            return false;
        }
        
        //Executando o parametro
        Parametro2.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
       
        float p2 = 0f;
        if (Robot.Retorno.type == Tipo.Continuo && !BoolFlag) {
            p2 = ((VarContinuo)Robot.Retorno).Value;
        } else if (Robot.Retorno.type == Tipo.Inteiro && !BoolFlag) {
            p2 = ((VarInteiro)Robot.Retorno).Value;
        } else if (Robot.Retorno.type == Tipo.Booleano && BoolFlag) {
            if (((VarBooleano)Robot.Retorno).Value) {
                p2 = 1;
            } else {
                p2 = 0;
            }
        } else {
            Debug.LogError("ERRO!, operações comparação so com tipos difentes");
        }

        VarBooleano r = new VarBooleano("Retorno");
        if (BoolFlag) {
            switch (Operation) {
                case CompareOperator.Igual:
                    r.Value = (p1 == p2);
                    break;
                case CompareOperator.Diferente:
                    r.Value = (p1 != p2);
                    break;
                default:
                    Debug.LogError("ERRO! comparação entre boleanos somente igual ou diferente");
                    break;
            }
        } else {
            switch (Operation) {
                case CompareOperator.Igual:
                    r.Value = (p1 == p2);
                    break;
                case CompareOperator.Diferente:
                    r.Value = (p1 != p2);
                    break;
                case CompareOperator.Maior:
                    r.Value = (p1 > p2);
                    break;
                case CompareOperator.MaiorIgual:
                    r.Value = (p1 >= p2);
                    break;
                case CompareOperator.Menor:
                    r.Value = (p1 <= p2);
                    break;
                case CompareOperator.MenorIgual:
                    r.Value = (p1 <= p2);
                    break;
                default:
                    break;
            }
        }
        Robot.Retorno = r;
        return false;
    }
}

[Serializable]
public class Se : Statement {
    public Se() {
        type = Tipo.Vazio;
        name = "Se";
        ParametrosTipos = new Tipo[1];
        ParametrosTipos[0] = Tipo.Booleano;
        Parametros = new Statement[1];
    }

    bool VerifyCondition(RobotCode Robot) {
        //Executando o parametro
        Parametros[0].Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            //Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Booleano) {
            Debug.LogError("Retorno de tipo diferente");
            //return false;
        }
        //Obtendo parametro
        bool boo = ((VarBooleano)Robot.Retorno).Value;
        //Liberando o retorno
        Robot.Retorno = null;
        return boo;
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametros == null || Parametros[0] == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametros[0].ReturnTipo() != Tipo.Booleano) {
            Debug.LogError("Em Se Argumento errado");
            return false;
        }
        if (!VerifyCondition(Robot)) {
            //goto linha do FimEntão
            int SeQuantidade = 0;
            for (int i = Robot.ProgramCounter; i < Robot.Code.Count; i++) {
                if (Robot.Code[i].name == "Se") SeQuantidade++;

                if (Robot.Code[i].name == "FimEntao") SeQuantidade--;

                if (SeQuantidade == 0) {
                    Robot.ProgramCounter = i;
                    return false;
                }
            }
            if (SeQuantidade > 0) {
                //Debug.Log("O if não fecha o escopo certo ignorar TUDO PARA BAIXO");
                Robot.ProgramCounter = Robot.Code.Count-1;
            } else {
                Debug.LogError("Algo deu errado");
            }
        }
        return false;
    }

    public override string ToString() {
        if (Parametros[0] != null) {
            return "Se(" + Parametros[0].ToString() + ") Entao:";
        }
        return "Se(NULL) Então:";
    }
}

[Serializable]
public class FimEntao : Statement {
    public FimEntao() {
        name = "FimEntao";
        type = Tipo.Vazio;
    }

    public override bool Execute(RobotCode Robot) {
        return false;
    }

    public override string ToString() {
        return "FimEntao";
    }
}

[Serializable]
public class Enquanto : Statement {
    public Statement Parametro;

    public List<Statement> SubCode;
    private int ProgramCounter;

    private bool Verified;
    public Enquanto() {
        type = Tipo.Vazio;
        Verified = false;
        ProgramCounter = 0;
    }

    bool VerifyCondition(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro.ReturnTipo() != Tipo.Booleano) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Booleano) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        bool boo = ((VarBooleano)Robot.Retorno).Value;
        //Liberando o retorno
        Robot.Retorno = null;
        return boo;
    }

    public override bool Execute(RobotCode Robot) {

        if (!Verified) {
            Verified = true;
            if (!VerifyCondition(Robot)) {
                return false;
            }
        }

        if (!SubCode[ProgramCounter].Execute(Robot)) {
            ProgramCounter++;
            //Se o PC for igual ao numero de comandos, o codigo ja chegou no fim, e pode ser retornado;
            if (ProgramCounter == SubCode.Count) {
                Verified = false;
                ProgramCounter = 0;
            }
        }

        return true;
    }
}

[Serializable]
public enum LogicOperator {
    e,
    ou,
    nao,
}

[Serializable]
public class ExpressaoLogica : Statement {
    public Statement Parametro1;
    public Statement Parametro2;
    public LogicOperator Operation;

    public ExpressaoLogica() {
        type = Tipo.Booleano;
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro1 == null) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro1.ReturnTipo() != Tipo.Booleano) {
            Debug.LogError("Em Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro1.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Booleano) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        bool p1 = false;
        if (Robot.Retorno.type == Tipo.Booleano) {
            p1 = ((VarBooleano)Robot.Retorno).Value;
        } else {
            Debug.LogError("ERRO!, operações logica so com booleano");
        }
        //Verificando se o Parametro foi passado
        if (Parametro2 == null && Operation != LogicOperator.nao) {
 
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro2.ReturnTipo() != Tipo.Booleano) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        bool p2 = false;
        //Executando o parametro
        if (Operation != LogicOperator.nao) {
            Parametro2.Execute(Robot);

            //Verificando se retorno do parametro foi passado
            if (Robot.Retorno == null) {
                Debug.LogError("Retorno Nulo");
                return false;
            }
            //Verificando se o retorno é do tipo correto
            if (Robot.Retorno.type != Tipo.Booleano) {
                Debug.LogError("Retorno de tipo diferente");
                return false;
            }
            
            if (Robot.Retorno.type == Tipo.Booleano) {
                p2 = ((VarBooleano)Robot.Retorno).Value;
            } else {
                Debug.LogError("ERRO!, operações logica so com booleano");
            }
        }
        
        VarBooleano r = new VarBooleano("Retorno");
        switch (Operation) {
            case LogicOperator.e:
                r.Value = (p1 && p2);
                break;
            case LogicOperator.ou:
                r.Value = (p1 || p2);
                break;
            case LogicOperator.nao:
                r.Value = !p1;
                break;
            default:
                break;
        }
        return false;
    }
}

[Serializable]
public class Nao : Statement {
    public Nao() {
        type = Tipo.Booleano;
        name = "Nao";
        Parametros = new Statement[1];
        ParametrosTipos = new Tipo[1];
        ParametrosTipos[0] = Tipo.Booleano;
    }

    public override bool Execute(RobotCode Robot) {
        if (Parametros[0] == null) {
            Debug.LogError("Erro! sem parametros");
            return false;
        }
        Parametros[0].Execute(Robot);
        if (Robot.Retorno == null) {
            Robot.Retorno = new VarBooleano("Nao", true);
            return false;
        }
        
        if (Robot.Retorno.type != ParametrosTipos[0]) {
            Debug.LogError("ERRO! Tipo retornado diferente de Boleano");
            return false;
        }
        VarBooleano r = (VarBooleano)Robot.Retorno;
        r.Value = !r.Value;
        Robot.Retorno = r;
        return false;
    }

    public override string ToString() {
        if (Parametros[0] != null)
            return "Não " + Parametros[0].ToString();
        return "Não NULL";
    }
}

[Serializable]
public class TemAliadoDanificado : Statement {
    public TemAliadoDanificado() {
        name = "TemAliadoDanificado";
        type = Tipo.Booleano;
    }
    public override bool Execute(RobotCode Robot) {
        if (Robot.Aliados == null || Robot.Aliados.Count == 0) {
            Debug.Log("Eu não tenho amiguinhus :(");
            return false;
        }
        bool ThereIs = Robot.Aliados.Exists((RobotCode r) => { return (r.VidaAtual < r.VidaMax); });
        
        Robot.Retorno = new VarBooleano("RoboMachucado", ThereIs);
        return false;
    }
    public override string ToString() {
        return "TemAliadoDanificado";
    }
}


[Serializable]
public class TemInimigoProximo : Statement {
    public TemInimigoProximo() {
        name = "TemInimigoProximo";
        type = Tipo.Booleano;
    }
    public override bool Execute(RobotCode Robot) {
        if (Robot.Inimigos == null ) {
            return false;
        }
        if (Robot.Inimigos.Count > 0) {
            Robot.Retorno = new VarBooleano("Tem mais que um inimigo", true);
        } else {
            Robot.Retorno = new VarBooleano("Não vejo inimigo", false);
        }
        return false;
    }

    public override string ToString() {
        return "TemInimigoProximo";
    }
}


[Serializable]
public class Avancar : Statement {
    private float Duration = 1;
    private float TimeCounter;
    public Avancar() {
        name = "Avancar";
        type = Tipo.Vazio;
        this.Parametros = new Statement[1];
        this.ParametrosTipos = new Tipo[1];
        this.ParametrosTipos[0] = Tipo.Posicao;
    }


    public override string ToString() {
        if (Parametros[0] != null) return "Avancar(" + Parametros[0].ToString() + ")";
        else return "Avancar(NULL)";
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametros[0] == null) {
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametros[0].ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Avancar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametros[0].Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.Log("Retorno Nulo, skipando");
            return false;
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
            return false;
        }
        //Obtendo parametro
        VarPosicao pos = (VarPosicao)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;
        //Executando o comando
        bool retorno = !Robot.WalkTo(new Vector3(pos.x, pos.y, pos.z));

        TimeCounter += Time.deltaTime;
        if (TimeCounter > Duration) {
            TimeCounter = 0;
            return false;
        }

        return retorno;
    }
}
