using System.Collections.Generic;

namespace Registration_training
{
    public class Comp_Curricular
    {
        private int codigo;
        private string nome;
        private Pre_Requisitos preRequisitos;
        private bool aprovado;

        public Comp_Curricular (int codigo, string nome, Pre_Requisitos preRequisitos)
        {
            this.codigo = codigo;
            this.nome = nome;
            this.preRequisitos = preRequisitos;
            this.aprovado = false;
        }
        
        public void SetAprovado(bool aprovado)
        {
            this.aprovado = aprovado;
        }
        public int GetCodigo()
        {
            return codigo;
        }
        public string GetNome()
        {
            return nome;
        }

        public bool Disponivel(int semestre, List<Cadeira> cadeiras)
        {
            if(!aprovado)
            {
                if(semestre >= preRequisitos.GetSemestre())
                {
                    if (preRequisitos.GetNumRequisitos() !=0)
                    {
                        int numRequisito = 0;
                        for (int i = 0; i < preRequisitos.GetNumRequisitos(); i++)
                        {
                            foreach (var c in cadeiras)
                            {
                                if(c.GetAprovacao() && c.GetNome() == preRequisitos.GetElementoCadeira(i))
                                {
                                    numRequisito++;
                                }
                            }
                            
                        }
                        if(numRequisito == preRequisitos.GetNumRequisitos())
                            {
                                return true;
                            }
                            else
                                return false;
                    }
                    else
                        return true;
                }
                return false;
            }
            
            return false;
        }
    }
}