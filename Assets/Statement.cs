using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    Booleano
}

[Serializable]
public abstract class Statement {
    protected Tipo type;

    public Tipo ReturnTipo() {
        return type;
    }

    /// <summary>
    /// Função que ira executar o comando, ira retornar true enquanto ela tiver executando, e retornara false quando terminar
    /// </summary>
    /// <returns>retorna true se esta executando, false caso ja tenha terminado de executar</returns>
    public abstract bool Execute(RobotCode Robot);
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
            Debug.LogError("Parametro nulo");
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
            Debug.LogError("Parametro nulo");
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
            Debug.LogError("Parametro nulo");
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
    private string VarName;

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
            Debug.LogError("Parametro nulo");
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
        type = Tipo.Posicao;
    }
    public override bool Execute(RobotCode Robot) {
        //Limpando a Lista de variaveis
        Variavel v;
        if (Global2Return == GlobalVar.Inicio) {
            v = new VarPosicao("Inicio", Robot.Inicio);
        } else {
            v = new VarPosicao("Objetivo",Robot.Objetivo);
        }

        Robot.Retorno = v;

        return false;
    }
}

[Serializable]
public class Atacar : Statement {
    public Statement Parametro;
    public Atacar() {
        type = Tipo.Vazio;
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
            Debug.LogError("Parametro nulo");
            return false;
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro.ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if(Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
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
        Robot.Atack(new Vector3(pos.x, pos.y, pos.z));
        return false;
    }
}

[Serializable]
public class AndarAte : Statement {
    public Statement Parametro;
    public AndarAte() {
        type = Tipo.Vazio;
    }
    public AndarAte(Statement aqui) {
        Parametro = aqui;
        type = Tipo.Vazio;
    }

    public override bool Execute(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
            Debug.LogError("Parametro nulo");
        }
        //Verificando se o retorno do Parametro é do tipo certo
        if (Parametro.ReturnTipo() != Tipo.Posicao) {
            Debug.LogError("Em Atacar Argumento errado");
            return false;
        }
        //Executando o parametro
        Parametro.Execute(Robot);
        //Verificando se retorno do parametro foi passado
        if (Robot.Retorno == null) {
            Debug.LogError("Retorno Nulo");
        }
        //Verificando se o retorno é do tipo correto
        if (Robot.Retorno.type != Tipo.Posicao) {
            Debug.LogError("Retorno de tipo diferente");
        }
        //Obtendo parametro
        VarPosicao pos = (VarPosicao)Robot.Retorno;
        //Liberando o retorno
        Robot.Retorno = null;
        //Executando o comando
        Robot.WalkToo(new Vector3(pos.x, pos.y, pos.z));

        return false;
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
            Debug.LogError("Parametro nulo");
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
            Debug.LogError("Parametro nulo");
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
            Debug.LogError("Parametro nulo");
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
    public Statement Parametro;

    public List<Statement> SubCode;
    private int ProgramCounter;

    private bool Verified;
    public Se() {
        type = Tipo.Vazio;
        Verified = false;
        ProgramCounter = 0;
    }

    bool VerifyCondition(RobotCode Robot) {
        //Verificando se o Parametro foi passado
        if (Parametro == null) {
            Debug.LogError("Parametro nulo");
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
                return false;
            }
        }

        return true;
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
            Debug.LogError("Parametro nulo");
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
            Debug.LogError("Parametro nulo");
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
            Debug.LogError("Parametro nulo");
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