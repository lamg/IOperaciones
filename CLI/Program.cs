using System;
using IOperaciones;

namespace CLI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			bool ok = false;
			if (args.Length == 3) {
				ok = true;
				var g = Recorridos.Cargar(args[0],out ok);
				int[] a = new int[2];
				for (int i = 0; ok && i < a.Length; i++) {
					ok = int.TryParse (args [i + 1], out a [i]);
				}
				if (ok) {
					int w,c;
					var r = Recorridos.Dijkstra (g, a[0], a[1], out w, out c);
					if (c == 0) {
						Console.WriteLine ("No hay camino");
					} else {
						for (int i = c-1; i != -1; i--) {
							Console.Write ("{0} ", r [i]);
						}
						Console.WriteLine ();
						Console.WriteLine ("w: {0}",w);	
					}
				}
			}
			if (!ok) {
				Console.WriteLine ("Sintaxis: CLI.exe archivo-con-formato-grafo #vertice_partida #vertice_llegada");
				Console.WriteLine ("Verifique que cada argumento tiene el formato correcto");
			}
		}
	}
}
