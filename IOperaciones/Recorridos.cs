using System;
using System.IO;

namespace IOperaciones
{
	public class Recorridos
	{
		public static int[] Dijkstra(Grafo m, int a, int b, out int w, out int c){
			// m[i,j] != 0 == (i,j) es una arista
			// m[0].len == m[1].len
			// 0 =< a < m[0].len /\ 0 =< b m[0].len
			w = 0;
			int[] R = new int[m.V];
			var L = new int[m.V];
			for (int i = 0; i < m.V; i++) {
				L [i] = int.MaxValue;
			}
			// L[i] = neutro de minimo
			L [a] = 0;
			// a es el vertice con menor L[i] asociado
			var F = new bool[m.V];
			for (int i = 0; i < m.V; i++) {
				F [i] = false;
			}
			// F[i] == el vertice i ha sido visitado
			int u = 0;
			bool connected = true;
			while (!F[b] && connected) {
				int min = int.MaxValue;
				for (int i = 0; i < m.V; i++) {
					if (!F[i] && L[i] < min) {
						u = i;
						min = L [i];
					}
				}
				connected = min != int.MaxValue;
				// u es el minimo valor de L no visitado
				F [u] = true;
				// u es visitado
				int ca;
				var r = m.Adyacentes (u, out ca);
				for (int i = 0; i < ca; i++) {
					int t = L [u] + m [u, r [i]];
					if (t < L[r[i]]) {
						L [r [i]] = t;
						R [r [i]] = u;
					}
				}
				// L tiene todos los minimos de los caminos desde a hasta i
			}
			var C = new int[m.V];
			if (connected) {
				w = L [u];
				// R[i] es el previo vertice a i en el camino que pasa por i
				R [0] = a;

				c = 0;
				for (int i = b; i != a; i=R[i]) {
					C [c] = i;
					c++;
				}
				C [c] = a;
				c++;
				// C de 0 a c tiene el camino mas corto entre a y b	
			} else {
				c = 0;
			}
			return C;
		}

		public static Grafo Cargar(string filename, out bool ok){
			Grafo g = null;
			ok = File.Exists (filename);
			if (ok) {
				var f = File.OpenText (filename);
				var s = f.ReadLine ();
				int n;
				ok = int.TryParse (s, out n);
				if (ok) {
					g = new Grafo (n);
				}
				s = f.ReadLine ();
				while (ok && s != null) {
					int[] pe = new int[3];  
					var edge = s.Split (",".ToCharArray(), pe.Length);
					ok = edge.Length == pe.Length; 
					for (int i = 0; ok && i != edge.Length; i++) {
						ok = int.TryParse (edge [i], out pe[i]);	
					}
					if (ok) {
						g.AnhadeArista (pe [0], pe [1], pe [2]);
					}
					s = f.ReadLine ();
				}
			}
			return g;
		}
	}

	public class Grafo {

		int[,] m;
		int e;

		public Grafo (int v)
		{
			m = new int[v, v];
			e = 0;
		}

		public int V {
			get { return m.GetLength (0);}
		}

		public int E {
			get { return e;}
		}

		public void AnhadeArista(int a, int b, int w){
			// 0 =< a < V /\ 0 =< b < V /\ a != b
			if (0 <= a && a < V && 0 <= b && b < V) {
				m [a, b] = w;
				e++;	
			}
		}

		public int[] Adyacentes(int u, out int c){
			var r = new int[V - 1];
			c = 0;
			for (int i = 0; i < V; i++) {
				if (m[u,i] != 0) {
					r [c] = i;
					c++;
				}
			}
			return r;
		}

		public int this[int i, int j] {
			get {return m[i,j];}
		}
	}
}

