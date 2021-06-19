using System.Collections.Generic;

namespace Registration_training
{
    public class Pre_Requisitos
    {
        private int semestre;
        private int numRequisito;
        private List<string> cadeira;

        public Pre_Requisitos()
        {
            this.semestre = 0;
            this.numRequisito = 0;
            this.cadeira = new List<string>();
        }
        public Pre_Requisitos(int semestre, int numRequisito, List<string> cadeira)
        {
            this.semestre = semestre;
            this.numRequisito = numRequisito;
            this.cadeira = cadeira;
        }

        public int GetSemestre()
        {
            return semestre;
        }
        public List<string> GetCadeiras()
        {
            return cadeira;
        }
        public int GetNumRequisitos()
        {
            return numRequisito;
        }
        public string GetElementoCadeira(int indice)
        {
            return cadeira[indice];
        }
    }
}