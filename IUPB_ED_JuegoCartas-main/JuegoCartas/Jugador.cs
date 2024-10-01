using System.Text;

namespace JuegoCartas
{
    public class Jugador
    {
        private const int TOTAL_CARTAS = 10;
        private Carta[] cartas;
        private Random r = new Random();
        public int Puntaje { get; private set; }

        public Jugador()
        {
            Puntaje = 0;
        }

        public void Repartir()
        {
            cartas = new Carta[TOTAL_CARTAS];
            for (int i = 0; i < TOTAL_CARTAS; i++)
            {
                cartas[i] = new Carta(r);
            }
        }

        public void Mostrar(ListView lv)
        {
            lv.Items.Clear();
            foreach (Carta carta in cartas)
            {
                carta.mostrar(lv);
            }
        }

        public string ObtenerResultado()
        {
            Puntaje = 0;
            StringBuilder resultado = new StringBuilder();

            Dictionary<NombreCarta, int> contadorCartas = new Dictionary<NombreCarta, int>();
            Dictionary<Pinta, List<NombreCarta>> cartasPorPinta = new Dictionary<Pinta, List<NombreCarta>>();

            // Inicializar contadores de cartas y agrupar por pinta
            foreach (NombreCarta nombre in Enum.GetValues(typeof(NombreCarta)))
            {
                contadorCartas[nombre] = 0;
            }
            foreach (Pinta pinta in Enum.GetValues(typeof(Pinta)))
            {
                cartasPorPinta[pinta] = new List<NombreCarta>();
            }

            // Contar cartas y agrupar por pinta
            foreach (Carta carta in cartas)
            {
                NombreCarta nombre = carta.ObtenerNombre();
                Pinta pinta = carta.ObtenerPinta();
                contadorCartas[nombre]++;
                cartasPorPinta[pinta].Add(nombre);

                // Calcular puntaje
                Puntaje += ObtenerValorCarta(nombre);
            }

            // Mostrar las cartas repetidas
            resultado.AppendLine("Cartas repetidas:");
            foreach (var entry in contadorCartas)
            {
                if (entry.Value >= 2)
                {
                    resultado.AppendLine($"{entry.Value} cartas de valor {entry.Key}");
                }
            }

            // Verificar y mostrar escaleras por pinta
            resultado.AppendLine("\nEscaleras:");
            foreach (var entry in cartasPorPinta)
            {
                var cartasDePinta = entry.Value.OrderBy(c => (int)c).ToList();
                int longitudEscalera = 1;
                NombreCarta inicioEscalera = cartasDePinta[0];

                for (int i = 1; i < cartasDePinta.Count; i++)
                {
                    if ((int)cartasDePinta[i] == (int)cartasDePinta[i - 1] + 1)
                    {
                        longitudEscalera++;
                    }
                    else
                    {
                        if (longitudEscalera >= 3)
                        {
                            resultado.AppendLine($"Escalera de {longitudEscalera} cartas de {inicioEscalera} a {cartasDePinta[i - 1]} en {entry.Key}");
                        }
                        longitudEscalera = 1;
                        inicioEscalera = cartasDePinta[i];
                    }
                }

                // Si la última secuencia también es una escalera
                if (longitudEscalera >= 3)
                {
                    resultado.AppendLine($"Escalera de {longitudEscalera} cartas de {inicioEscalera} a {cartasDePinta.Last()} en {entry.Key}");
                }
            }

            // Mostrar puntaje total
            resultado.AppendLine($"\nPuntaje total: {Puntaje}");

            return resultado.ToString();
        }

        private int ObtenerValorCarta(NombreCarta nombre)
        {
            switch (nombre)
            {
                case NombreCarta.AS:
                case NombreCarta.JACK:
                case NombreCarta.QUEEN:
                case NombreCarta.KING:
                    return 10;
                default:
                    return (int)nombre + 1; // Los números valen su número + 1
            }
        }
    }
}
