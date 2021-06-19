using System.Collections.Generic;

namespace Registration_training.Interfaces
{
    public interface Historico <T>
    {
        List<T> ListaCompleto();
        List<T> ListaPorSemestre(int semestre);
        T RetornaPorCodigo(int codigo);
        void Insere(int codigo, string nome, int semestre, double nota);
        double CRE();
    }
}