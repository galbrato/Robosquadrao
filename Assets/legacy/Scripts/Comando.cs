using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Comando{
    public string Nome;
    public string Tipo_Do_Retorno;
    public RoboBehaviour MeuRobo;

    public abstract bool Execute();
}

public class CoamndoAndarAteh : Comando {
    public Vector3 Parametro;
    override public bool Execute() {
        return MeuRobo.AndarAteh(Parametro);
    }
}
/*
public class ComandoAtacar : Comando {
    override public bool Execute() {
        return MeuRobo.AndarAteh();
    }
}

public class ComandoAlocaInteiro : Comando {
    override public bool Execute() {
        return MeuRobo.AndarAteh(V);
    }
}

public class ComandoAtribui : Comando {
    override public bool Execute() {
        return MeuRobo.AndarAteh();
    }
}



public class Inteiro {
    int Valor;
    string Rotulo;
}
*/