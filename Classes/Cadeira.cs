using System.Collections.Generic;

namespace Registration_training
{
    public class Cadeira
    {
        private int codigo;
        private string nome;
        private int semestre;
        private double nota;
        private bool aprovado;

        public Cadeira()
        {
            this.codigo = 0;
            this.nome = "";
            this.semestre = 0;
            this.nota = 0.0;
            this.aprovado = false;
        }
        public Cadeira(int codigo, string nome, int semestre, double nota){
            this.codigo = codigo;
            this.nome = nome;
            this.semestre = semestre;
            this.nota = nota;
            if(nota<5.0)
                aprovado = false;
            else
                aprovado = true;
        }

        public double GetNota()
        {
            return nota;
        }

        public int GetSemestre()
        {
            return semestre;
        }

        public string GetNome()
        {
            return nome;
        }

        public int GetCodigo()
        {
            return codigo;
        }

        public bool GetAprovacao()
        {
            return aprovado;
        }
    }
}