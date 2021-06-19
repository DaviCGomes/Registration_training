using System;
using System.Collections.Generic;
using System.IO;

namespace Registration_training
{
    class Program
    {
        static List<Comp_Curricular> compCurriculares = new List<Comp_Curricular>();
        static Aluno aluno;
        static int maximoMatricula = 0;

        static void Main(string[] args)
        {
            //Carrega os componentes curriculares
            CarregaCompCurriculares();
            
            //Carrega os dados e o historico
            if(!CarregaHistorico())
                //Caso não haja dados salvos, inicializa um novo cadastro
                PrimeiraEntrada();
            
            string opcaoUsuario = ObterOpcaoUsuario();


            while (opcaoUsuario.ToUpper() != "X")
            {
                switch (opcaoUsuario)
                {
                    case "1":
                        ListarCompCurricular();
                        break;
                    case "2":
                        InserirMenu();
                        break;
                    case "3":
                        VisualizarHistorico();
                        break;
                    case "4":
                        VisualizarSemestre();
                        break;
                    case "5":
                        Console.WriteLine("Seu CRE é: {0}", aluno.CRE());
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                opcaoUsuario = ObterOpcaoUsuario();
            }

            SalvarHitorico();
        }

        private static void CarregaCompCurriculares()
        {
            Console.WriteLine("Carregando os componentes curriculares.");
            StreamReader sr = new StreamReader("Instancia_Cadeiras.txt");
            string line;

            for (int i = 0; (line = sr.ReadLine()) != null; i++)
            {
                //Separa os dados da linha
                string[] infor = line.Split(' ');

                //variáveis para adicionar cada componente
                int codigo = i;
                string nome = infor[0];
                int semestre = int.Parse(infor[1]);
                int numRequisito = int.Parse(infor[2]);

                List<string> cadeira = new List<string>();
                if(numRequisito != 0)
                    for (int j = 3; j < (numRequisito + 3); j++)
                    {
                        cadeira.Add(infor[j]);
                    }
                
                Pre_Requisitos pre = new Pre_Requisitos(semestre, numRequisito,cadeira);
                Comp_Curricular comp = new Comp_Curricular(codigo, nome, pre);
                compCurriculares.Add(comp);
            }
            Console.WriteLine("Componentes curriculares carregados.");
        }

        private static bool CarregaHistorico()
        {
            Console.WriteLine("Carregando Histórico");
            StreamReader sr = new StreamReader("Historico.txt");
            string line = sr.ReadLine();

            //Verifica se o arquivo está vazio
            if(line == null)
            {
                sr.Close();
                Console.WriteLine("Historico não encontrado");
                return false;
            }
            
            //Linha informando o Nome
            string[] inforNome = line.Split(' ');
            string nome = inforNome[1];
            
            //Linha informando a Matricula
            line = sr.ReadLine();
            string[] inforMatricula = line.Split(' ');
            int matricula = int.Parse(inforMatricula[1]);

            //Linha informando o semestre atual
            line = sr.ReadLine();
            string[] inforSemestre = line.Split(' ');
            int semestreAtual = int.Parse(inforSemestre[2]);

            aluno = new Aluno (nome, matricula, semestreAtual);

            //Linha Semestre Máximo
            line = sr.ReadLine();
            //Linha CRE
            line = sr.ReadLine();
            //Linha Histórico
            line = sr.ReadLine();
            //Linha nula
            line = sr.ReadLine();

            for (int i = 1; i < semestreAtual; i++)
            {
                int semestreCursado = i;
                
                //Linha indicativa do semestre
                line = sr.ReadLine();

                for (int j = 0; (line = sr.ReadLine()) != ""; j++)
                {
                    if (line == null)
                    {
                        break;
                    }
                    string[] inforComponente = line.Split(' ');

                    int codigoComponente = int.Parse(inforComponente[0]);
                    string nomeComponente = inforComponente[1];
                    double nota = double.Parse(inforComponente[5]);

                    aluno.Insere(codigo: codigoComponente,
                                nome: nomeComponente,
                                semestre: semestreCursado,
                                nota: nota);
                    
                    if(nota >= 5.0)
                    {
                        compCurriculares[codigoComponente].SetAprovado(true);
                    }
                }
            }

            sr.Close();
            Console.WriteLine("Historico carregado");
            return true;
        }

        private static void PrimeiraEntrada()
        {
            Console.WriteLine();
            Console.WriteLine("Seja bem vindo a Universidade");
            Console.WriteLine("Esse é seu primeiro acesso ao sistema.");
            Console.WriteLine("Por favor, informe seu primeiro nome: ");
            string entradaNome = Console.ReadLine();

            Console.WriteLine("Informe a sua matricula: ");
            int entradaMatricula = int.Parse(Console.ReadLine());

            aluno = new Aluno(nome: entradaNome, matricula: entradaMatricula, semestreAtual: 1);
            Console.WriteLine("Cadastro realizado com sucesso.");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static List<int> ListarCompCurricular()
        {
            List<Comp_Curricular> opcoes = new List<Comp_Curricular>();
            for (int i = 0; i < compCurriculares.Count; i++)
            {
                //Verifica se o aluno ainda não foi aprovado na disciplina
                if(compCurriculares[i].Disponivel(aluno.GetSemestreAtual(), aluno.ListaCompleto()))
                {
                    opcoes.Add(compCurriculares[i]);
                }
            }

            if(opcoes.Count != 0)
            {
                List<Cadeira> cadeiraSemestre = aluno.ListaPorSemestre(aluno.GetSemestreAtual());
                if(cadeiraSemestre.Count > 0)
                {
                    if(cadeiraSemestre.Count != 1)
                    {
                        for(int i = 0; i < opcoes.Count; i++)
                        {
                            foreach (var h in cadeiraSemestre)
                            {
                                if(opcoes[i].GetCodigo() == h.GetCodigo())
                                {
                                    opcoes.Remove(opcoes[i]);
                                }
                            }
                        }
                    }
                    else 
                    {
                        for(int i = 0; i < opcoes.Count; i++)
                        {
                            if(opcoes[i].GetCodigo() == cadeiraSemestre[0].GetCodigo())
                            {
                                opcoes.Remove(opcoes[i]);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Você já terminou o curso. Não há mais caderias para se matricular.");
            }

            List<int> opcoesCodigo = new List<int>();
            Console.WriteLine("Você pode se matricular mas seguintes cadeiras:");
            foreach (var c in opcoes)
            {
                opcoesCodigo.Add(c.GetCodigo());
                Console.WriteLine("{0}-{1}", c.GetCodigo(), c.GetNome());
            }
            return opcoesCodigo;
        }

        private static void InserirMenu()
        {
            string inserirOpcoes = InserirOpcoes();

            while (maximoMatricula != 7 && inserirOpcoes != "3")
            {
                switch (inserirOpcoes)
                {
                    case "1":
                        InserirMatricula();
                        break;
                    case "2":
                        VerMatricula();
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                inserirOpcoes = InserirOpcoes();
            }
            if(maximoMatricula == 7)
            {
                Console.WriteLine("Você ultrapassou o preiodo máximo de seu curso.");
                Console.WriteLine("Você não pode mais se matricular");
                return;
            }
            aluno.SetSemestreAtual(aluno.GetSemestreAtual()+1);
            maximoMatricula = 0;
        }

        private static void InserirMatricula()
        {
            if(maximoMatricula == 5)
            {
                Console.WriteLine("Você já alcançou o limite de matricula desse semestre");
                return;
            }

            int semestre = aluno.GetSemestreAtual();

            if(semestre > aluno.GetSemestreMaximo())
            {
                maximoMatricula = 7;
            }
            
            List<int> opcoes = ListarCompCurricular();
            Console.WriteLine("Escolha qual cadeira você se matriculou?");
            int codigo = int.Parse(Console.ReadLine());
            while (!opcoes.Contains(codigo))
            {
                Console.WriteLine("Você inseriu um código errado. Tente novamente: ");
                codigo = int.Parse(Console.ReadLine());
            }
            string nome = Componentes.GetName(typeof(Componentes),codigo);

            Console.WriteLine("Qual foi a sua nota nessa cadeira?");
            double nota = double.Parse(Console.ReadLine());
            while (nota < 0 || nota > 10)
            {
                Console.WriteLine("Nota inválida. Tente novamente: ");
                nota = double.Parse(Console.ReadLine());
            }

            if(nota >= 5.0)
            {
                compCurriculares[codigo].SetAprovado(true);
            }

            aluno.Insere(codigo, nome, semestre, nota);
            maximoMatricula++;
        }

        private static void VerMatricula()
        {
            List<Cadeira> matriculaSemestre = aluno.ListaPorSemestre(aluno.GetSemestreAtual());
            if(matriculaSemestre.Count == 0)
            {
                Console.WriteLine("Sem matrícula no momento");
                InserirOpcoes();
            }
            Console.WriteLine("Você se matriculou nessas cadeiras: ");
            for (int i = 0; i < matriculaSemestre.Count; i++)
            {
                Console.WriteLine("{0} - {1}", matriculaSemestre[i].GetCodigo(), matriculaSemestre[i].GetNome());
            }
        }

        private static string InserirOpcoes()
        {
            Console.WriteLine();
            Console.WriteLine("Informe a opção desejada: ");
            Console.WriteLine("1- Registar matrícula");
            Console.WriteLine("2- Ver matrícula atual");
            Console.WriteLine("3- Terminar de registrar matrícula");
            Console.WriteLine("C- Limpar console");
            Console.WriteLine();

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }

        private static void VisualizarHistorico()
        {
            List<Cadeira> matriculaSemestre = aluno.ListaCompleto();
            if(matriculaSemestre.Count == 0)
            {
                Console.WriteLine("Você não realizou nenhuma matricula no momento");
                return;
            }
            Console.WriteLine(aluno.ToString());
        }

        private static void VisualizarSemestre()
        {
            Console.WriteLine("Informe qual semestre você quer ver: ");
            int entradaSemestre = int.Parse(Console.ReadLine());

            while (entradaSemestre > aluno.GetSemestreAtual() && entradaSemestre <= 0)
            {
                Console.WriteLine("Semestre ainda não matriculado. Tente novamente: ");
                Console.WriteLine("Informe qual semestre você quer ver: ");
                entradaSemestre = int.Parse(Console.ReadLine());
            }

            List<Cadeira> semestre = aluno.ListaPorSemestre(semestre: entradaSemestre);
            if(semestre.Count == 0)
            {
                Console.WriteLine("Você não realizou nenhuma matricula no momento");
                return;
            }
            string retorno = "";
            for (int i = 0,j = 0; i < aluno.GetSemestreAtual(); i++)
            {
                retorno += i +": " + Environment.NewLine;
                while (semestre[j].GetSemestre() != i)
                {
                    retorno += semestre[j].GetCodigo() + "\t";
                    retorno += semestre[j].GetNome() +"\t"; 
                    retorno += semestre[j].GetAprovacao() ? "Aprovado" : "Reprovado";
                    retorno += " com nota:\t" + semestre[j].GetNota() + Environment.NewLine;
                    ++j;
                }
            }
            Console.WriteLine(retorno);
        }

        private static void SalvarHitorico()
        {
            List<Cadeira> list = aluno.ListaCompleto();
            if(list.Count == 0)
            {
                Console.WriteLine("Sem informações para salvar");
                return;
            }
            
            string saida = aluno.ToString();
            using(StreamWriter sw = new StreamWriter("Historico.txt"))
            {
                sw.Write(saida);
                sw.Close();
            }
            
            
        }
        
        private static string ObterOpcaoUsuario()
        {
            Console.WriteLine();
            Console.WriteLine("DIO Histórico a seu dispor");
            Console.WriteLine("Você está no {0}º semestre", aluno.GetSemestreAtual());
            Console.WriteLine("Informe a opção desejada: ");
            Console.WriteLine("1- Mostrar Cadeiras disponíveis");
            Console.WriteLine("2- Inserir no Histórico");
            Console.WriteLine("3- Visualizar Histórico todo");
            Console.WriteLine("4- Visualizar Histórico de um semestre");
            Console.WriteLine("5- Visualizar o CRE");
            Console.WriteLine("C- Limpar console");
            Console.WriteLine("X- Salvar e Sair");
            Console.WriteLine();

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }
    }
}
