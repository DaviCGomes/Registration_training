using System;
using System.Collections.Generic;
using Registration_training.Interfaces;

namespace Registration_training
{
    public class Aluno : Historico<Cadeira>
    {
        string nome;
        int matricula;
        int semestreAtual, semestreMaximo;
        List<Cadeira> cadeiras;

        public Aluno(string nome, int matricula, int semestreAtual)
        {
            this.nome = nome;
            this.matricula = matricula;
            this.semestreAtual = semestreAtual;
            this.semestreMaximo = 15;
            this.cadeiras = new List<Cadeira>();
        }

        public double CRE()
        {
            if(cadeiras.Count == 0)
                return 0;
            double cre = 0;
            for (int i = 0; i < cadeiras.Count; i++)
            {
                cre += cadeiras[i].GetNota();
            }
            cre = cre/cadeiras.Count;
            return cre;
        }

        public void Insere(int codigo, string nome, int semestre, double nota)
        {
            Cadeira cadeira = new Cadeira(codigo, nome, semestre, nota);
            this.cadeiras.Add(cadeira);
        }

        public List<Cadeira> ListaCompleto()
        {
            return cadeiras;
        }

        public List<Cadeira> ListaPorSemestre(int semestre)
        {
            List<Cadeira> c = new List<Cadeira>();
            
            for (int i = 0; i < this.cadeiras.Count; i++)
            {
                if(this.cadeiras[i].GetSemestre() == semestre)
                    c.Add(cadeiras[i]);
            }
            
            return c;
        }
        public Cadeira RetornaPorCodigo(int codigo)
        {
            return cadeiras[codigo];
        }

        public override string ToString()
        {
            string retorno = "";
            retorno += "Nome: " + this.nome + Environment.NewLine;
            retorno += "Matricula: " + this.matricula + Environment.NewLine;
            retorno += "Semestre Atual: " + this.semestreAtual + Environment.NewLine;
            retorno += "Semestre Máximo para termino do curso: " + this.semestreMaximo + Environment.NewLine;
            retorno += "CRE: " + CRE() + Environment.NewLine;
            retorno += "Historico:" + Environment.NewLine;
            retorno += Environment.NewLine;
            
            for (int i = 1,j = 0; i < semestreAtual; i++)
            {
                retorno += i +": " + Environment.NewLine;
                
                while (j < cadeiras.Count && cadeiras[j].GetSemestre() == i)
                {
                    retorno += cadeiras[j].GetCodigo() + " ";
                    retorno += cadeiras[j].GetNome() +" ";
                    retorno += cadeiras[j].GetAprovacao() ? "Aprovado" : "Reprovado";
                    retorno += " com nota: " + cadeiras[j].GetNota() + Environment.NewLine;
                    ++j;
                }
                retorno += Environment.NewLine;
            }
            return retorno;
        }

        public int GetSemestreAtual()
        {
            return semestreAtual;
        }
        public void SetSemestreAtual(int semestreAtual)
        {
            this.semestreAtual = semestreAtual;
        }

        public int GetSemestreMaximo()
        {
            return semestreMaximo;
        }
    }
}